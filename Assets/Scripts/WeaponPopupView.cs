using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

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
            WeaponIcon[i] = GameObject.Find($"WeaponPopupView").transform.Find($"WeaponPopupScreen/Weapon{i+1}").gameObject;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (popupCheck == 0)
        {
            string temp;
            for(int i=0;i<10;i++)
            {              
                switch (player.GetWeaponList().ElementAt(i).weaponType)
                {
                    case WeaponType.sword:
                        temp = "검";
                        break;
                    case WeaponType.blunt:
                        temp = "둔기";
                        break;
                    case WeaponType.spear:
                        temp = "창";
                        break;
                    case WeaponType.dagger:
                        temp = "단검";
                        break;
                    case WeaponType.wand:
                        temp = "지팡이";
                        break;
                    default:
                        temp = "?";
                        break;
                }
                WeaponIcon[i].transform.GetChild(0).GetComponent<Text>().text = $"{temp}";
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