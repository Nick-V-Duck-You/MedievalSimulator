using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static SimpleDialogueManager;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class JsonParser : MonoBehaviour
{
    public GameObject DialogueUI;

    [SerializeField] private Text LineUI;
    [SerializeField] private Text NameUI;

    public DialoguesContainer _dialogueDatabase;
    public DialogueLinesData firstLine;

    public GameObject triggerObject; // Assign this in the Inspector
    public GameObject Player; // Assign this in the Inspector

    public float distance;

    public int currentLineID;

    void Start()
    {
        DialogueUI.SetActive(false);

        currentLineID = 0;

        // Загружаем из папки ресурсес жсон 
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Dialogues");

        //записываем всю эту хуету из жсон строки в объект класса DialoguesContainer (контейнер по ключу верхнего уровня, короче там список диалогов)
        _dialogueDatabase = JsonUtility.FromJson<DialoguesContainer>(jsonTextAsset.text);

        //создаем объект класса DialogueLinesData (реплика короче) и запихиваем в него информацию из контейнера Диалог 1 реплики с индексом 0
        //firstLine = _dialogueDatabase.Dialogue1[currentLineID];

    }

    void Update()
    {
        //создаем объект класса DialogueLinesData (реплика короче) и запихиваем в него информацию из контейнера Диалог 1 реплики с индексом 0
        firstLine = _dialogueDatabase.Dialogue1[currentLineID];

        distance = Vector3.Distance(triggerObject.transform.position, Player.transform.position);
        if (distance < 5f){
            PlayDialogue();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Keypad Enter key pressed!");
                currentLineID += 1;
            }
        }


    }

    public void PlayDialogue()
    {
        DialogueUI.SetActive(true);
        //вытаскиваем из реплики по ключам имя персонажа и реплику
        NameUI.text = firstLine.Character;
        LineUI.text = firstLine.Line;
    }
}
