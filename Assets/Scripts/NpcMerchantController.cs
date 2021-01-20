using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcMerchantController : MonoBehaviour
{
    Button[] SaleButtons = new Button[3];
    Text StatusText;

    StageChoice stageChoice;
    GameObject weaponChangePanel;
    EquipmentChangePanelController equipmentChanger;

    public Equipment[] EquipmentsOnSale;

    void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            SaleButtons[i] = GameObject.Find("Canvas").transform
                .Find($"NpcPanel_Merchant/Sale{i + 1}").gameObject.GetComponent<Button>();
        }
        StatusText = GameObject.Find("Canvas").transform
                .Find("NpcPanel_Merchant/StatusText").gameObject.GetComponent<Text>();

        stageChoice = GameObject.Find("Canvas").GetComponent<StageChoice>();
        weaponChangePanel = GameObject.Find("Canvas").transform.Find("WeaponChangePanel").gameObject;
        equipmentChanger = GameObject.Find("Canvas").transform.Find("EquipmentChangePanel").gameObject.GetComponent<EquipmentChangePanelController>();
    }

    void OnEnable()
    {
        if(EquipmentsOnSale != null && EquipmentsOnSale.Length == 3)
        {
            for(int i = 0; i < 3; i++)
            {
                var equipment = EquipmentsOnSale[i];
                var button = SaleButtons[i];
                button.GetComponentInChildren<Text>().text = equipment.name;
                
                // TODO: 방어구도 이미지 생기면 적용
                var image = button.transform.Find("Image").gameObject.GetComponent<Image>();
                if (equipment.type == "weapon")
                {
                    var weapon = equipment as Weapon;
                    image.sprite = weapon.weaponImg;   
                }
                else
                {
                    image.sprite = null;
                }

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnClickSaleButton(equipment));
            }
        }

        if (StatusText != null)
        {
            StatusText.text = "상인을 만났다";
        }
    }

    void OnClickSaleButton(Equipment equipment)
    {
        var player = GameState.Instance.player;
        if (equipment.type == "weapon")
        {
            if (player.BuyItem(equipment))
            {
                if (player.GetWeaponList().Count > 10)
                    weaponChangePanel.SetActive(true);
                this.gameObject.SetActive(false);
                stageChoice.MoveToNextStage();
            }
            else
            {
                StatusText.text = "돈이 부족해";
            }
        }
        else
        {
            if (player.BuyItem(equipment))
            {
                equipmentChanger.DisplayPanel(equipment, (e) => 
                {
                    player.BuyItem(e);
                    this.gameObject.SetActive(false);
                    stageChoice.MoveToNextStage();
                });
            }
            else
            {
                StatusText.text = "돈이 부족해";
            }
        }
    }
}
