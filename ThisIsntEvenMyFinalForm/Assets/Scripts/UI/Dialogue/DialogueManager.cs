using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private DialogueView _dialogueView;
    [SerializeField] private List<Conversation> _conversationList;
    [SerializeField] private Conversation _endGameConversation;
    [SerializeField] private CharacterPoseManager _playerPoseManager;
    [SerializeField] private CharacterPoseManager _enemyPoseManager;

    private Queue<Conversation> _conversationQueue = new Queue<Conversation>();
    private Queue<Sentence> _sentenceQueue = new Queue<Sentence>();
    private Action _onConversationComplete;
    private bool _conversationInProgress = false;
    private Sentence _currentSentence;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _conversationQueue = new Queue<Conversation>(_conversationList);
    }

    private void Update()
    {
        if (_conversationInProgress &&
            Input.GetKeyDown(KeyCode.F))
        {
            ShowNextSentence();
        }
    }

    public void StartConversation(Action onComplete)
    {
        if (_conversationQueue == null || _conversationQueue.Count == 0)
        {
            Debug.Log("No more conversations");
            onComplete?.Invoke();
            return;
        }

        _conversationInProgress = true;
        _onConversationComplete = onComplete;
        _sentenceQueue = new Queue<Sentence>();

        var conversation = _conversationQueue.Dequeue();
        StartConversation(conversation);
    }

    public void StartEndGameConversation(Action onComplete)
    {
        if (_conversationInProgress)
        {
            return;
        }

        _conversationInProgress = true;
        _onConversationComplete = onComplete;
        _sentenceQueue = new Queue<Sentence>();
        StartConversation(_endGameConversation);
    }

    private void StartConversation(Conversation conversation)
    {
        if (_sentenceQueue.Count > 0)
        {
            //Conversation is in progress.
            return;
        }

        _dialogueView.ShowDialogueBox();

        foreach (var dialog in conversation.DialogeList)
        {
            foreach (var sentence in dialog.Sentences)
            {
                sentence.TalkingCharacter = dialog.TalkingCharacter;
                _sentenceQueue.Enqueue(sentence);
            }
        }

        ShowNextSentence();
    }

    private void ShowNextSentence()
    {
        if (DialogueView.SentenceAnimationInProgress)
        {
            return;
        }

        if (_sentenceQueue == null || _sentenceQueue.Count == 0)
        {
            _onConversationComplete?.Invoke();
            _conversationInProgress = false;
            _dialogueView.HideDialogue();
            _enemyPoseManager.HideCharacter();
            _playerPoseManager.HideCharacter();
            return;
        }

        _currentSentence = _sentenceQueue.Dequeue();
        UpdateCharacterPose();
        StartCoroutine(_dialogueView.AnimateSentence(_currentSentence.SentenceText));
    }

    private void UpdateCharacterPose()
    {
        _enemyPoseManager.gameObject.SetActive(false);
        _playerPoseManager.gameObject.SetActive(false);

        var poseView = _playerPoseManager;
        var powerLevel = PlayerController.Instance.GetPowerLevel();
        if (_currentSentence.TalkingCharacter == CharacterType.Enemy)
        {
            poseView = _enemyPoseManager;
            powerLevel = AIController.Instance.GetPowerLevel();
        }
        poseView.gameObject.SetActive(true);

        poseView.UpdatePose(_currentSentence, powerLevel);
    }
}
