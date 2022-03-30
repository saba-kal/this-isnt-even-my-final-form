using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public static bool DialogInProgress { get; private set; } = false;

    private Queue<Sentence> _sentenceQueue = new Queue<Sentence>();

    private Action _onConversationComplete;
    private bool _sentenceAnimationInProgress = false;
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
    }

    public void StartConversation(Conversation conversation, Action onComplete)
    {
        DialogInProgress = true;
        _onConversationComplete = onComplete;
        _sentenceQueue = new Queue<Sentence>();
        foreach (var dialog in conversation.DialogeList)
        {
            foreach (var sentence in dialog.Sentences)
            {
                _sentenceQueue.Enqueue(sentence);
            }
        }

        ShowNextSentence();
    }

    private void ShowNextSentence()
    {
        if (_sentenceAnimationInProgress)
        {
            Debug.Log("Stopping sentence animation.");
            Debug.Log(_currentSentence.SentenceText);
            return;
        }

        _currentSentence = _sentenceQueue.Dequeue();
        if (_currentSentence == null)
        {
            DialogInProgress = false;
            _onConversationComplete?.Invoke();
            Debug.Log("Conversation complete.");
            return;
        }

        StartCoroutine(StartSentenceAnimation());
    }

    private IEnumerator StartSentenceAnimation()
    {
        _sentenceAnimationInProgress = true;
        Debug.Log("Starting sentence animation.");
        yield return new WaitForSeconds(2f);

        _sentenceAnimationInProgress = false;
        Debug.Log("Completed sentence animation.");
        Debug.Log(_currentSentence.SentenceText);
        ShowNextSentence();
    }
}
