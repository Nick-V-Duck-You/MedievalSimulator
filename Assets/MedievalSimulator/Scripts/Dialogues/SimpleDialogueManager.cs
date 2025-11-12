using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections; // Нужен для корутин, хотя в этом коде не используется, но полезен

public class SimpleDialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    // Перетащите сюда ваши UI элементы Text из инспектора
    public Text CharacterNameText;
    public Text DialogueLineText;

    [Header("Audio Settings")]
    // Перетащите сюда компонент AudioSource из инспектора
    public AudioSource audioSource;

    [Header("JSON Data")]
    // Перетащите ваш файл 'dialogues.json' сюда из папки Resources
    public TextAsset dialoguesJsonFile;

    // --- Классы C# для парсинга JSON ---
    [Serializable]
    public class DialogueLineData
    {
        public int id;
        public string Character;
        public string Line;
        public string audioClip; // Путь к аудиофайлу в JSON
    }

    [Serializable]
    public class Dialogue1Container
    {
        public DialogueLineData[] Dialogue1;
    }
    // ------------------------------------

    private List<DialogueLineData> currentDialogueLines;
    private int currentLineIndex = 0;
    private bool dialogueIsActive = false;

    void Start()
    {
        if (dialoguesJsonFile != null && audioSource != null)
        {
            ParseDialogue1();
            StartDialogue();
        }
        else
        {
            Debug.LogError("Не все зависимости назначены в инспекторе SimpleDialogueManager!");
        }
    }

    void Update()
    {
        // Проверяем нажатие кнопки Enter (или Return) для перехода к следующей реплике
        if (dialogueIsActive && Input.GetKeyDown(KeyCode.Return))
        {
            DisplayNextLine();
        }
    }

    private void ParseDialogue1()
    {
        Dialogue1Container container = JsonUtility.FromJson<Dialogue1Container>(dialoguesJsonFile.text);
        currentDialogueLines = new List<DialogueLineData>(container.Dialogue1);
    }

    public void StartDialogue()
    {
        if (currentDialogueLines.Count > 0)
        {
            dialogueIsActive = true;
            currentLineIndex = 0;
            CharacterNameText.gameObject.SetActive(true);
            DialogueLineText.gameObject.SetActive(true);
            DisplayCurrentLine();
        }
    }

    public void DisplayNextLine()
    {
        if (currentLineIndex >= currentDialogueLines.Count - 1)
        {
            EndDialogue();
            return;
        }

        currentLineIndex++;
        DisplayCurrentLine();
    }

    private void DisplayCurrentLine()
    {
        DialogueLineData lineData = currentDialogueLines[currentLineIndex];

        CharacterNameText.text = lineData.Character;
        DialogueLineText.text = lineData.Line;

        PlayAudioClip(lineData.audioClip);
    }

    private void PlayAudioClip(string fullPath)
    {
        // В JSON путь: "MedievalSimulator/Resources/Audio/Dialogue1_0.wav" (теперь с 'c')
        // Unity Resources.Load требует путь относительно папки Assets/Resources и БЕЗ расширения.

        fullPath = fullPath.Replace('\\', '/');
        string resourcesPath = "";

        // Находим индекс папки 'Resources/'
        int startIndex = fullPath.IndexOf("Resources/");

        if (startIndex != -1)
        {
            // Обрезаем все до начала папки Resources и саму папку Resources/
            // В итоге должно получиться: "Audio/Dialogue1_0.wav"
            resourcesPath = fullPath.Substring(startIndex + "Resources/".Length);

            // Удаляем расширение ".wav" (если оно есть)
            if (resourcesPath.EndsWith(".wav"))
            {
                resourcesPath = resourcesPath.Substring(0, resourcesPath.Length - 4);
            }
            // resourcesPath теперь: "Audio/Dialogue1_0"
        }
        else
        {
            Debug.LogError($"Ошибка: Путь '{fullPath}' не содержит 'Resources/'. Проверьте название папки.");
            return;
        }

        // Загружаем клип из папки Resources
        AudioClip clip = Resources.Load<AudioClip>(resourcesPath);

        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Аудиофайл не найден по пути Resources: {resourcesPath}");
        }
    }

    private void EndDialogue()
    {
        Debug.Log("Диалог 1 завершен.");
        dialogueIsActive = false;
        CharacterNameText.text = "";
        DialogueLineText.text = "";
        audioSource.Stop();
    }
}
