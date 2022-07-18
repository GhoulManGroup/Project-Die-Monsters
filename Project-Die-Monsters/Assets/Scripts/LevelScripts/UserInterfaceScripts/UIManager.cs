using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour // This class will oversee the enabling and hiding of diffrent UI Elements
{
    public GameObject DiceSelectInteface;
    public GameObject DiceRollInteractUI;
    public GameObject InspectWindow;
    public GameObject CreaturePoolUI;
    public GameObject CreatureControlUI;
    public GameObject AttackWindow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void test()
    {
        DiceSelectInteface.SetActive(false);
    }
}
