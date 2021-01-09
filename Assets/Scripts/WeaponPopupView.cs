using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponPopupView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject WeaponPopupScreen;
    GameObject[] WeaponIcon = new GameObject[10] ;
    Player player = GameState.Instance.player;
    int popupCheck = 0;


    private void Start() 
    {
        for(int i=0;i<10;i++)
        {
            WeaponIcon[i] = GameObject.Find($"WeaponPopupView").transform.Find($"WeaponPopupView/WeaponPopupScreen/Weapon{i+1}");
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (popupCheck == 0)
        {
            for(int i=0;i<10;i++)
            {             
                weaponIcon[i].transform.GetChild(0).GetComponent<Text>().text = $"{player.GetWeaponList().ElementAt(i).attack}";
            }
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