﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class WeaponPopupView : MonoBehaviour, IPointerDownHandler
{

    public GameObject WeaponPopupScreen;
    GameObject[] WeaponIcon = new GameObject[10] ;
    int popupCheck = 0;

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            WeaponIcon[i] = GameObject.Find($"WeaponPopupView").transform.Find($"WeaponPopupScreen/Weapon{i + 1}").gameObject;
        }

    }

    public void OnPointerDown(PointerEventData data)
    {
        Player player = GameState.Instance.player;
        if (popupCheck == 0)
        {
            string temp;
            WeaponPopupScreen.SetActive(true);
            for (int i=0;i<10;i++)
            {              
                switch (player.GetWeaponList().ElementAt(i).weaponType)
                {
                    case WeaponType.sword:
                        WeaponIcon[i].GetComponent<Image>().sprite = Resources.Load("Img/sword", typeof(Sprite)) as Sprite;
                        temp = "검";
                        break;
                    case WeaponType.blunt:
                        WeaponIcon[i].GetComponent<Image>().sprite = Resources.Load("Img/hammer", typeof(Sprite)) as Sprite;
                        temp = "둔기";
                        break;
                    case WeaponType.spear:
                        WeaponIcon[i].GetComponent<Image>().sprite = Resources.Load("Img/spear", typeof(Sprite)) as Sprite;
                        temp = "창";
                        break;
                    case WeaponType.dagger:
                        WeaponIcon[i].GetComponent<Image>().sprite = Resources.Load("Img/knife", typeof(Sprite)) as Sprite;
                        temp = "단검";
                        break;
                    case WeaponType.wand:
                        WeaponIcon[i].GetComponent<Image>().sprite = Resources.Load("Img/wand", typeof(Sprite)) as Sprite;
                        temp = "지팡이";
                        break;
                    default:
                        WeaponIcon[i].GetComponent<Image>().sprite = Resources.Load("Img/fist", typeof(Sprite)) as Sprite;
                        temp = "맨주먹";
                        break;
                }
                WeaponIcon[i].transform.GetChild(0).GetComponent<Text>().text = $"{temp}";
            }
            popupCheck = 1;
        }
        else
        {
            WeaponPopupScreen.SetActive(false);
            popupCheck = 0;
        }
    }
}