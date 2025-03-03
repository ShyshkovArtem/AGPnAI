using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PassiveItemScriptableObject", menuName ="ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multipler;
    public float Multipler { get => multipler; private set => multipler = value; }

    [SerializeField]
    int level;  //Should be modified only in the editor, NOT the game
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab; //The prefab of the next level
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }

}
