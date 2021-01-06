using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChestPanelController : MonoBehaviour
{
    public GameObject chestPanel;
    public GameObject chestType;
    public GameObject chestDescription;
    public StageChoice stageChoice;
    public GameObject weaponChangePanel;
    Player player = GameState.Instance.player;
    public void OnClickTreasureButton()
    {
        player.SetEquipment(JsonDB.GetEquipment("weapon_000"));
        if (player.GetWeaponList().Count > 10)
        {
            Debug.Log("무기 10장 넘었어요.");
            weaponChangePanel.SetActive(true);
        }
       stageChoice.MoveToNextStage();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
