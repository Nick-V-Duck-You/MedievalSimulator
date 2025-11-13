using UnityEngine;
using UnityEngine.UI;

public class DialoguePlayer : MonoBehaviour
{
    public GameObject DialogueUI;

    [SerializeField] private Text LineUI;
    [SerializeField] private Text NameUI;

    public GameObject triggerObject; // Assign this in the Inspector
    public GameObject Player; // Assign this in the Inspector

    public float distance;

    public JsonParser JsonParser;
    void Start()
    {
        DialogueUI.SetActive(false);
    }

    void Update()
    {
        distance = Vector3.Distance(triggerObject.transform.position, Player.transform.position);
        if (distance < 5f){
            PlayDialogue();
            JsonParser.LineFiller(); // ÍÀÄÎ ÈÑÏÐÀÂÈÒÜ ÌÍÅ ÍÅ ÍÐÀÂÈÒÑß ×ÒÎ ÎÍÎ ÂÛÇÛÂÀÅÒÑß 60 ÐÀÇ Â ÑÅÊÓÍÄÓ
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Keypad Enter key pressed!");
                JsonParser.currentLineID += 1;
            }
        }
    }

    public void PlayDialogue()
    {
        DialogueUI.SetActive(true);
        //âûòàñêèâàåì èç ðåïëèêè ïî êëþ÷àì èìÿ ïåðñîíàæà è ðåïëèêó
        NameUI.text = JsonParser.currentLine.Character;
        LineUI.text = JsonParser.currentLine.Line;
    }
}
