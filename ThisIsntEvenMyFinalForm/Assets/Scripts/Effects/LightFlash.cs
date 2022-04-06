using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class LightFlash : MonoBehaviour
{
    [SerializeField] private float _intensity = 100;
    [SerializeField] private float _duration = 1;
    [SerializeField] private Light2D _light;

    private float _effectTotalTime = 0f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(IncreaseLightIntensityForDuration());
    }

    private IEnumerator IncreaseLightIntensityForDuration()
    {
        while (_effectTotalTime < _duration)
        {
            _light.intensity = _intensity * Mathf.Lerp(1, 0, _effectTotalTime / _duration);
            _effectTotalTime += Time.deltaTime;
            yield return null;
        }

        _light.intensity = 0f;
        _effectTotalTime = 0f;
    }
}
