using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class ChromaticAberrationEffect : MonoBehaviour
{
    public static ChromaticAberrationEffect Instance { get; private set; }

    [SerializeField] private float _duration = 0.3f;

    private ChromaticAberration _chromaticAberration;

    private float _effectTotalTime = 0f;

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

        var profile = GetComponent<Volume>().profile;
        profile.TryGet(out _chromaticAberration);
    }

    public void Trigger()
    {
        if (_chromaticAberration == null)
        {
            Debug.LogError("Unable to trigger chromatic aberration because the effect was not found in the post-processing volume.");
            return;
        }

        StopCoroutine("IncreaseChromaticAberrationForDuration");
        StartCoroutine("IncreaseChromaticAberrationForDuration");
    }

    private IEnumerator IncreaseChromaticAberrationForDuration()
    {
        while (_effectTotalTime < _duration)
        {
            _chromaticAberration.intensity.Override(Mathf.Lerp(1, 0, _effectTotalTime / _duration));
            _effectTotalTime += Time.deltaTime;
            yield return null;
        }

        _chromaticAberration.intensity.Override(0);
        _effectTotalTime = 0f;
    }
}
