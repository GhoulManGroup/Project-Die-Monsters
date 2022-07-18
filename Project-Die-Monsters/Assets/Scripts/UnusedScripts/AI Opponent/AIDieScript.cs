using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIDieScript : MonoBehaviour
{
    public GameObject myColor;
    public GameObject myIcon;
    public Die myDie;

    string diecrest = "Level";
    string diecolor;

    float frame = 24;
    int timer = 8;

    public string myResult = "Level";

    [Header("DieSpriteList")]
    public List<Sprite> dieColorSpites = new List<Sprite>();
    public List<Sprite> crestSprites = new List<Sprite>();

    // Start is called before the first frame update
    public void Start()
    {
        myColor.GetComponent<Image>().enabled = false;
        myIcon.GetComponent<Image>().enabled = false;
    }
    public void showUI()
    {
        myColor.GetComponent<Image>().enabled = true;
        myIcon.GetComponent<Image>().enabled = true;
    }

    public void SetImage()
    {
        diecolor = myDie.dieColor.ToString();
        switch (diecolor)
        {
            case "Blue":
                myColor.GetComponent<Image>().sprite = dieColorSpites[0];
                break;
            case "Red":
                myColor.GetComponent<Image>().sprite = dieColorSpites[1];
                break;
            case "Green":
                myColor.GetComponent<Image>().sprite = dieColorSpites[2];
                break;
            case "Yellow":
                myColor.GetComponent<Image>().sprite = dieColorSpites[3];
                break;
            case "White":
                myColor.GetComponent<Image>().sprite = dieColorSpites[4];
                break;
            case "Black":
                myColor.GetComponent<Image>().sprite = dieColorSpites[5];
                break;
        }

        switch (diecrest)
        {
            case "Level":
                switch (myDie.dieCreatureLevel.ToString())
                {
                    case "one":
                        myIcon.GetComponent<Image>().sprite = crestSprites[4];
                        myResult = "L1C";
                        break;
                    case "two":
                        myIcon.GetComponent<Image>().sprite = crestSprites[5];
                        myResult = "L2C";
                        break;
                    case "three":
                        myIcon.GetComponent<Image>().sprite = crestSprites[6];
                        myResult = "L3C";
                        break;
                    case "four":
                        myIcon.GetComponent<Image>().sprite = crestSprites[7];
                        myResult = "L4C";
                        break;
                }
                break;
            case "Move":
                myIcon.GetComponent<Image>().sprite = crestSprites[0];
                myResult = "Move";
                break;
            case "Attack":
                myIcon.GetComponent<Image>().sprite = crestSprites[1];
                myResult = "Attack";
                break;
            case "Defence":
                myIcon.GetComponent<Image>().sprite = crestSprites[2];
                myResult = "Defence";
                break;
            case "AP":
                myIcon.GetComponent<Image>().sprite = crestSprites[3];
                myResult = "AP";
                break;
        }
    }

    public void RollAnimation()
    {
        frame -= 1;
        if (frame <= 0)
        {
            
        }
    }
    public void RollResult()
    {
       
    }

    public void removeDie()
    {
        myDie = null;
    }
}
