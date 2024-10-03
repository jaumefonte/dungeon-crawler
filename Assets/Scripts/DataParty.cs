using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NEW_PartyData", menuName = "Data/Party Data")]
public class DataParty : ScriptableObject
{
    public List<DataCharacter> partyMembers;
}
