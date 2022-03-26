using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class Bullet : BaseBullet
{
    [SerializeField] protected GameObject _onDestroyEffect;

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
        if (_onDestroyEffect == null || !gameObject.scene.isLoaded)
        {
            return;
        }

        var effect = Instantiate(_onDestroyEffect);
        effect.transform.position = transform.position;
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
