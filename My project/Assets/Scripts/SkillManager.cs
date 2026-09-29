using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
   
    [SerializeField] private List<Skill> habilidades = new List<Skill>();

    
    [SerializeField] private GameObject objetoEspada;
    [SerializeField] private float duracionAtaqueEspada = 0.5f;
    [SerializeField] private float dañoBaseEspada = 15f;

    private float multiplicadorDaño = 1f;
    private Player jugador;
    private Coroutine corrutinaAtaque;
    private HitboxEspada hitboxEspadaScript;

    private void Awake()
    {
        jugador = GetComponent<Player>();

        if (objetoEspada != null)
        {
            hitboxEspadaScript = objetoEspada.GetComponent<HitboxEspada>();
            objetoEspada.SetActive(false);
        }

        if (habilidades.Count > 0)
        {
            habilidades[0].Desbloquear();
        }
    }

    public void EjecutarHabilidad(int indice)
    {
        // Validar que el índice esté dentro del rango de la lista configurada
        if (indice < 0 || indice >= habilidades.Count)
        {
            Debug.LogWarning($"[SkillManager] El índice de ataque {indice} está fuera de los límites de la lista.");
        }
        else
        {
            Skill habilidadSeleccionada = habilidades[indice];

            // Evaluar si la habilidad requiere ser desbloqueada primero
            if (!habilidadSeleccionada.EstaDesbloqueada())
            {
                IntentarDesbloquear(habilidadSeleccionada);
            }
            else if (hitboxEspadaScript != null)
            {
                // Si está desbloqueada, configuramos el daño y ejecutamos el ataque
                float dañoFinal = dañoBaseEspada * multiplicadorDaño;
                hitboxEspadaScript.ConfigurarDaño(dañoFinal);

                if (jugador.animator != null && !string.IsNullOrEmpty(habilidadSeleccionada.triggerAnimacion))
                {
                    jugador.animator.SetTrigger(habilidadSeleccionada.triggerAnimacion);
                }

                if (corrutinaAtaque != null)
                {
                    StopCoroutine(corrutinaAtaque);
                }

                corrutinaAtaque = StartCoroutine(ActivarEspadaPorTiempo());
            }
        }
    }
    private void IntentarDesbloquear(Skill habilidad)
    {
        if (jugador.puntosDeHabilidad >= habilidad.costo)
        {
            jugador.RestarPuntosHabilidad(habilidad.costo);
            habilidad.Desbloquear();
            Debug.Log($"¡Habilidad '{habilidad.nombre}' Desbloqueada!");
        }
        else
        {
            Debug.LogWarning($"Faltan puntos para '{habilidad.nombre}'. Tienes {jugador.puntosDeHabilidad}, necesitas {habilidad.costo}.");
        }
    }

    private IEnumerator ActivarEspadaPorTiempo()
    {
        if (objetoEspada != null) objetoEspada.SetActive(true);
        yield return new WaitForSeconds(duracionAtaqueEspada);
        if (objetoEspada != null) objetoEspada.SetActive(false);
    }

    public void ModificarDañoBase(float nuevoDaño)
    {
        dañoBaseEspada = nuevoDaño;
    }

    public void AumentarMultiplicadorDaño(float extraMultiplicador)
    {
        multiplicadorDaño += extraMultiplicador;
    }
}