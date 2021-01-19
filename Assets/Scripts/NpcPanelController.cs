using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcPanelController : MonoBehaviour
{
    GameObject NpcPanel;
    GameObject NpcPanel_Merchant;
    GameObject NpcPanel_Healer;
    GameObject NpcPanel_Enchanter;

    public NpcCard NpcCard;

    void Start()
    {
        NpcPanel = GameObject.Find("NpcPanel");
        NpcPanel_Merchant = GameObject.Find("NpcPanel_Merchant");
        NpcPanel_Healer = GameObject.Find("NpcPanel_Healer");
        NpcPanel_Enchanter = GameObject.Find("NpcPanel_Enchanter");

        Button button1 = GameObject.Find($"NpcButton1").GetComponent<Button>();
        button1.onClick.AddListener(() => {
            var merchantContoller = NpcPanel_Merchant.GetComponent<NpcMerchantController>();
            merchantContoller.EquipmentsOnSale = this.NpcCard.EquipmentsOnSale;

            NpcPanel.SetActive(false);
            NpcPanel_Merchant.SetActive(true);
        });

        Button button2 = GameObject.Find($"NpcButton2").GetComponent<Button>();
        button2.onClick.AddListener(() => {
            NpcPanel.SetActive(false);
            NpcPanel_Healer.SetActive(true);
        });

        Button button3 = GameObject.Find($"NpcButton3").GetComponent<Button>();
        button3.onClick.AddListener(() => {
            NpcPanel.SetActive(false);
            NpcPanel_Enchanter.SetActive(true);
        });
    }

    void Update()
    {
        
    }
}
