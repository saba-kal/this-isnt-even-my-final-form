using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private string _playerName;
    [SerializeField] private string _enemyName;
    [SerializeField] private DialogueView _dialogueView;
    [SerializeField] private Conversation _initialConversation;
    [SerializeField] private List<Conversation> _playerWinConversationList;
    [SerializeField] private List<Conversation> _enemyWinConversationList;
    [SerializeField] private Conversation _playerWinEndGameConversation;
    [SerializeField] private Conversation _enemyWinEndGameConversation;
    [SerializeField] private CharacterPoseManager _playerPoseManager;
    [SerializeField] private CharacterPoseManager _enemyPoseManager;

    private Queue<Conversation> _playerWinConversationQueue = new Queue<Conversation>();
    private Queue<Conversation> _enemyWinConversationQueue = new Queue<Conversation>();
    private Queue<Sentence> _sentenceQueue = new Queue<Sentence>();
    private Action _onConversationComplete;
    private Action _onCharacterPowerUpStart;
    private Action _onCharacterPowerUpEnd;
    private bool _powerUpStarted = false;
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

        _playerWinConversationQueue = new Queue<Conversation>(_playerWinConversationList);
        _enemyWinConversationQueue = new Queue<Conversation>(_enemyWinConversationList);
    }

    private void Update()
    {
        if (_conversationInProgress &&
            Input.GetKeyDown(KeyCode.F))
        {
            ShowNextSentence();
        }
    }
    public void StartInitialConversation(Action onComplete)
    {
        if (_conversationInProgress)
        {
            return;
        }

        if (DialogueSkipToggle.IsOn)
        {
            onComplete?.Invoke();
            return;
        }

        _conversationInProgress = true;
        _onConversationComplete = onComplete;
        _sentenceQueue = new Queue<Sentence>();
        StartConversation(_initialConversation);
    }

    public void StartConversation(
        bool playerIsWinner,
        Action onPowerUpStart,
        Action onPowerUpEnd,
        Action onComplete)
    {
        var conversationQueue = playerIsWinner ?
            _playerWinConversationQueue : _enemyWinConversationQueue;

        if (conversationQueue == null ||
            conversationQueue.Count == 0 ||
            DialogueSkipToggle.IsOn)
        {
            onPowerUpEnd?.Invoke();
            onComplete?.Invoke();
            return;
        }

        _conversationInProgress = true;
        _onCharacterPowerUpStart = onPowerUpStart;
        _onCharacterPowerUpEnd = onPowerUpEnd;
        _onConversationComplete = onComplete;
        _sentenceQueue = new Queue<Sentence>();

        var conversation = conversationQueue.Dequeue();
        StartConversation(conversation);
    }

    public void StartEndGameConversation(bool playerIsWinner, Action onComplete)
    {
        var conversation = playerIsWinner ?
            _playerWinEndGameConversation : _enemyWinEndGameConversation;

        if (_conversationInProgress)
        {
            return;
        }

        if (DialogueSkipToggle.IsOn)
        {
            onComplete?.Invoke();
            return;
        }

        _conversationInProgress = true;
        _onConversationComplete = onComplete;
        _sentenceQueue = new Queue<Sentence>();
        StartConversation(conversation);
    }

    public bool ConversationInProgress()
    {
        return _conversationInProgress;
    }

    private void StartConversation(Conversation conversation)
    {
        if (_sentenceQueue.Count > 0)
        {
            //Conversation is in progress.
            return;
        }

        _dialogueView.ShowDialogueBox();
        _powerUpStarted = false;

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
        if (_currentSentence.IsPowerUp)
        {
            _powerUpStarted = true;
            _onCharacterPowerUpStart?.Invoke();
        }
        else if (_powerUpStarted)
        {
            _powerUpStarted = false;
            _onCharacterPowerUpEnd?.Invoke();
        }

        UpdateCharacterPose();
        StartCoroutine(_dialogueView.AnimateSentence(GetNameParam(_currentSentence.TalkingCharacter), BuildSentence()));
    }

    private void UpdateCharacterPose()
    {
        _enemyPoseManager.gameObject.SetActive(false);
        _playerPoseManager.gameObject.SetActive(false);

        var poseView = _playerPoseManager;
        if (_currentSentence.TalkingCharacter == CharacterType.Enemy)
        {
            poseView = _enemyPoseManager;
        }

        poseView.gameObject.SetActive(true);
        poseView.UpdatePose(_currentSentence, GetPowerLevel(_currentSentence.TalkingCharacter));
    }

    private string BuildSentence()
    {
        var sentence = _currentSentence.SentenceText;
        sentence = sentence
            .Replace("{enemy_name}", GetNameParam(CharacterType.Enemy))
            .Replace("{player_name}", GetNameParam(CharacterType.Player))
            .Replace("{enemy_sides}", GetSideCountParam(CharacterType.Enemy))
            .Replace("{player_sides}", GetSideCountParam(CharacterType.Player))
            .Replace("{enemy_shape}", GetShapeParam(CharacterType.Enemy))
            .Replace("{player_shape}", GetShapeParam(CharacterType.Player))
            .Replace("{enemy_sides_stutter}", GetSideCountStutterParam(CharacterType.Enemy))
            .Replace("{player_sides_stutter}", GetSideCountStutterParam(CharacterType.Player))
            .Replace("{enemy_shape_stutter}", GetShapeStutterParam(CharacterType.Enemy))
            .Replace("{player_shape_stutter}", GetShapeStutterParam(CharacterType.Player));

        return sentence;
    }

    private string GetNameParam(CharacterType characterType)
    {
        switch (characterType)
        {
            case CharacterType.Enemy:
                return _enemyName;
            case CharacterType.Player:
                return _playerName;
        }

        return "No Name";
    }

    private string GetSideCountParam(CharacterType characterType)
    {
        switch (GetPowerLevel(characterType))
        {
            case 1:
                return "three";
            case 2:
                return "four";
            case 3:
                return "five";
            case 4:
                return "six";
        }
        return "zero";
    }

    private string GetShapeParam(CharacterType characterType)
    {
        switch (GetPowerLevel(characterType))
        {
            case 1:
                return "triangle";
            case 2:
                return "square";
            case 3:
                return "pentagon";
            case 4:
                return "hexagon";
        }

        return "no shape";
    }

    private string GetSideCountStutterParam(CharacterType characterType)
    {
        switch (GetPowerLevel(characterType))
        {
            case 1:
                return "th-th-three";
            case 2:
                return "f-f-four";
            case 3:
                return "f-f-five";
            case 4:
                return "s-s-six";
        }
        return "z-z-zero";
    }

    private string GetShapeStutterParam(CharacterType characterType)
    {
        switch (GetPowerLevel(characterType))
        {
            case 1:
                return "t-t-triangle";
            case 2:
                return "s-s-square";
            case 3:
                return "p-p-pentagon";
            case 4:
                return "h-h-hexagon";
        }

        return "n-n-no shape";
    }

    private int GetPowerLevel(CharacterType characterType)
    {
        return characterType == CharacterType.Enemy ?
            AIController.Instance.GetPowerLevel() :
            PlayerController.Instance.GetPowerLevel();
    }
}
