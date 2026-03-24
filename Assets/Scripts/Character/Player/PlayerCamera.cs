using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float sensitivityModifier = 1f;

    public Transform playerBody;

    private float xRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LockCursor();
    }

    // Update is called once per frame
    void Update()
    {
       // float mouseX = inputRegistry.instance.MouseInput.x * mouseSensitivity * Time.deltaTime;
       // float mouseY = inputRegistry.instance.MouseInput.y * mouseSensitivity * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime * sensitivityModifier;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime * sensitivityModifier;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
