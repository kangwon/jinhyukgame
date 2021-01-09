using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestPanelController : MonoBehaviour
{
    Text chestType;
    Text chestDescription;
    StageChoice stageChoice;
    GameObject weaponChangePanel;

    Player player = GameState.Instance.player;

    public ChestCard ChestCard;

    void OnEnable()
    {
        chestType = GameObject.Find("ChestPanel/ChestType").GetComponent<Text>();
        chestDescription = GameObject.Find("ChestPanel/ChestDescription").GetComponent<Text>();
        stageChoice = GameObject.Find("Canvas").GetComponent<StageChoice>();
        weaponChangePanel = GameObject.Find("WeaponChangePanel");
    }
    
    public void OnClickTreasureButton()
    {
        switch (ChestCard.ChestType)
        {
            case ChestType.Equipment:
                player.SetEquipment(JsonDB.GetWeapon("weapon_000"));
                if (player.GetWeaponList().Count > 10)
                {
                    weaponChangePanel.SetActive(true);
                }
                break;
            case ChestType.Heal:
                break;
            case ChestType.Dispel:
                break;
            case ChestType.Damage:
                break;
            case ChestType.Debuff:
                break;
        }
       stageChoice.MoveToNextStage();
    }
}
