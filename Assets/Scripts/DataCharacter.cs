using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NEW_CharacterData", menuName = "Data/Character Data")]
public class DataCharacter : ScriptableObject
{
    public string characterName;
    public Sprite characterImage;
    public int damage;
    public int maxHP;
    public List<Sprite> attackFrames;
}
