using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SwipeMovement : MonoSingleton<SwipeMovement>
{
    [Header("Movement")]
    [SerializeField] private float dragMultiplier = 5f;
    [SerializeField] private Vector2 bounds;

    private float _mouseStartPos;
    private float _mouseCurrentPos;
    private float _playerCurrentPos;
    
    private void Start()
    {
        _playerCurrentPos = transform.localPosition.x;
        _mouseStartPos = Input.mousePosition.x;
        _mouseStartPos /= Screen.width;
    }


    private void Update()
    {
        if (!GameplayController.Instance.IsActive) return;
        HandLeMovement();
    }

    private void HandLeMovement()
    {

        if (Input.GetMouseButtonDown(0))
        {
            _playerCurrentPos = transform.localPosition.x;
            _mouseStartPos = Input.mousePosition.x;
            _mouseStartPos /= Screen.width;

        }
        if (Input.GetMouseButton(0))
        {
            var localPos = transform.localPosition;
            _mouseCurrentPos = Input.mousePosition.x;
            _mouseCurrentPos /= Screen.width;
            var targetPos = _playerCurrentPos + (_mouseCurrentPos - _mouseStartPos) * dragMultiplier;
            targetPos = Mathf.Clamp(targetPos, bounds.x, bounds.y);

            transform.localPosition = new Vector3(targetPos, localPos.y, localPos.z);

        }

    }

    
}