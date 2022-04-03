using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Dialogue
{
    public CharacterType TalkingCharacter = CharacterType.Player;

    public List<Sentence> Sentences;
}
