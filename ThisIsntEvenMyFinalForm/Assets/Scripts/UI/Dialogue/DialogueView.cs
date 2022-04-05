using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueView : MonoBehaviour
{
    public static bool SentenceAnimationInProgress { get; private set; } = false;

    [SerializeField] private float _timeBetweenCharacters = 0.1f;
    [SerializeField] private TextMeshProUGUI _sentenceText;
    [SerializeField] private TextMeshProUGUI _nameText;
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

    public IEnumerator AnimateSentence(string characterName, string sentence)
    {
        _nameText.text = characterName;
        _sentenceText.text = "";

        SentenceAnimationInProgress = true;
        _dialogBox.SetActive(true);
        _buttonIndicator.SetActive(false);
        foreach (var character in sentence.ToCharArray())
        {
            _sentenceText.text += character;
            yield return new WaitForSeconds(_timeBetweenCharacters);
        }
        _buttonIndicator.SetActive(true);
        SentenceAnimationInProgress = false;
    }
}
