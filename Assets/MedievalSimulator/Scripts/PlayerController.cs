using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;

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

    
    private float SurfIndex;             //Индекс на переменную поверхности фмода
    private RaycastHit RH_SurfaceTag;   //Луч материалчекера бьющий в линзу тегов
    private float RH_Distance = 1.2f;   //длинна луча 
    public LayerMask SurfaceLayer;      // на каком слое проверяет тэг
    [SerializeField] public float fs_walk = 0.3f; //рекомедуемые значения для триггера шагов
    [SerializeField] private float fs_run = 0.2f;
    public float fs_triggerrate;

    [SerializeField] private FMODUnity.EventReference Testsound;
    private EventInstance _StepInst; // это звуки для шагов 
    private Coroutine stepRoutine;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _StepInst = FMODUnity.RuntimeManager.CreateInstance(Testsound); // это звуки для шагов 
        fs_triggerrate = fs_walk;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * RH_Distance, Color.cyan);
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

        fs_triggerrate = isRunning ? fs_run : fs_walk;

        if (characterController.isGrounded && isMoving)
        {
            SurfaceTagCheck();
            if (stepRoutine == null)
            {
                stepRoutine = StartCoroutine(PlayStepsRoutine());
            }
        }
        else
        {
            // Если мы не двигаемся или в воздухе, останавливаем корутину
            if (stepRoutine != null)
            {
                StopCoroutine(stepRoutine);
                stepRoutine = null; // Сбрасываем ссылку, чтобы можно было запустить снова
                _StepInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // это звуки для шагов 
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

    void SurfaceTagCheck() //селектор поверхности для шагов
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RH_SurfaceTag, RH_Distance))
            switch (RH_SurfaceTag.collider.tag)
            { 
                case "Surf_Earth":
                SurfIndex = 0;
                 break;

                case "Surf_Grass":
                SurfIndex = 1;
                 break;

                case "Surf_Stone":
                SurfIndex = 2;
                 break;

                default:
                SurfIndex = 0;
                 break;
            }
    }

    private IEnumerator PlayStepsRoutine()
    {
        while (true)
        {
            //ждем интервал чтобы не играть звук сразу
            yield return new WaitForSeconds(fs_triggerrate);

            PlayFM_Steps();
        }
    }

    void PlayFM_Steps() // играем шаги
    {
        //Debug.Log("Звук Growl вызван в секунду: " + Time.time); // Добавьте эту строку
        bool isRunning = (fs_triggerrate == fs_run);
        EventInstance _StepInst = RuntimeManager.CreateInstance(Testsound);

        fs_triggerrate = isRunning ? fs_run : fs_walk;

         _StepInst.setParameterByName("fmp_IsRunning", isRunning ? 1f : 0f, true);
        _StepInst.setParameterByName("fmp_SurfIndex", SurfIndex, true);

        _StepInst.start();
        _StepInst.release();

    }
}
