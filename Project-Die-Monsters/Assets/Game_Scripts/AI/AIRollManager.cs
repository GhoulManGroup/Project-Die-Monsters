using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRollManager : MonoBehaviour
{
    [Header("3D Dice Rolling")]
    [SerializeField] GameObject diceFab;
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> DiceToRoll = new List<GameObject>();
    
    [SerializeField] GameObject LVLRef;
    [SerializeField] AIManager mRef;

    public void SetUpAIDice()
    {
        mRef = this.GetComponent<AIManager>();
        LVLRef = GameObject.FindGameObjectWithTag("LevelController");
        LVLRef.GetComponent<CameraController>().switchCamera("Dice");


        //Spawn a dice at the spawn points whose position in the list is equal to the number of dice already in the space.
        GameObject DiceSpawned;
        for (int i = 0; i < 3; i++)
        {
            DiceSpawned = Instantiate(diceFab, spawnPoints[DiceToRoll.Count].transform.position, Quaternion.identity);
            DiceToRoll.Add(DiceSpawned);
        }
        
        StartCoroutine("AssignCreatureToDice");
    }

    private IEnumerator AssignCreatureToDice()
    {
       
        yield return null;
    }

    private IEnumerator RollDice()
    {
        yield return null;
    }

    private IEnumerator countCrests()
    {
        yield return null;
    }
}
