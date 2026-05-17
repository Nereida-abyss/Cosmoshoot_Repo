using UnityEngine;

public class AlienShipMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("Cámara")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookUpAngle = 80f;
    [SerializeField] private float maxLookDownAngle = 80f;

    [Header("Flotación")]
    [SerializeField] private float floatAmplitude = 0.2f;
    [SerializeField] private float floatSpeed = 3f;

    private float xRotation = 0f;
    private float startY;
    private float floatTimer = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Configuración para nave flotante
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.linearDamping = 1f;

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        startY = transform.position.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Movimiento lateral (A/D)
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = transform.right * horizontal * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        // Flotación (efecto de vaivén)
        floatTimer += Time.deltaTime * floatSpeed;
        float newY = startY + Mathf.Sin(floatTimer) * floatAmplitude;
        Vector3 floatPosition = rb.position;
        floatPosition.y = newY;
        rb.MovePosition(floatPosition);

        // Rotación de cámara con ratón
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookUpAngle, maxLookDownAngle);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Control del cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
