using UnityEngine;
using UnityEngine.InputSystem;

public class LookAtMouse : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float minDistanceThreshold = 0.2f; 

    private Camera mainCamera;
    private Vector3 targetDirection;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        
        if (Mouse.current == null) return;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hitInfo.point;
            targetPosition.y = transform.position.y;

            Vector3 direction = targetPosition - transform.position;

            if (direction.sqrMagnitude > minDistanceThreshold * minDistanceThreshold)
            {
                targetDirection = direction.normalized;
            }
        }
    }

    private void LateUpdate()
    {
      
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}