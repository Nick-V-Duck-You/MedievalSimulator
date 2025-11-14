using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialoguePlayer : MonoBehaviour
{
    public GameObject DialogueUI;

    [SerializeField] private Text LineUI;
    [SerializeField] private Text NameUI;

    [SerializeField] public string DialogueKey;

    public GameObject triggerObject; // Assign this in the Inspector
    public GameObject Player; // Assign this in the Inspector

    public float distance;

    public bool isDialoguePlayed;

    public JsonParser JsonParser;
    void Start()
    {
        DialogueUI.SetActive(false);
        isDialoguePlayed = false;
    }

    void Update()
    {
        distance = Vector3.Distance(triggerObject.transform.position, Player.transform.position);
        if (distance < 5f){

            JsonParser.LineFiller(DialogueKey);// НАДО ИСПРАВИТЬ МНЕ НЕ НРАВИТСЯ ЧТО ОНО ВЫЗЫВАЕТСЯ 60 РАЗ В СЕКУНДУ
            PlayDialogue();

            /* typeof возвращает тип для DialoguesContainer
            .GetField(TargetDialogueName) ищет внутри DialoguesContainer то, что является значением TargetDialogueName (дает ебучий указатель)
            .GetValue() достает по указателю содержимое типа object
            (List<DialogueLinesData>) преобразует в тип List<DialogueLinesData>
            */
            List<DialogueLinesData> currentDialogueList = (List<DialogueLinesData>)typeof(DialoguesContainer) 
                .GetField(DialogueKey)
                .GetValue(JsonParser._dialogueDatabase);

            if ((Input.GetKeyDown(KeyCode.Return)) & (JsonParser.currentLineID < currentDialogueList.Count - 1))
            {
                Debug.Log("Keypad Enter key pressed!");
                JsonParser.currentLineID += 1;
            }
            else if ((Input.GetKeyDown(KeyCode.Return)) & (JsonParser.currentLineID == currentDialogueList.Count - 1))
            {
                Debug.Log("Keypad Enter key pressed! DialogueUI was closed");
                JsonParser.currentLineID = 0;
                DialogueUI.SetActive(false);
                isDialoguePlayed = true;
                Destroy(gameObject);
            }
        }
    }

    public void PlayDialogue()
    {
        if (!isDialoguePlayed)
        {
            DialogueUI.SetActive(true);
            //вытаскиваем из реплики по ключам имя персонажа и реплику
            NameUI.text = JsonParser.currentLine.Character;
            LineUI.text = JsonParser.currentLine.Line;
        }
        
    }
}
