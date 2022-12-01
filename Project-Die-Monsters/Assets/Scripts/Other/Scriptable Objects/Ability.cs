using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ability : ScriptableObject
{
    public AbilityType abilityType;
    //Type
    //Effect Script
    //Cost
    //Else?
}

public enum AbilityType
{
    Activate, Trigger, None
}