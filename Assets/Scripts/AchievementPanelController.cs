using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPanelController : MonoBehaviour
{
    Text Title;
    Text Description;

    public void Awake()
    {
        AchievementManager.Instance.panelController = this;
        
        Title = GameObject.Find("Canvas").transform
            .Find("AchievmentPanel/Notification/Title").gameObject.GetComponent<Text>();
        Description = GameObject.Find("Canvas").transform
            .Find("AchievmentPanel/Notification/Description").gameObject.GetComponent<Text>();
    }
}
