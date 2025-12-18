using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform _cameraTransform;

    [Header("Settings")]
    [SerializeField] private float _gravity = -14f;
    [SerializeField] public float _speed = 5f;
    [SerializeField] private float _speedRun = 10f;

    [Range(1, 100)]
    [SerializeField] private float _sensetivity = 50f;

    private float rotationX;
    private Vector3 velocity;
    Vector3 move;
    private bool isActive = true;

    [SerializeField] private FMODUnity.EventReference Testsound;
    private FMOD.Studio.EventInstance _growl; // это звуки для шагов извините за нейминг потом поменяю...

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _growl = FMODUnity.RuntimeManager.CreateInstance(Testsound); // это звуки для шагов извините за нейминг потом поменяю...

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

        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (characterController.isGrounded && isMoving)
        {
            if (!IsInvoking("Growl"))// это звуки для шагов извините за нейминг потом поменяю...
            {
                Debug.Log("Запуск InvokeRepeating в секунду: " + Time.time); // Добавьте эту строку
                InvokeRepeating("Growl", 0f, 0.6f); // нужно включить ваншот
            }
        }
        else
        {
            // Если мы не двигаемся или в воздухе, останавливаем повторение
            if (IsInvoking("Growl"))
            {
                CancelInvoke("Growl"); // Останавливаем шаги
                _growl.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // это звуки для шагов извините за нейминг потом поменяю...
            }
        }
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

    void Growl() // это звуки для шагов извините за нейминг потом поменяю...
    {
        Debug.Log("Звук Growl вызван в секунду: " + Time.time); // Добавьте эту строку
        PLAYBACK_STATE playbackState;
        _growl.getPlaybackState(out playbackState);
        if (playbackState != PLAYBACK_STATE.PLAYING)
        {
            _growl.start();
        }
    }
}
