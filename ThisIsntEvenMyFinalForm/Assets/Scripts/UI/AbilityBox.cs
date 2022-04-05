using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBox : MonoBehaviour
{
    [SerializeField] private PlayerAbilityType _type;
    [SerializeField] private PlayerAbilityManager _playerAbilityManager;
    [SerializeField] private Image _reloadMask;
    [SerializeField] private GameObject _newAbilityIndicator;
    [SerializeField] private float _newAbilityIndicatorTime = 10f;

    private float? _abilityCooldownTime = null;
    private float _abilityTimer = 0f;

    private void OnEnable()
    {
        PowerLevelManager.OnPowerUp += UpdateAbilityBoxUI;
        ShowNewAbilityIndicator();
    }

    private void OnDisable()
    {
        PowerLevelManager.OnPowerUp -= UpdateAbilityBoxUI;
    }

    private void Start()
    {
        UpdateAbilityBoxUI(1);
    }

    public PlayerAbilityType GetAbilityType()
    {
        return _type;
    }

    private void Update()
    {
        if (_abilityCooldownTime == null)
        {
            //Ability has no cooldown.
            return;
        }

        if (_abilityTimer > 0)
        {
            _abilityTimer -= Time.deltaTime;
            _reloadMask.fillAmount = _abilityTimer / _abilityCooldownTime.Value;
        }
        else
        {
            _abilityTimer = 0f;
        }
    }

    private void UpdateAbilityBoxUI(int _)
    {
        _abilityCooldownTime = _playerAbilityManager.GetAbilityCooldown(_type);
        _playerAbilityManager.SetOnAbilityActivated(StartAbilityCooldown, _type);
    }

    private void StartAbilityCooldown()
    {
        if (_abilityCooldownTime == null)
        {
            //Ability has no cooldown.
            return;
        }

        _abilityTimer = _abilityCooldownTime.Value;
        _reloadMask.fillAmount = 1f;
    }

    private void ShowNewAbilityIndicator()
    {
        _newAbilityIndicator.SetActive(true);
        StartCoroutine(HideNewAbilityIndicator());
    }

    private IEnumerator HideNewAbilityIndicator()
    {
        yield return new WaitForSeconds(_newAbilityIndicatorTime);
        _newAbilityIndicator.SetActive(false);
    }
}
