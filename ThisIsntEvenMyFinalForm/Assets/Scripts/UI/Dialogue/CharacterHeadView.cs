using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterHeadView : MonoBehaviour
{
    public int PowerLevel = 1;

    private Image _headImage;

    private void Awake()
    {
        _headImage = GetComponent<Image>();
    }

    public void SetColor(Color color)
    {
        _headImage.color = color;
    }
}
