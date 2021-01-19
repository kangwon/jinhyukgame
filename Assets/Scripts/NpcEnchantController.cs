using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEditor.Build.Reporting;
using UnityEditorInternal;
using System;

public class NpcEnchantController : MonoBehaviour
{
    // Start is called before the first frame update

    List<int> enchantWeaponMny = new List<int>(new int[] { 25, 100, 250, 500, 750, 1000, });
    List<int> enchantEquipmentMny = new List<int>(new int[] { 50, 175, 400, 750, 1000, 1500, });
    List<string> prefixlist = new List<string>(new string[] { "broken", "weak", "normal", "strong", "amazing" });
    List<string> typelist = new List<string>(new string[] { "weapon", "helmet", "armor", "shoes" });
    int minus_mny;

    List<Weapon> weaponList;
    Equipment pHelmet, pArmor, pShoes;
    GameObject[] I_card = new GameObject[10];
    GameObject helcard;
    GameObject armcard;
    GameObject shocard;

    GameObject beforeCard;
    GameObject beforeImg;
    GameObject afterCard;
    GameObject afterImg;
    GameObject enchantButton;
    public GameObject EnchantPanel;
    int card_Index = -1;
    bool EnchantOn = true;
    public StageChoice stageChoice;

    string beforeid;
    string checkprefix;
    string checktype;
    string checkrank;
    int changeprefix;

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
        helcard.transform.GetChild(0).GetComponent<Text>().text = $"{pHelmet.statEffect.defense}";
        armcard.transform.GetChild(0).GetComponent<Text>().text = $"{pArmor.statEffect.maxHp}";
        shocard.transform.GetChild(0).GetComponent<Text>().text = $"{pShoes.statEffect.speed}";
        beforeCard.transform.GetChild(0).GetComponent<Text>().text = $"선택된 카드 없음";
        afterCard.transform.GetChild(0).GetComponent<Text>().text = $"선택된 카드 없음";
    }
    void CoinButton()
    {  
        if (checkprefix == "4")
        {
            Debug.Log("최대 강화 수식어입니다.");
        }
        else
        {
            Player player = GameState.Instance.player;

            if (checktype == "weapon")
            {
                if (changeprefix / 10 == 0)
                    weaponList[card_Index] = JsonDB.GetWeapon($"weapon_00{changeprefix}");
                else if (changeprefix / 100 == 0)
                    weaponList[card_Index] = JsonDB.GetWeapon($"weapon_0{changeprefix}");
                else
                    weaponList[card_Index] = JsonDB.GetWeapon($"weapon_{changeprefix}");
                player.SetWeaponList(weaponList);
            }
            else if (checktype == "helmet")
            {
                if (changeprefix / 10 == 0)
                    pHelmet = JsonDB.GetEquipment($"helmet{changeprefix / 100}_0{changeprefix % 100}");
                else
                    pHelmet = JsonDB.GetEquipment($"helmet{changeprefix / 100}_{changeprefix % 100}");

                player.SetEquipment(pHelmet);
            }
            else if (checktype == "armor")
            {
                if (changeprefix / 10 == 0)
                    pArmor = JsonDB.GetEquipment($"armor{changeprefix / 100}_0{changeprefix % 100}");
                else
                    pArmor = JsonDB.GetEquipment($"armor{changeprefix / 100}_{changeprefix % 100}");
                player.SetEquipment(pArmor);
            }
            else if (checktype == "shoes")
            {
                if (changeprefix / 10 == 0)
                    pShoes = JsonDB.GetEquipment($"shoes{changeprefix / 100}_0{changeprefix % 100}");
                else
                    pShoes = JsonDB.GetEquipment($"shoes{changeprefix / 100}_{changeprefix % 100}");
                player.SetEquipment(pShoes);
            }
            else
            {
                return;
            }
            // 장비쪽 체크



            // update 반복 방지 및 스테이지 넘김
            EnchantOn = true;
            stageChoice.MoveToNextStage();
        }
    }

    void OnClickCard(int c_index)
    {
        card_Index = c_index;


        if (c_index < weaponList.Count)
        {
            if (weaponList.ElementAt(card_Index).id == "bare_fist")
            {
                Debug.Log("맨주먹인데..");
                return;
            }

            beforeCard.transform.GetChild(0).GetComponent<Text>().text = $"{weaponList.ElementAt(c_index).prefix}";

            beforeImg.GetComponent<Image>().sprite = weaponList.ElementAt(card_Index).weaponImg;
            afterImg.GetComponent<Image>().sprite = weaponList.ElementAt(card_Index).weaponImg;
            beforeid = $"{weaponList.ElementAt(card_Index).id}";
            checkprefix = beforeid.Substring(9);
            checktype = "weapon";
            changeprefix = Convert.ToInt32(beforeid.Substring(7)) + 1;

            afterCard.transform.GetChild(0).GetComponent<Text>().text = prefixlist[changeprefix%10];

            if ($"{weaponList.ElementAt(c_index).rank}" == "common")
            {
                enchantButton.transform.GetChild(0).GetComponent<Text>().text = $"{enchantWeaponMny[0]} Coin  확인";
                minus_mny = enchantWeaponMny[0];
            }
            else if ($"{weaponList.ElementAt(c_index).rank}" == "uncommon")
            {
                enchantButton.transform.GetChild(0).GetComponent<Text>().text = $"{enchantWeaponMny[1]} Coin  확인";
                minus_mny = enchantWeaponMny[1];
            }
            else if ($"{weaponList.ElementAt(c_index).rank}" == "rare")
            {
                enchantButton.transform.GetChild(0).GetComponent<Text>().text = $"{enchantWeaponMny[2]} Coin  확인";
                minus_mny = enchantWeaponMny[2];
            }
            else if ($"{weaponList.ElementAt(c_index).rank}" == "unique")
            {
                enchantButton.transform.GetChild(0).GetComponent<Text>().text = $"{enchantWeaponMny[3]} Coin  확인";
                minus_mny = enchantWeaponMny[3];
            }
            else if ($"{weaponList.ElementAt(c_index).rank}" == "legendary")
            {
                if ($"{weaponList.ElementAt(c_index).prefix}" != "strong")
                {
                    enchantButton.transform.GetChild(0).GetComponent<Text>().text = $"{enchantWeaponMny[4]} Coin  확인";
                    minus_mny = enchantWeaponMny[4];
                }
                else
                {
                    enchantButton.transform.GetChild(0).GetComponent<Text>().text = $"{enchantWeaponMny[5]} Coin  확인";
                    minus_mny = enchantWeaponMny[5];
                }
            }
        }
        else
        {
            beforeCard.transform.GetChild(0).GetComponent<Text>().text = $"클릭";
            afterCard.transform.GetChild(0).GetComponent<Text>().text = $"클릭";
        }
    }

    void OnClickCard2(string equipType)
    {
        // Debug.Log(equipType);

        checktype = equipType;


        if (equipType == "helmet" && pHelmet.id != "helmet")
        {
            beforeCard.transform.GetChild(0).GetComponent<Text>().text = $"{pHelmet.prefix}";
            checkrank = $"{pHelmet.rank}";
            checkprefix = $"{pHelmet.prefix}";
            beforeid = $"{pHelmet.id}";
        }
        else if (equipType == "armor" && pArmor.id != "armor")
        {
            beforeCard.transform.GetChild(0).GetComponent<Text>().text = $"{pArmor.prefix}";
            checkrank = $"{pArmor.rank}";
            checkprefix = $"{pArmor.prefix}";
            beforeid = $"{pArmor.id}";
        }
        else if (equipType == "shoes" && pShoes.id != "shoes")
        {
            beforeCard.transform.GetChild(0).GetComponent<Text>().text = $"{pShoes.prefix}";
            checkrank = $"{pShoes.rank}";
            checkprefix = $"{pShoes.prefix}";
            beforeid = $"{pShoes.id}";
        }
        else
        {
            Debug.Log("아직 착용한 게 없어요");
            return;
        }

        if (checkrank == "common")
        {
            enchantButton.transform.GetChild(0).GetComponent<Text>().text = $"{enchantEquipmentMny[0]} Coin  확인";
            minus_mny = enchantEquipmentMny[0];
        }
        else if (checkrank == "uncommon")
        {
            enchantButton.transform.GetChild(0).GetComponent<Text>().text = $"{enchantEquipmentMny[1]} Coin  확인";
            minus_mny = enchantEquipmentMny[1];
        }
        else if (checkrank == "rare")
        {
            enchantButton.transform.GetChild(0).GetComponent<Text>().text = $"{enchantEquipmentMny[2]} Coin  확인";
            minus_mny = enchantEquipmentMny[2];
        }
        else if (checkrank == "unique")
        {
            enchantButton.transform.GetChild(0).GetComponent<Text>().text = $"{enchantEquipmentMny[3]} Coin  확인";
            minus_mny = enchantEquipmentMny[3];
        }
        else
        {
            if (checkprefix != "strong")
            {
                enchantButton.transform.GetChild(0).GetComponent<Text>().text = $"{enchantEquipmentMny[4]} Coin  확인";
                minus_mny = enchantEquipmentMny[4];
            }
            else
            {
                enchantButton.transform.GetChild(0).GetComponent<Text>().text = $"{enchantEquipmentMny[5]} Coin  확인";
                minus_mny = enchantEquipmentMny[5];
            }
        }

        // Debug.Log($"{beforeid}");

        changeprefix = Convert.ToInt32(beforeid.Substring(6, 1))*100 + Convert.ToInt32(beforeid.Substring(8)) + 1;
        afterCard.transform.GetChild(0).GetComponent<Text>().text = prefixlist[changeprefix % 10];
        // Debug.Log($"{changeprefix}");
    }

    void Start()
    {
        EnchantPanel = GameObject.Find("NpcPanel_Enchanter");
        beforeCard = GameObject.Find("NpcPanel_Enchanter/b_weapon");
        beforeImg = GameObject.Find("NpcPanel_Enchanter/b_weapon/before_img");
        afterCard = GameObject.Find("NpcPanel_Enchanter/a_weapon");
        afterImg = GameObject.Find("NpcPanel_Enchanter/a_weapon/after_img");
        weaponList = GameState.Instance.player?.GetWeaponList();
        pArmor = GameState.Instance.player?.GetArmor();
        pHelmet = GameState.Instance.player?.GetHelmet();
        pShoes = GameState.Instance.player?.GetShoes();
        enchantButton = GameObject.Find("NpcPanel_Enchanter/EnchantButton");
        enchantButton.GetComponent<Button>().onClick.AddListener(() =>
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
        helcard = GameObject.Find("WeaponSelect/Helmet");
        helcard.GetComponent<Button>().onClick.AddListener(() =>
        {
            OnClickCard2("helmet");
        });
        armcard = GameObject.Find("WeaponSelect/Armor");
        armcard.GetComponent<Button>().onClick.AddListener(() =>
        {
            OnClickCard2("armor");
        });
        shocard = GameObject.Find("WeaponSelect/Shoes");
        shocard.GetComponent<Button>().onClick.AddListener(() =>
        {
            OnClickCard2("shoes");
        });
    }

    // Update is called once per frame
    void Update()
    {
        // update 반복 방지
        if (EnchantOn)
        {
            weaponList = GameState.Instance.player.GetWeaponList();
            pArmor = GameState.Instance.player.GetArmor();
            pHelmet = GameState.Instance.player.GetHelmet();
            pShoes = GameState.Instance.player.GetShoes();
            PrintCard();
            EnchantOn = false;
        }
    }
}
