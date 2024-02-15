using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICreatureController : MonoBehaviour
{
    public List<GameObject> myCreatures = new List<GameObject>();
    

    public IEnumerator PerformAction()
    {
        yield return StartCoroutine(AICreatureAttack());

        yield return StartCoroutine(AICreatureCastAbility());

        yield return StartCoroutine(AICreatureMove());

        yield return null;
    }
    IEnumerator AICreatureAttack()
    {
        yield return null;
    }

    IEnumerator AICreatureCastAbility()
    {
        yield return null;
    }

    IEnumerator AICreatureMove()
    {
        yield return null;
    }

    // for each creature check possible actions that can be taken

    //Attack

    //Move

    //Ability 

}
