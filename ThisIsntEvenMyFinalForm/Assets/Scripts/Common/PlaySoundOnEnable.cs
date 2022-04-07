using UnityEngine;
using System.Collections;

public class PlaySoundOnEnable : MonoBehaviour
{
    [SerializeField] private string _soundName;

    private void OnEnable()
    {
        SoundManager.Instance?.Play(_soundName);
    }
}
