﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSelectPanelController : MonoBehaviour
{
    public delegate void WorldSelectCallback(WorldId selectedWorld);

    Button WorldButton1, WorldButton2;

    void Start()
    {
        WorldButton1 = GameObject.Find("WorldSelectPanel/WorldButton1").GetComponent<Button>();
        WorldButton2 = GameObject.Find("WorldSelectPanel/WorldButton2").GetComponent<Button>();

        this.gameObject.SetActive(false);
    }

    public void DisplayPanel(WorldId world1, WorldId world2, WorldSelectCallback callback)
    {
        WorldButton1.GetComponentInChildren<Text>().text = world1.ToString(); 
        WorldButton1.onClick.AddListener(() => 
        {
            this.gameObject.SetActive(false);
            callback(world1);
        });
        WorldButton2.GetComponentInChildren<Text>().text = world2.ToString();
        WorldButton2.onClick.AddListener(() => 
        {
            this.gameObject.SetActive(false);
            callback(world2);
        });

        this.transform.localPosition = StageChoice.PanelDisplayPosition;
        this.gameObject.SetActive(true);
    }
}
