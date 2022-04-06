using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSkipToggle : MonoBehaviour
{
    public static DialogueSkipToggle Instance { get; private set; }

    private Toggle _toggle;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _toggle = GetComponent<Toggle>();
    }

    public bool IsOn()
    {
        return _toggle.isOn;
    }
}
