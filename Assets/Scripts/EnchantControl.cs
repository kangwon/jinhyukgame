using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEditor.Build.Reporting;
using UnityEditorInternal;
using System;

public class EnchantControl : MonoBehaviour
{
    // Start is called before the first frame update

    List<int> EnchantMny = new List<int>(new int[] { 50, 175, 400, 750, 1000, 1500, });
    int minus_mny;

    List<Weapon> weaponList;
    GameObject[] I_card = new GameObject[10];
    GameObject beforeCard;
    GameObject afterCard;
    GameObject okButton;
    public GameObject EnchantPanel;
    int card_Index = 0;
    bool EnchantOn = true;
    public StageChoice stageChoice;

    void PrintCard()
    {
        for (int i = 0; i < 10; i++)
        {
            int temp = i;
            if (temp < weaponList.Count)
                I_card[temp].transform.GetChild(0).GetComponent<Text>().text = $"{weaponList.ElementAt(temp).statEffect.attack}";
            else
                I_card[temp].transform.GetChild(0).GetComponent<Text>().text = $"카드 없음";
        }
        beforeCard.transform.GetChild(0).GetComponent<Text>().text = $"선택된 카드 없음";
    }
    void CoinButton()
    {
        string beforeid = $"{weaponList.ElementAt(card_Index).id}";
        string checkprefix = beforeid.Substring(9);
        int changeid = Convert.ToInt32(beforeid.Substring(7)) + 1;

        if (checkprefix == "4")
        {
            Debug.Log("최대 강화 수식어입니다.");
        }
        else
        {
            Player player = GameState.Instance.player;

            Debug.Log($"index {card_Index}");
            weaponList[card_Index] = JsonDB.GetWeapon($"weapon_{changeid}");
            player.SetWeaponList(weaponList);

            Debug.Log($"count {weaponList.Count}");

            /*
            player.SetEquipment(JsonDB.GetWeapon($"weapon_{changeid}"));
            weaponList = player.GetWeaponList();
            Debug.Log($"nonremove : {weaponList.Count}");
            weaponList.RemoveAt(card_Index);
            Debug.Log($"remove : {weaponList.Count}");
            var sortList = weaponList.OrderBy(x => x.id).ToList();
            player.SetWeaponList(sortList);
            */
            /*
            Debug.Log($"weapon_{checkprefix}");
            Debug.Log($"weapon_{changeid}");
            Weapon nWeapon = JsonDB.GetWeapon($"weapon_{changeid}");
            weaponList[card_Index] = nWeapon;
            var sortList = weaponList.OrderBy(x => x.id).ToList();
            player.SetWeaponList(sortList);
            */
            EnchantOn = true;
            stageChoice.MoveToNextStage();
        }
    }

    void OnClickCard(int c_index)
    {
        card_Index = c_index;


        if (c_index < weaponList.Count)
        {
            beforeCard.transform.GetChild(0).GetComponent<Text>().text = $"{weaponList.ElementAt(c_index).statEffect.attack}";

            if ($"{weaponList.ElementAt(c_index).rank}" == "common")
            {
                okButton.transform.GetChild(0).GetComponent<Text>().text = $"{EnchantMny[0]} Coin  확인";
                afterCard.transform.GetChild(0).GetComponent<Text>().text = $"{EnchantMny[0]} Coin  확인";
                minus_mny = EnchantMny[0];
            }
            else if ($"{weaponList.ElementAt(c_index).rank}" == "uncommon")
            {
                okButton.transform.GetChild(0).GetComponent<Text>().text = $"{EnchantMny[1]} Coin  확인";
                afterCard.transform.GetChild(0).GetComponent<Text>().text = $"{EnchantMny[1]} Coin  확인";
                minus_mny = EnchantMny[1];
            }
            else if ($"{weaponList.ElementAt(c_index).rank}" == "rare")
            {
                okButton.transform.GetChild(0).GetComponent<Text>().text = $"{EnchantMny[2]} Coin  확인";
                afterCard.transform.GetChild(0).GetComponent<Text>().text = $"{EnchantMny[2]} Coin  확인";
                minus_mny = EnchantMny[2];
            }
            else if ($"{weaponList.ElementAt(c_index).rank}" == "unique")
            {
                okButton.transform.GetChild(0).GetComponent<Text>().text = $"{EnchantMny[3]} Coin  확인";
                afterCard.transform.GetChild(0).GetComponent<Text>().text = $"{EnchantMny[3]} Coin  확인";
                minus_mny = EnchantMny[3];
            }
            else
            {
                if ($"{weaponList.ElementAt(c_index).prefix}" != "strong")
                {
                    okButton.transform.GetChild(0).GetComponent<Text>().text = $"{EnchantMny[4]} Coin  확인";
                    afterCard.transform.GetChild(0).GetComponent<Text>().text = $"{EnchantMny[4]} Coin  확인";
                    minus_mny = EnchantMny[4];
                }
                else
                {
                    okButton.transform.GetChild(0).GetComponent<Text>().text = $"{EnchantMny[5]} Coin  확인";
                    afterCard.transform.GetChild(0).GetComponent<Text>().text = $"{EnchantMny[5]} Coin  확인";
                    minus_mny = EnchantMny[5];
                }
            }


        }
        else
        {
            beforeCard.transform.GetChild(0).GetComponent<Text>().text = $"클릭";
            afterCard.transform.GetChild(0).GetComponent<Text>().text = $"클릭";
        }
    }

    void Start()
    {
        EnchantPanel = GameObject.Find("NpcPanel_Enchanter");
        beforeCard = GameObject.Find("NpcPanel_Enchanter/b_weapon");
        afterCard = GameObject.Find("NpcPanel_Enchanter/a_weapon");
        weaponList = GameState.Instance.player?.GetWeaponList();
        okButton = GameObject.Find("NpcPanel_Enchanter/OkButton");
        okButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            CoinButton();
        });
        for (int i = 0; i < 10; i++) 
        {
            int temp = i;
            I_card[temp] = GameObject.Find($"WeaponSelect/Weapon{temp + 1}");
            I_card[temp].GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClickCard(temp);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EnchantOn)
        {
            weaponList = GameState.Instance.player.GetWeaponList();
            PrintCard();
            EnchantOn = false;
        }
    }
}
