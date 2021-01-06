using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponPopupView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject WeaponPopupScreen;
    int popupCheck = 0;

    public void OnPointerDown(PointerEventData data)
    {
        if (popupCheck == 0)
        {
            WeaponPopupScreen.SetActive(true);
            popupCheck = 1;
        }
        else
        {
            WeaponPopupScreen.SetActive(false);
            popupCheck = 0;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        // WeaponPopupScreen.SetActive(false);
    }
}