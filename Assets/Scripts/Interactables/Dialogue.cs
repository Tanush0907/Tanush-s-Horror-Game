using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Dialogue : MonoBehaviour, ITalkable
{
    [TextArea(3, 10)]
    [SerializeField] private string dialogue;
    public float rotationSpeed;
    public float typingSpeed = 0.05f;
    private TextMeshProUGUI dialogueBox;
    private void Start()
    {
        dialogueBox = Player.Instance.dialogueBox;

    }
    public void Talk()
    {
        StartTyping(dialogue);
    }

    public void StartTyping(string message)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(message));
    }

    private IEnumerator TypeText(string message)
    {
        dialogueBox.text = "";
        foreach (char c in message)
        {
            dialogueBox.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
