using UnityEngine;
using System.Collections;
using FMODUnity;

public class PlaySoundOnEnable : MonoBehaviour
{
    [SerializeField] private EventReference _soundEvent;

    private void OnEnable()
    {
        RuntimeManager.PlayOneShot(_soundEvent, transform.position);
    }
}
