using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Sentence
{
    [Range(1, 4)]
    public int PoseNumber = 1;

    public FacialExpressionType Face = FacialExpressionType.Smug;

    [TextArea]
    public string SentenceText;
}
