using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JetBrains.Annotations;

public class OnHoverEnable : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
    // Class that exists to toggle UI Extension objects that hide / show additonal details such as description text for what UI elements are.
    [SerializeField]
    GameObject[] ObjectsToEnable;

    private void Start()
    {
        foreach (var item in ObjectsToEnable)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var item in ObjectsToEnable)
        {
            item.gameObject.SetActive(true);
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var item in ObjectsToEnable)
        {
            item.gameObject.SetActive(false);
        }
    }
}
