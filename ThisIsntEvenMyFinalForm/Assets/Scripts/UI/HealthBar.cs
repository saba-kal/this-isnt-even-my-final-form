using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void Start()
    {
        VirtualStart();
    }

    protected virtual void VirtualStart()
    {
        _slider.minValue = 0;
        _slider.maxValue = 1;
    }

    public virtual void SetHealth(int currentHealth, int maxHealth)
    {
        _slider.value = (float)currentHealth / maxHealth;
    }
}
