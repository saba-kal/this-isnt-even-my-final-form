using UnityEngine;
using System.Collections;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }
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

    public void ShowInteractionTooltip()
    {
        Debug.Log("Press F to interact with object.");
    }
}
