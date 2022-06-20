using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChoice : MonoBehaviour
{
    public string MyChoice = "None";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChoiceToMake()
    {
        switch (MyChoice)
        {
            case "DieToKeep":
                WhatDieDoIWant();
                break;
        }
    }

    public void WhatDieDoIWant()
    {
        
    }
}
