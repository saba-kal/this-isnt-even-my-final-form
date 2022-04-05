using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityBoxManager : MonoBehaviour
{
    [SerializeField] private List<AbilityBox> _abilityBoxes;
    [SerializeField] private PlayerAbilityManager _playerAbilityManager;

    private void OnEnable()
    {
        PowerLevelManager.OnPowerUp += UpdateAbilityBoxes;
    }

    private void OnDisable()
    {
        PowerLevelManager.OnPowerUp -= UpdateAbilityBoxes;
    }

    // Use this for initialization
    void Start()
    {
        UpdateAbilityBoxes(0);
    }

    private void UpdateAbilityBoxes(int _)
    {
        foreach (var abilityBox in _abilityBoxes)
        {
            abilityBox.gameObject.SetActive(_playerAbilityManager.PlayerAbilityIsUnlocked(abilityBox.GetAbilityType()));
        }
    }
}
