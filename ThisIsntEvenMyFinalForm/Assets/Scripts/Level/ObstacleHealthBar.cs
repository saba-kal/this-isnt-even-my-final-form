using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleHealthBar : HealthBar
{
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _endColor;
    [SerializeField] private List<SpriteRenderer> _spriteRenderers;

    protected override void VirtualStart()
    {
        foreach (var renderer in _spriteRenderers)
        {
            renderer.color = _startColor;
        }
    }

    public override void SetHealth(int currentHealth, int maxHealth)
    {
        foreach (var renderer in _spriteRenderers)
        {
            if (renderer != null)
            {
                renderer.color = Color.Lerp(_endColor, _startColor, (float)currentHealth / maxHealth);
            }
        }
    }
}
