using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterHealth : MonoBehaviour
{
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

    private void OnCurrentHealthDeath()
    {
        var currentHealth = _healths[_currentHealthIndex];
        Destroy(currentHealth.gameObject);

        _currentHealthIndex++;
        if (_currentHealthIndex >= _healths.Count)
        {
            Debug.Log($"Character {gameObject.name} fully died");
            Destroy(gameObject); //TODO death effects.
        }
        else
        {
            var newHealth = _healths[_currentHealthIndex];
            newHealth.gameObject.SetActive(true);
            newHealth.SetOnDeath(OnCurrentHealthDeath);
        }
    }
}
