using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentChangePanelController : MonoBehaviour
{
    Player player;
    Equipment afterEquipment;

    public delegate void EquipmentAcceptCallback(Equipment equipment);
    EquipmentAcceptCallback acceptCallback;

    Text BeforeStatText, BeforeNameText;
    Text AfterStatText, AfterNameText;
    Text GoldText;

    Button AcceptButton;
    Button CancelButton;

    void Start()
    {
        BeforeStatText = GameObject.Find("EquipmentChangePanel/BeforeStatText").GetComponent<Text>();
        BeforeNameText = GameObject.Find("EquipmentChangePanel/BeforeNameText").GetComponent<Text>();
        AfterStatText = GameObject.Find("EquipmentChangePanel/AfterStatText").GetComponent<Text>();
        AfterNameText = GameObject.Find("EquipmentChangePanel/AfterNameText").GetComponent<Text>();
        GoldText = GameObject.Find("EquipmentChangePanel/GoldText").GetComponent<Text>();
        
        AcceptButton = GameObject.Find("EquipmentChangePanel/AcceptButton").GetComponent<Button>();
        AcceptButton.onClick.AddListener(OnClickAcceptButton);
        CancelButton = GameObject.Find("EquipmentChangePanel/CancelButton").GetComponent<Button>();
        CancelButton.onClick.AddListener(OnClickCancelButton);
        
        this.gameObject.SetActive(false);
    }

    Equipment GetBeforeEquipment()
    {
        switch (afterEquipment.type)
        {
            case "armor":
                return player.GetArmor();
            case "helmet":
                return player.GetHelmet();
            case "shoes":
                return player.GetShoes();
            default:
                throw new NotImplementedException($"Invalid equipment type: {afterEquipment.type}");
        }
    }

    string GetStatString(Stat stat)
    {
        return $"체 {stat.maxHp}\n\n공 {stat.attack}\n\n방 {stat.defense}\n\n속 {stat.speed}";
    }

    void UpdateEquipmentInfo()
    {
        Equipment beforeEquipment = GetBeforeEquipment();
        Stat beforeStat = GetBeforeEquipment().statEffect;
        BeforeStatText.text = GetStatString(beforeStat);
        BeforeNameText.text = beforeEquipment.name;

        Stat afterStat = afterEquipment.statEffect;
        AfterStatText.text = GetStatString(afterStat);
        AfterNameText.text = afterEquipment.name;

        GoldText.text = $"{afterEquipment.price} G";
    }

    void OnClickAcceptButton()
    {
        this.acceptCallback(afterEquipment);
        this.gameObject.SetActive(false);
    }

    void OnClickCancelButton()
    {
        this.gameObject.SetActive(false);
    }

    // 이거 써서 불러내면 됨
    public void DisplayPanel(Equipment equipment, EquipmentAcceptCallback acceptCallback)
    {
        this.player = GameState.Instance.player;
        this.afterEquipment = equipment;
        this.acceptCallback = acceptCallback;

        this.UpdateEquipmentInfo();

        this.transform.localPosition = StageChoice.PanelDisplayPosition;
        this.gameObject.SetActive(true);
    }
}
