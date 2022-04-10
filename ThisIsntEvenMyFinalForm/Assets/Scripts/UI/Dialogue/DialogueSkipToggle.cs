using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSkipToggle : MonoBehaviour
{
    public static bool IsOn { get; private set; } = false;

    void Awake()
    {
        var toggle = GetComponent<Toggle>();
        toggle.isOn = IsOn;
        toggle.onValueChanged.AddListener(isOn =>
        {
            IsOn = isOn;
        });
    }
}
