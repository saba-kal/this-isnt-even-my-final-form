using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueView : MonoBehaviour
{
    public static bool SentenceAnimationInProgress { get; private set; } = false;

    [SerializeField] private float _timeBetweenCharacters = 0.1f;
    [SerializeField] private TextMeshProUGUI _sentenxeText;
    [SerializeField] private GameObject _dialogBox;
    [SerializeField] private GameObject _buttonIndicator;

    private void Start()
    {
        _buttonIndicator.SetActive(false);
        HideDialogue();
    }

    public void ShowDialogueBox()
    {
        _dialogBox.SetActive(true);
    }

    public void HideDialogue()
    {
        _dialogBox.SetActive(false);
    }

    public IEnumerator AnimateSentence(string sentence)
    {
        SentenceAnimationInProgress = true;
        _dialogBox.SetActive(true);
        _buttonIndicator.SetActive(false);
        _sentenxeText.text = "";
        foreach (var character in sentence.ToCharArray())
        {
            _sentenxeText.text += character;
            yield return new WaitForSeconds(_timeBetweenCharacters);
        }
        _buttonIndicator.SetActive(true);
        SentenceAnimationInProgress = false;
    }
}
