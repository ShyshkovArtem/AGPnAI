using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;

    private static readonly int IsMoving = Animator.StringToHash("IsMoving");


    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        if (animator == null)
            Debug.LogError("Animator component missing from Player!");
        if (playerMovement == null)
            Debug.LogError("PlayerMovement component missing from Player!");
        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer component missing from Player!");
    }


    void Update()
    {
        UpdateAnimation();
    }


    void UpdateAnimation()
    {
        if (playerMovement.moveDir.x != 0 || playerMovement.moveDir.y != 0)
        {
            animator.SetBool(IsMoving, true);
            SpriteDirectionCheck();
        }
        else
        {
            animator.SetBool(IsMoving, false);
        }
    }


    void SpriteDirectionCheck()
    {
        spriteRenderer.flipX = playerMovement.lastHorizontalVector < 0;
    }


    public void SetAnimatorController(RuntimeAnimatorController controller)
    {
        if (animator == null) animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = controller;
    }
}

