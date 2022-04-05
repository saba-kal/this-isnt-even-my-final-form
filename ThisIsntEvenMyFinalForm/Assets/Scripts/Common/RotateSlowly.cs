using UnityEngine;
using System.Collections;

public class RotateSlowly : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 50f;

    void Update()
    {
        transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
    }
}
