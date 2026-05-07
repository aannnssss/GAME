using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 120f; // скорость поворота
    public Transform playerBody;

    float xRotation = 0f; // накопители углов поворота
    float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Вертикальный поворот камеры (вверх/вниз)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Горизонтальный поворот игрока (влево/вправо)
        yRotation += mouseX;
        playerBody.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}