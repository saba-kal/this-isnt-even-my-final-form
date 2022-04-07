using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _damageFill;
    [SerializeField] private float _damageFillSpeed;

    private void Start()
    {
        VirtualStart();
    }

    protected virtual void VirtualStart()
    {
        _slider.minValue = 0;
        _slider.maxValue = 1;

        if (_damageFill != null)
        {
            _damageFill.fillAmount = 1f;
        }
    }

    public virtual void SetHealth(int currentHealth, int maxHealth)
    {
        _slider.value = (float)currentHealth / maxHealth;

        if (_damageFill != null)
        {
            StopCoroutine("SlowlyMoveDamageFill");
            StartCoroutine(SlowlyMoveDamageFill(_slider.value));
        }
    }

    private IEnumerator SlowlyMoveDamageFill(float targetFillAmount)
    {
        while (_damageFill.fillAmount > targetFillAmount)
        {
            _damageFill.fillAmount -= _damageFillSpeed * Time.deltaTime;
            yield return null;
        }
        _damageFill.fillAmount = targetFillAmount;

        if (targetFillAmount <= 0.001f)
        {
            _damageFill.gameObject.SetActive(false);
        }
    }
}
