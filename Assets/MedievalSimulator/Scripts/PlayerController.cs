using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform _cameraTransform;

    [Header("Settings")]
    [SerializeField] private float _gravity = -14f;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _speedRun = 10f;

    [Range(1, 100)]
    [SerializeField] private float _sensetivity = 50f;

    private float rotationX;
    private Vector3 velocity;
    Vector3 move;
    private bool isActive = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!isActive) return;

        Rotate();
        Move();
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensetivity;
        float mouseY = Input.GetAxis("Mouse Y") * _sensetivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        _cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? _speedRun : _speed;

        characterController.Move(move * currentSpeed * Time.deltaTime);

        if (characterController.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += _gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public Vector3 CurrentVelocity
    {
        get
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 move = transform.right * moveX + transform.forward * moveZ;

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? _speedRun : _speed;

            return move.normalized * currentSpeed;
        }
    }
    public void SetActive(bool active)
    {
        isActive = active;
    }
}
