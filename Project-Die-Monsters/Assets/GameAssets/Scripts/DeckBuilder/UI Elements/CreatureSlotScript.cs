using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureSlotScript : MonoBehaviour
{

    public Die myDie;
    public Creature myCreature; // creature scriptable object is contained wihtin dice scriptable object so all necessary display data is contained within both objects

    public Text CreatureName;
    public Image myImage;
    public Image myLevel;
    public Image CloseBTN;

    public int mySlot; // what slot I am in the List.
    GameObject dRef;
    public List<Sprite> SpriteList = new List<Sprite>();

    //public Sprite 
    // Start is called before the first frame update
    void Start()
    {
        HideMe();
        dRef = GameObject.FindGameObjectWithTag("DeckWindow");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveMe()
    {
        myDie = null;
        dRef.GetComponent<DeckBuilderUIManager>().tempDeck.RemoveAt(mySlot);
        dRef.GetComponent<DeckBuilderUIManager>().DisplayContents();

    }

    public void HideMe()
    {
        myImage.GetComponent<Image>().enabled = false;
        myLevel.GetComponent<Image>().enabled = false;
        CreatureName.GetComponent<Text>().enabled = false;
        CloseBTN.GetComponent<Image>().enabled = false;


    }
    public void ShowMe()
    {
        myImage.GetComponent<Image>().enabled = true;
        myLevel.GetComponent<Image>().enabled = true;
        CreatureName.GetComponent<Text>().enabled = true;
        CloseBTN.GetComponent<Image>().enabled = true;

        SetDetails();
        
    }

    public void SetDetails()
    {
        if (myDie != null)
        {
            myCreature = myDie.dieCreature;
            CreatureName.text = myCreature.CreatureName.ToString();
        }
    }
}
