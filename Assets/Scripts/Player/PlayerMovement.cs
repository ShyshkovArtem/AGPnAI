using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement
    [HideInInspector] public Vector2 moveDir;
    [HideInInspector] public float lastHorizontalVector;
    [HideInInspector] public float lastVerticalVector;
    [HideInInspector] public Vector2 lastMovedVector;

    // References
    private Rigidbody2D rb;
    private PlayerAttributes playerAttributes;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAttributes = GetComponent<PlayerAttributes>();

        if (rb == null)
            Debug.LogError("Rigidbody2D component missing from Player!");
        if (playerAttributes == null)
            Debug.LogError("PlayerAttributes component missing from Player!");


        lastMovedVector = Vector2.right;
    }


    void Update()
    {
        if (GameManager.instance.isGameOver)
            return;

        HandleInput();
    }


    void FixedUpdate()
    {
        if (GameManager.instance.isGameOver)
            return;

        Move();
    }


    void HandleInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontalVector, 0f);
        }

        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(0f, lastVerticalVector);
        }

        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector);
        }
    }


    void Move()
    {
        rb.velocity = moveDir * playerAttributes.CurrentMoveSpeed;
    }
}

