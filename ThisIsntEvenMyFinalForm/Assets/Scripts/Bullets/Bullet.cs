using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 10f;
    [SerializeField] private GameObject _onDestroyEffect;

    private Vector2 _direction;
    private Health _health;

    void Start()
    {
        _health = GetComponent<Health>();
        Destroy(gameObject, _lifetime);
    }

    void Update()
    {
        transform.Translate(_direction.normalized * _speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var otherObjectsHealth = collision.gameObject.GetComponentInChildren<Health>();
        otherObjectsHealth?.TakeDamage(_damage);

        if (collision.gameObject.tag != Tags.BULLET)
        {
            //Colliding with a non-bullet object should destroy this bullet.
            Destroy(gameObject);
        }

        StartCoroutine(IncreaseLightBrightness());
    }

    private void OnDestroy()
    {
        if (_onDestroyEffect == null)
        {
            return;
        }

        var effect = Instantiate(_onDestroyEffect);
        effect.transform.position = transform.position;
        Destroy(effect, 2f);
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private IEnumerator IncreaseLightBrightness()
    {
        //TODO: move this to separate component.
        var lights = GetComponentsInChildren<Light2D>();

        foreach (var light in lights)
        {
            light.intensity *= 2;
        }

        yield return new WaitForSeconds(0.2f);

        foreach (var light in lights)
        {
            light.intensity /= 2;
        }

        yield return null;
    }
}
