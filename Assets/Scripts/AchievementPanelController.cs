using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPanelController : MonoBehaviour
{
    Text Title;
    Text Description;

    public AchievementPanelController()
    {
        AchievementManager.Instance.panelController = this;
    }

    public void Awake()
    {
        Title = GameObject.Find("Canvas").transform
            .Find("AchievmentPanel/Notification/Title").gameObject.GetComponent<Text>();
        Description = GameObject.Find("Canvas").transform
            .Find("AchievmentPanel/Notification/Description").gameObject.GetComponent<Text>();
    }

    public void Show(Achievement achievement)
    {
        this.gameObject.SetActive(true);
        Title.text = achievement.name;
        Description.text = achievement.description;
    }
}
