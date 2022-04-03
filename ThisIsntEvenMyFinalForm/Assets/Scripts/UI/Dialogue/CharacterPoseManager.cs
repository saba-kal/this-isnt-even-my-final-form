using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CharacterPoseManager : MonoBehaviour
{
    [SerializeField] private GameObject _posesContainer;
    [SerializeField] private Color _poseColor;

    private List<CharacterPoseView> _poses;

    // Use this for initialization
    void Start()
    {
        _poses = GetComponentsInChildren<CharacterPoseView>().ToList();
        SetColor();
        HideCharacter();
    }

    public void UpdatePose(
        Sentence sentence,
        int powerLevel)
    {
        if (powerLevel < 1)
        {
            powerLevel = 1;
        }

        HideCharacter();
        ShowActiveCharacter(powerLevel, sentence);
    }

    public void HideCharacter()
    {
        foreach (var pose in _poses)
        {
            pose.gameObject.SetActive(false);
        }
    }

    private void SetColor()
    {
        foreach (var pose in _poses)
        {
            pose.SetColor(_poseColor);
        }
    }

    private void ShowActiveCharacter(
        int powerLevel,
        Sentence sentence)
    {
        foreach (var pose in _poses)
        {
            if (pose.PoseNumber == sentence.PoseNumber)
            {
                pose.gameObject.SetActive(true);
                pose.ShowCharacter(powerLevel, sentence);
                break;
            }
        }
    }
}
