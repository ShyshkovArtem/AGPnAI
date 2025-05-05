using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    [SerializeField] private float pullSpeed;
    private PlayerAttributes playerAttributes;
    private IColliderManager colliderManager;

    void Start()
    {
        playerAttributes = FindObjectOfType<PlayerAttributes>();
        colliderManager = new CircleColliderManager(GetComponent<CircleCollider2D>(), playerAttributes);
    }

    void Update()
    {
        colliderManager.UpdateColliderRadius();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collectible = collision.gameObject.GetComponent<ICollectible>();
        if (collectible != null)
        {
            CollectItem(collision, collectible);
        }
    }

    private void CollectItem(Collider2D collision, ICollectible collectible)
    {
        var itemForceApplier = new ItemForceApplier(collision.gameObject.GetComponent<Rigidbody2D>(), pullSpeed);
        itemForceApplier.ApplyForce(transform.position);

        collectible.Collect();
    }
}




