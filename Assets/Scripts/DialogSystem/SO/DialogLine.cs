using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog/Dialog Line")]
public class DialogLine : ScriptableObject
{
    public string speakerName;
    [TextArea(2, 5)] public string dialogText;
    public Sprite portrait;
}
