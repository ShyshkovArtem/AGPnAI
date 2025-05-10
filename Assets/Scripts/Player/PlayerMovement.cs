using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player movement and input. Keeps last moved direction for animations or facing logic.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    // === Constants ===
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    // === Movement State ===
    [HideInInspector] public Vector2 MoveDirection { get; private set; }
    [HideInInspector] public Vector2 LastMovedDirection { get; private set; }

    private float _lastHorizontalInput;
    private float _lastVerticalInput;

    // === References ===
    private Rigidbody2D _rigidbody;
    private PlayerAttributes _attributes;

    // === Delegates (Strategy-like abstraction for input source) ===
    private System.Func<float> _getHorizontalInput;
    private System.Func<float> _getVerticalInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _attributes = GetComponent<PlayerAttributes>();

        if (_rigidbody == null)
            Debug.LogWarning("Missing Rigidbody2D on Player");
        if (_attributes == null)
            Debug.LogWarning("Missing PlayerAttributes on Player");

        // Inject input source (can be replaced for testing or alternate controls)
        _getHorizontalInput = () => Input.GetAxisRaw(HorizontalAxis);       //GPT recommended, could be useful, but later
        _getVerticalInput = () => Input.GetAxisRaw(VerticalAxis);

        LastMovedDirection = Vector2.right;
    }

    private void Update()
    {
        if (IsGameOver())
            return;

        HandleInput();
    }

    private void FixedUpdate()
    {
        if (IsGameOver())
            return;

        MovePlayer();
    }

    private void HandleInput()
    {
        float moveX = _getHorizontalInput();
        float moveY = _getVerticalInput();

        MoveDirection = new Vector2(moveX, moveY).normalized;

        if (MoveDirection.x != 0)
        {
            _lastHorizontalInput = MoveDirection.x;
            LastMovedDirection = new Vector2(_lastHorizontalInput, 0f);
        }

        if (MoveDirection.y != 0)
        {
            _lastVerticalInput = MoveDirection.y;
            LastMovedDirection = new Vector2(0f, _lastVerticalInput);
        }

        if (MoveDirection.x != 0 && MoveDirection.y != 0)
        {
            LastMovedDirection = new Vector2(_lastHorizontalInput, _lastVerticalInput);
        }
    }

    private void MovePlayer()
    {
        _rigidbody.velocity = MoveDirection * _attributes.CurrentMoveSpeed;
    }

    private bool IsGameOver()
    {
        return GameManager.instance != null && GameManager.instance.isGameOver;
    }
}


