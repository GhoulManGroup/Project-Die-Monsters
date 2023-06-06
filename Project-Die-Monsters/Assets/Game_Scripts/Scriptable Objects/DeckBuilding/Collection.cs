using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DeckCollection", menuName = "DeckCollection/Collection")]
public class Collection : ScriptableObject
{

    public List<Die> collectedDie = new List<Die>(); 

}
