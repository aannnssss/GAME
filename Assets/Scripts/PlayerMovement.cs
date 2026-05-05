using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Настройки движения")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 3f;
    
    [Header("Ссылки")]
    public Rigidbody rb;
    public Transform cameraTransform; // ← Перетащи сюда камеру из инспектора
    public CapsuleCollider playerCollider; // ← Перетащи коллайдер игрока
    
    [Header("Настройки приседания")]
    public float standHeight = 2f;
    public float crouchHeight = 1.2f;
    public float crouchCameraOffset = -0.5f;
    
    private float defaultCameraY;
    private bool isCrouching = false;
    private Vector3 originalColliderCenter;

    void Start()
    {
        if (cameraTransform != null)
            defaultCameraY = cameraTransform.localPosition.y;
        
        if (playerCollider != null)
            originalColliderCenter = playerCollider.center;
    }

    void FixedUpdate()
    {
        // Определяем текущую скорость
        float currentSpeed = walkSpeed;
        
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            currentSpeed = sprintSpeed;
        
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            currentSpeed = crouchSpeed;
            if (!isCrouching) StartCrouch();
        }
        else
        {
            if (isCrouching) StopCrouch();
        }

        // Движение
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        rb.MovePosition(rb.position + move * currentSpeed * Time.fixedDeltaTime);
    }

    void StartCrouch()
    {
        isCrouching = true;
        
        // Опускаем камеру
        if (cameraTransform != null)
        {
            Vector3 camPos = cameraTransform.localPosition;
            camPos.y = defaultCameraY + crouchCameraOffset;
            cameraTransform.localPosition = camPos;
        }
        
        // Уменьшаем коллайдер (если есть)
        if (playerCollider != null)
        {
            playerCollider.height = crouchHeight;
            Vector3 center = originalColliderCenter;
            center.y = crouchHeight / 2f;
            playerCollider.center = center;
        }
    }

    void StopCrouch()
    {
        isCrouching = false;
        
        // Возвращаем камеру
        if (cameraTransform != null)
        {
            Vector3 camPos = cameraTransform.localPosition;
            camPos.y = defaultCameraY;
            cameraTransform.localPosition = camPos;
        }
        
        // Возвращаем коллайдер
        if (playerCollider != null)
        {
            playerCollider.height = standHeight;
            playerCollider.center = originalColliderCenter;
        }
    }
}