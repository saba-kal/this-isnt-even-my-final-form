using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterPowerUpEffect : MonoBehaviour
{
    [SerializeField] private float _imageGlowStrength = 2.1f;
    [SerializeField] private float _effectMaxScrollSpeed = 2.1f;
    [SerializeField] private float _effectScrollAcceleration = 1f;
    [SerializeField] private float _effectGlowAcceleration = 10f;
    [SerializeField] private float _powerDownDeceleration = 0.1f;
    [SerializeField] private float _powerDownAcceleration = 10f;
    [SerializeField] private float _maxPowerDownGlow = 10f;
    [SerializeField] private Image _characterImage;
    [SerializeField] private GameObject _poweredUpEffect;

    private Material _characterImageMaterial;
    private PowerLevelManager _powerLevelManager;
    private bool _showPoweringUpEffect = false;
    private bool _showPoweredUpEffect = false;
    private bool _isPoweringDown = false;
    private float _effectScrollSpeed = 0f;
    private float _effectGlowSpeed = 0f;
    private float _effectScroll = 0f;

    // Use this for initialization
    void Awake()
    {
        _characterImageMaterial = _characterImage.material;
        _characterImageMaterial.SetFloat("_GradientStrength", 0);
        _characterImageMaterial.SetFloat("_GradientOffset", 0);
        _poweredUpEffect.SetActive(false);
        _powerLevelManager = GetComponent<PowerLevelManager>();
    }

    void Update()
    {
        if (_showPoweringUpEffect)
        {
            ShowPoweringUpEffect();
        }

        if (_showPoweredUpEffect)
        {
            ShowPoweredUpEffect();
        }
    }

    public void EnablePoweringUpEffect()
    {
        _effectScrollSpeed = 0f;
        _effectGlowSpeed = 0f;
        _effectScroll = 0f;
        _showPoweringUpEffect = true;
    }

    public void EnablePoweredUpEffect()
    {
        _showPoweredUpEffect = true;
        _poweredUpEffect.transform.localScale = Vector3.one * _powerLevelManager.GetPowerLevel() * 0.5f;
        _poweredUpEffect.SetActive(true);
    }

    private void ShowPoweringUpEffect()
    {
        _effectScrollSpeed += (_effectScrollAcceleration * Time.deltaTime);
        _effectScroll -= Mathf.Clamp(_effectScrollSpeed, 0, _effectMaxScrollSpeed);
        _characterImageMaterial.SetFloat("_GradientOffset", _effectScroll);

        _effectGlowSpeed += (_effectGlowAcceleration * Time.deltaTime);
        _characterImageMaterial.SetFloat("_GradientStrength", Mathf.Clamp(_effectGlowSpeed, 0, _imageGlowStrength));
    }

    private void ShowPoweredUpEffect()
    {
        var imageGlowStrength = _characterImageMaterial.GetFloat("_GradientStrength");
        if (_isPoweringDown)
        {
            imageGlowStrength -= _powerDownDeceleration * Time.deltaTime;
            if (imageGlowStrength <= 0)
            {
                imageGlowStrength = 0;
                _isPoweringDown = false;
                _showPoweredUpEffect = false;
                _poweredUpEffect.SetActive(false);
            }
        }
        else
        {
            imageGlowStrength += _powerDownAcceleration * Time.deltaTime;
            if (imageGlowStrength >= _maxPowerDownGlow)
            {
                _isPoweringDown = true;
            }
        }

        _characterImageMaterial.SetFloat("_GradientStrength", imageGlowStrength);
        _showPoweringUpEffect = false;
    }
}
