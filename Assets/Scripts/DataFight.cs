using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NEW_FightData", menuName = "Data/Fight Data")]
public class DataFight : ScriptableObject
{
    public List<DataCharacter> enemiesInFight;
}
