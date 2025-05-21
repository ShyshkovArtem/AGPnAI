using UnityEngine;

[CreateAssetMenu(fileName = "AI State", menuName = "AI/Dialog AI State")]
public class AIState : ScriptableObject
{
    public AIEmotion emotion;
    public float trustLevel;  // 0–100
    public AIMorality morality;
}

public enum AIEmotion { Friendly, Neutral, Hostile }
public enum AIMorality { Good, Neutral, Evil }
