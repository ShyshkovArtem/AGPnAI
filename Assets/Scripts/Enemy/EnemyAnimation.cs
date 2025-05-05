using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAnimation : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer sr;
    private Animator animator;
    private EnemyStats stats;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerMovement>().transform;
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (SceneManager.GetActiveScene().name != "TitleScreen")
        {
            if (transform.position.x > player.position.x)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        } 
    }


    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void PlayDeath()
    {
        animator.SetTrigger("Death");
    }
}
