using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using System;

public class TutorialStage : MonoBehaviour
{
    [SerializeField] private float _indicatorLightMaxIntensity = 100f;
    [SerializeField] private float _glowSpeed = 100f;
    [SerializeField] private float _wallMoveSpeed = 10f;
    [SerializeField] private float _tutorialRemoveDuration = 1.5f;
    [SerializeField] private GameObject _wall1;
    [SerializeField] private GameObject _wall2;
    [SerializeField] private GameObject _wall3;
    [SerializeField] private GameObject _wall4;
    [SerializeField] private Light2D _indicatorLight;
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _interactTooltip;
    [SerializeField] private GameObject _moveTooltip;

    private float _initalLightIntensity;

    // Use this for initialization
    void Start()
    {
        _initalLightIntensity = _indicatorLight.intensity;
        _interactTooltip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_interactTooltip != null && _indicatorLight != null)
        {
            ShowGlowingEffectForInteractibleObject();
        }
    }

    public void RemoveTutorialTooltips()
    {
        Destroy(_indicatorLight.gameObject);
        Destroy(_interactTooltip);
        Destroy(_moveTooltip);
    }

    public void RemoveTutorialStage(Action onComplete)
    {
        StartCoroutine(MoveWallTowardsDirection(_wall1, new Vector2(-1f, 0), onComplete));
        StartCoroutine(MoveWallTowardsDirection(_wall2, new Vector2(1f, 0)));
        StartCoroutine(MoveWallTowardsDirection(_wall3, new Vector2(0, 1f)));
        StartCoroutine(MoveWallTowardsDirection(_wall4, new Vector2(0, -1f)));
    }

    private void ShowGlowingEffectForInteractibleObject()
    {
        if (InteractableObject.GetNearestInteractable(_player.transform.position) != null)
        {
            _indicatorLight.intensity = _initalLightIntensity + ((1 + Mathf.Sin(Time.realtimeSinceStartup * _glowSpeed) / 2) * _indicatorLightMaxIntensity);
            _interactTooltip.SetActive(true);
        }
        else
        {
            _indicatorLight.intensity = _initalLightIntensity;
            _interactTooltip.SetActive(false);
        }
    }

    private IEnumerator MoveWallTowardsDirection(
        GameObject wall,
        Vector2 direction,
        Action onComplete = null)
    {
        var timeSinceAbilityStart = 0f;
        while (timeSinceAbilityStart < _tutorialRemoveDuration)
        {
            wall.transform.Translate(direction * Time.deltaTime * _wallMoveSpeed);
            timeSinceAbilityStart += Time.deltaTime;
            yield return null;
        }
        Destroy(wall);
        onComplete?.Invoke();
    }
}
