using UnityEngine;
using FMODUnity;

public class PlayOneShot : MonoBehaviour
{
    [SerializeField] private EventReference _event;

    public void Play()
    {
        RuntimeManager.PlayOneShot(_event, transform.position);
    }
}
