using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialog Node", menuName = "Dialog/Branching Dialog Node")]
public class DialogNode : ScriptableObject
{
    public string speakerName;
    [TextArea(2, 5)] public string dialogText;
    public Sprite portrait;

    public List<DialogChoice> choices;
}


[System.Serializable]
public class DialogChoice
{
    public string choiceText;
    public DialogNode nextNode;

    public AIEmotion? requiredEmotion;
    public float minTrust = 0;
    public float maxTrust = 100;
    public AIMorality? requiredMorality;
}
