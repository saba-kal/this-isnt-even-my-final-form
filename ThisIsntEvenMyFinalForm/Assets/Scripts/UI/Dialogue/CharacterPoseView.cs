using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CharacterPoseView : MonoBehaviour
{
    public int PoseNumber;

    [SerializeField] private Image _poseImage;

    private List<CharacterFaceView> _faces;
    private List<CharacterHeadView> _heads;

    void Awake()
    {
        _faces = GetComponentsInChildren<CharacterFaceView>().ToList();
        _heads = GetComponentsInChildren<CharacterHeadView>().ToList();
    }

    public void SetColor(Color color)
    {
        _poseImage.color = color;
        foreach (var head in _heads)
        {
            head.SetColor(color);
        }
    }

    public void ShowCharacter(
        int powerLevel,
        Sentence sentence)
    {
        HideHeads();
        HideFaces();
        ShowActiveHead(powerLevel);
        ShowActiveFace(sentence);
    }

    private void ShowActiveHead(
        int powerLevel)
    {
        var headFound = false;
        foreach (var head in _heads)
        {
            if (head.PowerLevel == powerLevel)
            {
                head.gameObject.SetActive(true);
                headFound = true;
                break;
            }
        }

        if (!headFound)
        {
            Debug.LogError($"Unable to find head for power level {powerLevel}");
        }
    }

    private void ShowActiveFace(
        Sentence sentence)
    {
        var faceFound = false;
        foreach (var face in _faces)
        {
            if (face.Expression == sentence.Face)
            {
                face.gameObject.SetActive(true);
                faceFound = true;
                break;
            }
        }

        if (!faceFound)
        {
            Debug.LogError($"Unable to find face for expression {sentence.Face}");
        }
    }

    private void HideHeads()
    {
        foreach (var head in _heads)
        {
            head.gameObject.SetActive(false);
        }
    }

    private void HideFaces()
    {
        foreach (var face in _faces)
        {
            face.gameObject.SetActive(false);
        }
    }

}
