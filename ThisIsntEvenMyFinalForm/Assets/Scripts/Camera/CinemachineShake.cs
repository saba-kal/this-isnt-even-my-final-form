using UnityEngine;
using System.Collections;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    [SerializeField] private float _maxAmplitudeGain = 5f;
    [SerializeField] private float _shakeDuration = 0.3f;

    private CinemachineVirtualCamera _virtualCamera;
    private float _shakeTotalTime = 0;

    // Use this for initialization
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake()
    {
        StartCoroutine(ShakeCameraOverTime());
    }

    private IEnumerator ShakeCameraOverTime()
    {
        var cameraNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        while (_shakeTotalTime < _shakeDuration)
        {
            cameraNoise.m_AmplitudeGain = Mathf.Lerp(_maxAmplitudeGain, 0, _shakeTotalTime / _shakeDuration);
            _shakeTotalTime += Time.deltaTime;
            yield return null;
        }

        _shakeTotalTime = 0f;
    }
}
