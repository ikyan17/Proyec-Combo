using UnityEngine;


public class EnemigoBase : Character
{
    [Header("Estadísticas del Enemigo")]
    [SerializeField] private float dañoAtaqueEnemigo = 10f; // Daño que el enemigo hace al jugador

    [Header("Recompensas")]
    [SerializeField] protected int puntosDeHabilidad = 1;
    [SerializeField] protected GameObject prefabMoneda;

    // Getter para que otros scripts (como la IA) sepan cuánto daño hace
    public float DañoAtaqueEnemigo => dañoAtaqueEnemigo;

  

    public override void TakeDamage(float damage, float multiplicadorCritico)
    {
        float dañoTotal = damage * multiplicadorCritico;
        base.TakeDamage(damage, multiplicadorCritico);
        Debug.Log($"[Combate] {gameObject.name} recibió {dañoTotal} de daño crítico. Vida restante: {m_vidaActual}/{m_vidaMaxima}");
    }

    protected override void Die()
    {
        if (m_vidaActual <= 0)
        {
            SoltarRecompensas();
            base.Die();
        }
    }

    private void SoltarRecompensas()
    {
        
        
      Instantiate(prefabMoneda, transform.position, Quaternion.identity);
    }
}