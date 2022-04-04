using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterHealth : MonoBehaviour
{
    public delegate void HealthLostEvent(CharacterHealth characterHealth);
    public static HealthLostEvent OnHealthLost;

    public delegate void DeathEvent(CharacterHealth characterHealth);
    public static DeathEvent OnDeath;

    [SerializeField] private List<Health> _healths;

    private int _currentHealthIndex = 0;

    // Use this for initialization
    void Start()
    {
        foreach (var health in _healths)
        {
            health.gameObject.SetActive(false);
            health.DestroyOnDeath = false;
        }

        _healths[_currentHealthIndex].gameObject.SetActive(true);
        _healths[_currentHealthIndex].SetOnDeath(OnCurrentHealthDeath);
    }

    public void SetImmune(bool immune)
    {
        foreach (var health in _healths)
        {
            health.SetImmune(immune);
        }
    }

    private void OnCurrentHealthDeath()
    {
        _currentHealthIndex++;
        if (_currentHealthIndex >= _healths.Count)
        {
            OnDeath?.Invoke(this);
        }
        else
        {
            var oldHealth = _healths[_currentHealthIndex - 1];
            Destroy(oldHealth.gameObject);

            var newHealth = _healths[_currentHealthIndex];
            newHealth.gameObject.SetActive(true);
            newHealth.SetOnDeath(OnCurrentHealthDeath);

            OnHealthLost?.Invoke(this);
        }
    }
}
