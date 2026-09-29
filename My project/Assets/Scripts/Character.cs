using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Transform laserTag;
    [SerializeField] protected float vel;
    [SerializeField] protected float velocidadCorrer = 8f;
    [SerializeField] protected float fuerzaSalto = 5f;
    [SerializeField] protected float m_vidaMaxima;

    protected float m_vidaActual;
    protected Vector2 inputMove;
    protected Rigidbody rb;
    protected bool estaCorriendo = false;
    protected bool isDead = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        m_vidaActual = m_vidaMaxima;
    }

    protected virtual void Update()
    {
        // Validación pasiva por si la vida baja a 0 sin pasar por TakeDamage (ej. desde el Inspector)
        if (m_vidaActual <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Move(bool corriendo)
    {
        Vector3 direction = new Vector3(inputMove.x, 0, inputMove.y);
        float velocidadActual = corriendo ? velocidadCorrer : vel;
        transform.Translate(direction * velocidadActual * Time.deltaTime);
    }

    public void Jump()
    {
        if (EstaEnSuelo())
        {
            // Aplicamos la fuerza de salto al Rigidbody manteniendo la velocidad actual en los otros ejes
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, fuerzaSalto, rb.linearVelocity.z);
        }
    }

    public bool EstaEnSuelo()
    {
        if (Physics.Raycast(laserTag.position, Vector3.down, out RaycastHit hit, 0.5f))
        {
            return hit.collider.CompareTag("Suelo");
        }
        return false;
    }

    public virtual void TakeDamage(float damage)
    {
        m_vidaActual -= damage;
        if (m_vidaActual <= 0 && !isDead) Die();
    }

    public virtual void TakeDamage(float damage, float multiplicadorCritico)
    {
        m_vidaActual -= (damage * multiplicadorCritico);
        if (m_vidaActual <= 0 && !isDead) Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        gameObject.SetActive(false);
    }

    public void SetInput(Vector2 nuevoInput)
    {
        inputMove = nuevoInput;
    }
} 