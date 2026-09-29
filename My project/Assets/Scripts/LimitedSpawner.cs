using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedSpawner : MonoBehaviour
{
    [Header("Configuración de Puntos de Spawn")]
    [SerializeField] private List<PuntoSpawn> puntosSpawn = new List<PuntoSpawn>();

    [Header("Límites y Tiempos")]
    [SerializeField] private int limiteMaximo = 5;
    [SerializeField] private float intervaloSpawn = 3f;

    private int contadorEnemigos = 0;

    private void Start()
    {
        StartCoroutine(RutinaSpawn());
    }

    private IEnumerator RutinaSpawn()
    {
        if (puntosSpawn.Count == 0) yield break;

        while (contadorEnemigos < limiteMaximo)
        {
            int indice = Random.Range(0, puntosSpawn.Count);
            PuntoSpawn seleccionado = puntosSpawn[indice];

            if (seleccionado.pivote != null && seleccionado.enemigoPrefab != null)
            {
                Instantiate(seleccionado.enemigoPrefab, seleccionado.pivote.position, seleccionado.pivote.rotation);
                contadorEnemigos++;
            }

            yield return new WaitForSeconds(intervaloSpawn);
        }
    }
}