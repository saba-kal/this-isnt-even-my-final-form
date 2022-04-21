using UnityEngine;
using System.Collections;

public class EnableDisableObjectInIntervals : MonoBehaviour
{
    [SerializeField] private float _interval = 1f;
    [SerializeField] private GameObject _objectToDisable;

    private float _elapsedTime = 0f;

    void Update()
    {
        if (_elapsedTime >= _interval)
        {
            _objectToDisable.SetActive(!_objectToDisable.activeSelf);
            _elapsedTime = 0;
        }

        _elapsedTime += Time.deltaTime;
    }
}
