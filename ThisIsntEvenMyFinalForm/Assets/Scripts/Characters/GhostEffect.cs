using UnityEngine;
using System.Collections;

public class GhostEffect : MonoBehaviour
{
    [SerializeField] private float _delay = 0.5f;
    [SerializeField] private float _fadeAmount = 1f;

    private float _timeSinceLastGhostSpawn = 0f;

    private void Update()
    {
        if (_timeSinceLastGhostSpawn >= _delay)
        {
            var spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            var ghost = new GameObject("ghost");
            ghost.transform.position = spriteRenderer.transform.position;
            ghost.transform.rotation = spriteRenderer.transform.rotation;
            ghost.transform.localScale = spriteRenderer.transform.localScale;

            var ghostSpriteRenderer = ghost.AddComponent<SpriteRenderer>();
            ghostSpriteRenderer.sprite = spriteRenderer.sprite;
            ghostSpriteRenderer.color = spriteRenderer.color;

            StartCoroutine(FadeGhost(ghostSpriteRenderer));

            _timeSinceLastGhostSpawn = 0f;
        }
        else
        {
            _timeSinceLastGhostSpawn += Time.deltaTime;
        }
    }

    private IEnumerator FadeGhost(SpriteRenderer ghostSpriteRenderer)
    {
        while (ghostSpriteRenderer.color.a > 0)
        {
            ghostSpriteRenderer.color = new Color(
                ghostSpriteRenderer.color.r,
                ghostSpriteRenderer.color.g,
                ghostSpriteRenderer.color.b,
                ghostSpriteRenderer.color.a - _fadeAmount * Time.deltaTime);
            yield return null;
        }

        Destroy(ghostSpriteRenderer.gameObject);
    }
}
