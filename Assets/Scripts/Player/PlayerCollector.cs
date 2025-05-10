using System.Collections.Generic;
using UnityEngine;


/// Pulls collectibles toward the player smoothly using MoveTowards,
/// then collects them when within a specified distance, optionally granting XP.

public class PlayerCollector : MonoBehaviour
{
    [Header("Pull Settings")]
    [Tooltip("Speed at which items are pulled toward the player.")]
    [SerializeField] private float pullSpeed = 5f;

    [Tooltip("Distance at which the item is considered collected.")]
    [SerializeField] private float collectDistance = 0.5f;

    [Header("Experience")]
    [Tooltip("PlayerExperience component for awarding XP.")]
    [SerializeField] private PlayerExperience experience;

    // Parallel lists to track collectibles and their rigidbodies
    private readonly List<ICollectible> _collectibles = new List<ICollectible>();
    private readonly List<Rigidbody2D> _rigidbodies = new List<Rigidbody2D>();

    private void Awake()
    {
        // Auto-assign experience if not set
        if (experience == null)
            experience = FindObjectOfType<PlayerExperience>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ICollectible>(out var collectible)
            && other.attachedRigidbody != null)
        {
            // Start pulling this item
            _collectibles.Add(collectible);
            _rigidbodies.Add(other.attachedRigidbody);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<ICollectible>(out var collectible))
        {
            // Stop pulling if player moves away
            int index = _collectibles.IndexOf(collectible);
            if (index >= 0)
            {
                _collectibles.RemoveAt(index);
                _rigidbodies.RemoveAt(index);
            }
        }
    }

    private void Update()
    {
        for (int i = _collectibles.Count - 1; i >= 0; i--)
        {
            var collectible = _collectibles[i];
            var rb = _rigidbodies[i];

            if (collectible == null || rb == null)
            {
                // Clean up null references
                RemoveAt(i);
                continue;
            }

            // Smoothly move the item toward the player
            Vector2 targetPos = transform.position;
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, pullSpeed * Time.deltaTime);
            rb.MovePosition(newPos);

            // Check for collection threshold
            if (Vector2.Distance(newPos, targetPos) <= collectDistance)
            {
                // Award experience if collectible supports it
                if (experience != null && collectible is IExperienceReward reward)
                    experience.GainExperience(reward.ExperienceAmount);

                // Finalize collection
                collectible.Collect();
                RemoveAt(i);
            }
        }
    }

    private void RemoveAt(int i)
    {
        _collectibles.RemoveAt(i);
        _rigidbodies.RemoveAt(i);
    }
}





