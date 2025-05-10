using UnityEngine;

/// Handles player animations by updating Animator parameters
/// and sprite orientation based on movement input.

public class PlayerAnimation : MonoBehaviour
{
    [Header("Dependencies")]
    [Tooltip("Animator controlling the player animations.")]
    [SerializeField] private Animator _animator;

    [Tooltip("Component providing movement direction.")]
    [SerializeField] private PlayerMovement _movement;

    [Tooltip("SpriteRenderer for flipping the sprite.")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");

    private bool _wasMoving;

    private void Awake()
    {
        // Resolve dependencies or warn
        _animator = _animator ?? GetComponent<Animator>() ?? WarnMissing<Animator>();
        _movement = _movement ?? GetComponent<PlayerMovement>() ?? WarnMissing<PlayerMovement>();
        _spriteRenderer = _spriteRenderer ?? GetComponent<SpriteRenderer>() ?? WarnMissing<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 dir = _movement.MoveDirection;
        bool isMoving = dir.sqrMagnitude > 0f;

        // Update Animator only when movement state changes
        if (isMoving != _wasMoving)
        {
            _wasMoving = isMoving;
            _animator.SetBool(IsMovingHash, isMoving);
        }

        // Flip sprite only on horizontal input; retain last direction when idle
        if (dir.x > 0f && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if (dir.x < 0f && !_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = true;
        }
    }

    /// <summary>
    /// Allows swapping animator controller at runtime.
    /// </summary>
    public void SetAnimatorController(RuntimeAnimatorController controller)
    {
        if (_animator == null)
            _animator = GetComponent<Animator>() ?? WarnMissing<Animator>();
        _animator.runtimeAnimatorController = controller;
    }

    // Utility for logging missing components
    private T WarnMissing<T>() where T : class
    {
        Debug.LogError($"{typeof(T).Name} is missing on {gameObject.name}", this);
        return null;
    }
}

