using UnityEngine;

public class HitboxEspada : MonoBehaviour
{
    private float dañoAtaque = 10f;

    public void ConfigurarDaño(float nuevoDaño)
    {
        dañoAtaque = nuevoDaño;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Evitar golpear al propio jugador
        if (Player.instancia != null && other.gameObject == Player.instancia.gameObject) return;

        // 2. Verificar si el objeto golpeado tiene un componente Character o EnemigoBase
        if (other.TryGetComponent<Character>(out var objetivo))
        {
            if (objetivo == Player.instancia) return;

            // 3. Aplicar daño directamente al enemigo impactado
            if (Player.instancia != null)
            {
                Player.instancia.AtacarEnemigo(objetivo, dañoAtaque);
                Debug.Log($"[HitboxEspada] ¡Golpe exitoso a {other.gameObject.name} con {dañoAtaque} de daño!");
            }
        }
    }
}