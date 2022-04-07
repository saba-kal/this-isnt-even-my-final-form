using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class InteractableObject : MonoBehaviour
{
    public float InteractRadius = 1f;
    [SerializeField] private int _key = 1;
    [SerializeField] private UnityEvent _onInteract;

    private static Dictionary<int, InteractableObject> _allInteractibleObjects;

    private void Awake()
    {
        if (_allInteractibleObjects == null)
        {
            _allInteractibleObjects = new Dictionary<int, InteractableObject>();
        }
    }

    private void Start()
    {
        _allInteractibleObjects.TryAdd(_key, this);
    }

    private void OnDestroy()
    {
        if (_allInteractibleObjects.ContainsKey(_key))
        {
            _allInteractibleObjects.Remove(_key);
        }
    }

    public void Interact()
    {
        _onInteract?.Invoke();
        if (_allInteractibleObjects.ContainsKey(_key))
        {
            _allInteractibleObjects.Remove(_key);
        }
        Destroy(this);
    }

    public static InteractableObject GetNearestInteractable(Vector2 position)
    {
        InteractableObject result = null;
        var minSqrDistance = float.MaxValue;
        foreach (var interactible in _allInteractibleObjects.Values)
        {
            //Calculate distance using square magnitude for better performance.
            var squareDistance = ((Vector2)interactible.transform.position - position).sqrMagnitude;
            var maxSquareDistance = interactible.InteractRadius * interactible.InteractRadius;
            if (squareDistance < maxSquareDistance &&
                squareDistance < minSqrDistance)
            {
                result = interactible;
                minSqrDistance = interactible.InteractRadius * interactible.InteractRadius;
            }
        }

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, InteractRadius);
    }
}
