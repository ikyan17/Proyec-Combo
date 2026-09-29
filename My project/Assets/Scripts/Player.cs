using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    public static Player instancia;

    [Header("Referencias")]
    public Animator animator; // Al ser public, Unity la muestra en el Inspector sin [SerializeField]
    private SkillManager skillManager;

    [Header("Gráficos y Físicas")]
    [SerializeField] private GameObject modeloVisual;
    [SerializeField] private Collider colliderJugador;

    [Header("Cámara y Seguimiento")]
    [SerializeField] private Transform puntoCamaraJugador;
    private Transform camaraPrincipalTransform;

    [Header("Estadísticas")]
    public int puntosDeHabilidad = 5; // Al ser public, se ve en el Inspector sin atributos extra
    [HideInInspector] public int contador;

    private Possessable objetivoPoseido;

    protected override void Awake()
    {
        base.Awake();
        if (instancia == null) instancia = this;
        else Destroy(gameObject);

        skillManager = GetComponent<SkillManager>();
        if (Camera.main != null)
            camaraPrincipalTransform = Camera.main.transform;
    }

    protected override void Update()
    {
        base.Update();

        if (objetivoPoseido == null)
        {
            Move(estaCorriendo);
            ActualizarAnimaciones();
        }
        else
        {
            objetivoPoseido.MoverEnemigo(inputMove, estaCorriendo);
        }
    }

    public void OnMove(InputValue value)
    {
        inputMove = value.Get<Vector2>();
    }

    public void OnSprint(InputValue value)
    {
        if (value.isPressed)
        {
            estaCorriendo = !estaCorriendo;
        }


    }
    public void OnJump(InputValue value)
    {
        if (value.isPressed && objetivoPoseido == null) Jump();
    }

    // --- MÁQUINA DE ESTADOS / 4 TIPOS DE ATAQUE ---
    // --- MÁQUINA DE ESTADOS / ATAQUES OPTIMIZADOS ---
    public void OnAttack(InputValue value) => ProcesarEntradaAtaque(value, 0);
    public void OnAttack1(InputValue value) => ProcesarEntradaAtaque(value, 1);
    public void OnAttack2(InputValue value) => ProcesarEntradaAtaque(value, 2);
    public void OnAttack3(InputValue value) => ProcesarEntradaAtaque(value, 3);

    private void ProcesarEntradaAtaque(InputValue value, int indiceAtaque)
    {
        if (value.isPressed && objetivoPoseido == null)
        {
            skillManager?.EjecutarHabilidad(indiceAtaque);
        }
    }

    public void AtacarEnemigo(Character enemigoObjetivo, float cantidadDaño)
    {
        if (enemigoObjetivo != null)
        {
            enemigoObjetivo.TakeDamage(cantidadDaño);
            Debug.Log("El Player atacó a {enemigoObjetivo.gameObject.name} causando {cantidadDaño} de daño.");
        }
    }

    public void OnPossess(InputValue value)
    {
        if (value.isPressed)
        {
            if (objetivoPoseido != null) IntentarPosesionEnCadena();
            else IntentarPosesion();
        }
    }

    public void OnUnpossess(InputValue value)
    {
        if (value.isPressed && objetivoPoseido != null) TerminarPosesion();
    }

    private void IntentarPosesion()
    {
        if (Camera.main == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 20f))
        {
            if (hit.collider.TryGetComponent<Possessable>(out var enemigo))
            {
                objetivoPoseido = enemigo;
                objetivoPoseido.SerPoseido();
                CambiarEstadoCuerpoOriginal(false);
            }
        }
    }

    private void IntentarPosesionEnCadena()
    {
        if (Camera.main == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 20f))
        {
            if (hit.collider.TryGetComponent<Possessable>(out var nuevoEnemigo) && nuevoEnemigo != objetivoPoseido)
            {
                objetivoPoseido.RestaurarIA();
                objetivoPoseido = nuevoEnemigo;
                objetivoPoseido.SerPoseido();
            }
        }
    }

    private void TerminarPosesion()
    {
        if (objetivoPoseido != null)
        {
            objetivoPoseido.RestaurarIA();
            transform.position = objetivoPoseido.transform.position;
            objetivoPoseido = null;
            CambiarEstadoCuerpoOriginal(true);
        }
    }

    private void CambiarEstadoCuerpoOriginal(bool activo)
    {
        if (modeloVisual != null) modeloVisual.SetActive(activo);
        if (colliderJugador != null) colliderJugador.enabled = activo;
        if (rb != null) rb.isKinematic = !activo;
    }

    public void RestarPuntosHabilidad(int cantidad)
    {
        puntosDeHabilidad -= cantidad;
    }

    public void SumarPuntosDeHabilidad(int cantidad)
    {
        puntosDeHabilidad += cantidad;
    }

    private void ActualizarAnimaciones()
    {
        if (animator == null) return;

        float multiplicadorCorrer = estaCorriendo ? 2f : 1f;

        float velX = inputMove.x * multiplicadorCorrer;
        float velZ = inputMove.y * multiplicadorCorrer;

        animator.SetFloat("VelX", velX);
        animator.SetFloat("VelZ", velZ);
        animator.SetBool("isSprinting", estaCorriendo);
    }
}