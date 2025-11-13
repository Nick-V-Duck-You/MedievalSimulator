using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JsonParser : MonoBehaviour
{
    public DialoguesContainer _dialogueDatabase;
    public DialogueLinesData currentLine;

    public int currentLineID;

    void Start()
    {
        currentLineID = 0;

        // Загружаем из папки ресурсес жсон, нужно в старте но наверно для экономии ресурсов можно будет захуярить не в старт а в момент взаимодействия в диалоговой системой
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Dialogues");

        //записываем всю эту хуету из жсон строки в объект класса DialoguesContainer (контейнер по ключу верхнего уровня, короче там список диалогов), нужно в старте но наверно для экономии ресурсов можно будет захуярить не в старт а в момент взаимодействия в диалоговой системой
        _dialogueDatabase = JsonUtility.FromJson<DialoguesContainer>(jsonTextAsset.text);

    }

    public void LineFiller()
    {
        //создаем объект класса DialogueLinesData (реплика короче) и запихиваем в него информацию из контейнера Диалог 1 реплики с индексом 0
        currentLine = _dialogueDatabase.Dialogue1 [currentLineID];
    }

}
