using System.Collections.Generic;
using UnityEngine;

public class JsonParser : MonoBehaviour
{
    public DialoguesContainer _dialogueDatabase;
    public DialogueLinesData currentLine;

    public int currentLineID;

    void Start()
    {
        currentLineID = 0;

        // «агружаем из папки ресурсес жсон, нужно в старте но наверно дл€ экономии ресурсов можно будет заху€рить не в старт а в момент взаимодействи€ в диалоговой системой
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Dialogues");

        //записываем всю эту хуету из жсон строки в объект класса DialoguesContainer (контейнер по ключу верхнего уровн€, короче там список диалогов), нужно в старте но наверно дл€ экономии ресурсов можно будет заху€рить не в старт а в момент взаимодействи€ в диалоговой системой
        _dialogueDatabase = JsonUtility.FromJson<DialoguesContainer>(jsonTextAsset.text);

    }

    public void LineFiller(string dialogueName)
    {
        // »спользуем переданное им€ диалога (dialogueName) вместо внутреннего пол€
        List<DialogueLinesData> targetList = (List<DialogueLinesData>)typeof(DialoguesContainer)
            .GetField(dialogueName)
            .GetValue(_dialogueDatabase);

        //создаем объект класса DialogueLinesData (реплика короче) и запихиваем в него информацию из контейнера ƒиалогов с именем targetList с индексом currentLineID
        currentLine = targetList[currentLineID];
    }

}
