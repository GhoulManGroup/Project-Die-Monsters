using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuilderDie : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject dRef;
    public string myState;
    public Die myDie; // which dice this object represents.
    public Sprite defaultSprite;
    Image myImage;
    public GameObject cardDisplay;

    // Start is called before the first frame update
    void Start()
    {
        dRef = GameObject.FindGameObjectWithTag("DeckWindow");
        myImage = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPressed()
    {

        if (dRef.GetComponent<DeckBuilderUIManager>().tempDeck.Count < 15)
        {
            bool lookforMe = false;
            for (int i = 0; i < dRef.GetComponent<DeckBuilderUIManager>().tempDeck.Count; i++)
            {
                if (dRef.GetComponent<DeckBuilderUIManager>().tempDeck[i].dieID == myDie.dieID)
                {
                    lookforMe = true;
                }

            }
            if (lookforMe == false)
            {

                dRef.GetComponent<DeckBuilderUIManager>().tempDeck.Add(myDie);
                dRef.GetComponent<DeckBuilderUIManager>().DisplayContents();
            }

            if (lookforMe == true)
            {
                Debug.Log("already in");
            }
        }
    }

    public void SetDetails()
    {
        if (myState == "Used")
        {
            myImage.sprite = myDie.dieCreature.CardArt;
        }

        if (myState == "Empty")
        {
            myImage.sprite = defaultSprite;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myState == "Used")
        {
            cardDisplay.GetComponent<Image>().sprite = myDie.dieCreature.CardArt;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (myState == "Used")
        {
            cardDisplay.GetComponent<Image>().sprite = defaultSprite;
        }
    }

    public void HideMe()
    {
        this.GetComponent<Image>().enabled = false;
    }
    
    public void ShowMe()
    {
        this.GetComponent<Image>().enabled = true;
    }
}
