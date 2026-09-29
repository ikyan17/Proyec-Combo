using UnityEngine;

public class EnemigoMelee_IA : EnemigoBase
{
    
    [SerializeField] private float visionRange = 12f;
    [SerializeField] private float dañoAtaque = 10f;
    private Transform playerObjetivo;

    // public: Para que el script de Posesión pueda apagar esta IA
    public bool iaActiva = true;

    // CORRECCIÓN: Cambiado de 'private void Update()' a 'protected override void Update()'
    protected override void Update()
    {
        base.Update(); // Ejecuta la validación de vida de Character / EnemigoBase

        if (!iaActiva) return; // Si está poseído, la IA no hace nada[cite: 3]

        BuscarJugadorMasCercano();
        Perseguir();
    }

    private void BuscarJugadorMasCercano()
    {
        GameObject[] jugadores = GameObject.FindGameObjectsWithTag("Player");

        float distanciaMasCorta = Mathf.Infinity;
        Transform jugadorCercano = null;

        foreach (GameObject p in jugadores)
        {
            float distancia = Vector3.Distance(transform.position, p.transform.position);
            if (distancia < distanciaMasCorta)
            {
                distanciaMasCorta = distancia;
                jugadorCercano = p.transform;
            }
        }

        playerObjetivo = jugadorCercano;
    }

    private void Perseguir()
    {
        if (playerObjetivo == null) return;

        float distancia = Vector3.Distance(transform.position, playerObjetivo.position);

        // Distancia mínima para que se detenga justo antes de tocarte y no baile a tu alrededor
        float distanciaDeDetencion = 1.5f;

        if (distancia > distanciaDeDetencion && distancia < visionRange)
        {
            Vector3 direccion = (playerObjetivo.position - transform.position).normalized;
            inputMove = new Vector2(direccion.x, direccion.z);
            Move(true);

            // Rotación suave hacia el jugador evitando giros bruscos o vueltas en círculos
            if (direccion != Vector3.zero)
            {
                Quaternion rotacionObjetivo = Quaternion.LookRotation(new Vector3(direccion.x, 0, direccion.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, 10f * Time.deltaTime);
            }
        }
        else
        {
            // Si está muy cerca o fuera de rango, detiene el movimiento
            inputMove = Vector2.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!iaActiva) return; // Si está poseído, la IA no ataca ni hace daño por contacto[cite: 3, 11]

        if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent<Player>(out var jugador))
        {
            // Como heredamos de EnemigoBase, podemos usar directamente DañoAtaqueEnemigo o la variable local dañoAtaque
            jugador.TakeDamage(dañoAtaque);
            Debug.Log($"[Combate] {gameObject.name} golpeó al jugador y le causó {dañoAtaque} de daño.");
        }
    }

    // Método para modificar el daño del enemigo dinámicamente desde otro script
    public void SetDañoAtaque(float nuevoDaño)
    {
        dañoAtaque = nuevoDaño;
    }
}