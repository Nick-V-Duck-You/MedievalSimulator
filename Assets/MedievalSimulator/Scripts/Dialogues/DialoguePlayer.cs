using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePlayer : MonoBehaviour
{
    public GameObject DialogueUI;

    [SerializeField] private Text LineUI;
    [SerializeField] private Text NameUI;

    [SerializeField] public string DialogueKey;

    public GameObject triggerObject; // Assign this in the Inspector
    public GameObject Player; // Assign this in the Inspector
    private static PlayerController playerController;

    public float distance;

    public bool isDialoguePlayed;
    public bool isDialogueStarted;

    [Header("Actions after the dialogue ends")]
    [SerializeField] public bool isDestroyRecuired;
    [SerializeField] public bool isSpecialActionRecuired;
    [SerializeField] public GameObject GameObjectWithSpecialAction;


    //public bool isDialogueStarted;

    public int lineNumberFmod;

    public JsonParser JsonParser;

    //private FMODUnity.StudioEventEmitter _emitter;

    
    [SerializeField] private FMODUnity.EventReference _FMD_NPC_Dialogue;
    private FMOD.Studio.EventInstance _CharIsTalking;
   
    [SerializeField]
    public enum DialogueName
    {
        D_kuplinov,
        D_Losyash
    }

    
    

    [SerializeField] public DialogueName dialogueNameFmod;

    private void Awake()
    {
        // поиск PlayerController по тегу (это нужно, чтобы находить управление гг и  останваливать/возобнавляеть его работу при диалоге)
        if (playerController == null)
        {
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
                playerController = playerGO.GetComponent<PlayerController>();
            else
                Debug.LogError("DialoguePlayer: не найден объект с тегом Player");
        }

        // пока не вырезаю код с эммитером и оставляю как шаргалку, если никому не надо снесите, это хорошее решение для постоянных звуков на карте, для диалогов с параметрами лучше через инстансы, 
        /* кешируем FMOD эмиттер
        _emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        if (_emitter == null)
        Debug.LogError("DialoguePlayer: нет StudioEventEmitter на объекте");
        */
    }

    void Start()
    {
        DialogueUI.SetActive(false);
        isDialoguePlayed = false;
        isDialogueStarted = false;
        lineNumberFmod = 0; // Крош починил, всё работает как задумано
       
    }

    void Update()
    {
        distance = Vector3.Distance(triggerObject.transform.position, Player.transform.position);

        // ---- СТАРТ ДИАЛОГА ОДИН РАЗ ПО ENTER ----
        if (distance < 5f && !isDialogueStarted && !isDialoguePlayed && Input.GetKeyDown(KeyCode.Return))
        {
            // грузим первую строку и ставим параметры FMOD и играем первую реплику
            JsonParser.LineFiller(DialogueKey);
            PlayDialogue();

            // ставим параметры FMOD и играем первую реплику
            /*/ if (_emitter != null)
             {
                 _emitter.SetParameter("Dialogue Name", (float)dialogueNameFmod);
                 _emitter.SetParameter("Line Number", lineNumberFmod);
                 _emitter.Play();
             }
             /*/
            
            _CharIsTalking = FMODUnity.RuntimeManager.CreateInstance(_FMD_NPC_Dialogue);

            // почему эта хрень не работает большой вопрос, мб enum to string  как то надо присрать попробовать
            //_CharIsTalking.setParameterByNameWithLabel("Dialogue Name", dialogueNameFmod);
            

            _CharIsTalking.start();
            
            //_CharIsTalking.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(_CharIsTalking,gameObject);


            //Debug.LogError("DialoguePlayer: sound is");


            isDialogueStarted = true;
            return;

        }

        if (!isDialogueStarted || isDialoguePlayed || !DialogueUI.activeSelf)
            return;

        // ---- ПРОЛИСТЫВАНИЕ РЕПЛИК ПО ENTER ----
        if (Input.GetKeyDown(KeyCode.Return))
        {
            List<DialogueLinesData> currentDialogueList =
                (List<DialogueLinesData>)typeof(DialoguesContainer)
                    .GetField(DialogueKey)
                    .GetValue(JsonParser._dialogueDatabase);

            // не последняя строка - перелистываем дальше
            if (JsonParser.currentLineID < currentDialogueList.Count - 1)
            {
                Debug.Log($"Enter pressed, switching to next dialogue line (ID: {JsonParser.currentLineID}).");
                JsonParser.currentLineID += 1;
                lineNumberFmod += 1;

                // обновляем текст из JSON
                JsonParser.LineFiller(DialogueKey);
                NameUI.text = JsonParser.currentLine.Character;
                LineUI.text = JsonParser.currentLine.Line;

                //_CharIsTalking.setParameterByNameWithLabel

                _CharIsTalking.setParameterByName("Line Number", lineNumberFmod);
                _CharIsTalking.start();
               

                // обновляем звук
                /*
                if (_emitter != null)
                {
                    _emitter.SetParameter("line number", lineNumberFmod);
                    _emitter.Play();
                }
                */
            }

            // последняя строка - закрываем диалог
            else
            {
                Debug.Log("Keypad Enter key pressed! DialogueUI was closed");
                JsonParser.currentLineID = 0;
                isDialoguePlayed = true;
                isDialogueStarted = false;
                DialogueUI.SetActive(false);
                _CharIsTalking.release();
                EndDialogue();
            }
        }
    }

    public void PlayDialogue()
    {
        if (!isDialoguePlayed)
        {
            // включаем видимость курсора 
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // Останавливаем время
            Time.timeScale = 0f;

            // Отключаем управление персонажем
            if (playerController != null)
                playerController.SetActive(false);

            DialogueUI.SetActive(true);
            //вытаскиваем из реплики по ключам имя персонажа и реплику
            NameUI.text = JsonParser.currentLine.Character;
            LineUI.text = JsonParser.currentLine.Line;

            /*
             if (_emitter != null)
            {
                _emitter.SetParameter("Dialogue Name", (float)dialogueNameFmod);
            }
            */
        }
        
    }

    public void EndDialogue()
    {
        // Возвращаем игровой режим
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Возвращаем время
        Time.timeScale = 1f;

        // Включаем управление персонажем
        if (playerController != null)
            playerController.SetActive(true);

        if (isDestroyRecuired)
        {
            Destroy(gameObject);
        }
        if (isSpecialActionRecuired)
        {
            GameObjectWithSpecialAction.GetComponent<SpecialAction>().Action();
        }
    }
}
