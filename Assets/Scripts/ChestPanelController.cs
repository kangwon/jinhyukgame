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

    void Start()
    {
        chestType = GameObject.Find("/Canvas/ChestPanel/ChestType").GetComponent<Text>();
        chestDescription = GameObject.Find("/Canvas/ChestPanel/ChestDescription").GetComponent<Text>();
        stageChoice = GameObject.Find("/Canva").GetComponent<StageChoice>();
        weaponChangePanel = GameObject.Find("/Canva/WeaponChangePanel");
    }
    
    public void OnClickTreasureButton()
    {
        player.SetEquipment(JsonDB.GetWeapon("weapon_000"));
        if (player.GetWeaponList().Count > 10)
        {
            weaponChangePanel.SetActive(true);
        }
       stageChoice.MoveToNextStage();
    }
}
