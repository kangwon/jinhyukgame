using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.Build.Reporting;
using UnityEditorInternal;

readonly struct EnchantInfo
{
    public Weapon Weapon { get; }
    public int Index { get; }

    public int Price { get => GameConstant.WeaponEnchantPrice[Weapon.rank][Weapon.prefix]; }
    public Prefix NextPrefix 
    {
        get
        {
            if (Weapon.prefix == Prefix.none)
                return Prefix.none;
            else
                return (Prefix)Math.Min((int)(Weapon.prefix + 1), (int)Prefix.amazing);
        }
    }
    public string NextWeaponId
    {
        get => $"{Weapon.id.Substring(0, 9)}{(int)NextPrefix}";
    }

    public EnchantInfo(Weapon weapon, int index)
    {
        Weapon = weapon;
        Index = index;
    }
}

public class NpcEnchantController : MonoBehaviour
{
    GameObject[] WeaponCards = new GameObject[10];

    GameObject beforeCard;
    GameObject beforeImg;
    GameObject afterCard;
    GameObject afterImg;
    GameObject enchantButton;
    GameObject EnchantPanel;
    public StageChoice stageChoice;

    EnchantInfo selectedEnchant;

    void CoinButton()
    {  
        if (selectedEnchant.Weapon.prefix == Prefix.amazing)
        {
            Debug.Log("최대 강화 수식어입니다.");
            return;
        }

        Player player = GameState.Instance.player;
        if (player.Pay(selectedEnchant.Price))
        {
            var weaponList = player.GetWeaponList();
            weaponList[selectedEnchant.Index] = JsonDB.GetWeapon(selectedEnchant.NextWeaponId);
            player.SetWeaponList(weaponList);
            
            stageChoice.MoveToNextStage();
        }
        else
        {
            Debug.Log("강화 비용 부족");
        }
    }

    void OnClickWeaponCard(EnchantInfo selectedEnchant)
    {
        this.selectedEnchant = selectedEnchant;
        var weapon = selectedEnchant.Weapon;

        beforeImg.GetComponent<Image>().sprite = weapon.weaponImg;
        afterImg.GetComponent<Image>().sprite = weapon.weaponImg;

        beforeCard.GetComponentInChildren<Text>().text = weapon.prefix.ToString();
        afterCard.GetComponentInChildren<Text>().text = selectedEnchant.NextPrefix.ToString();

        if (weapon.id == "bare_fist" || weapon.prefix == Prefix.amazing)
            enchantButton.GetComponentInChildren<Text>().text = $"강화 불가";
        else
            enchantButton.GetComponentInChildren<Text>().text = $"{selectedEnchant.Price} Coin  확인";
    }

    void Start()
    {
        EnchantPanel = GameObject.Find("NpcPanel_Enchanter");
        beforeCard = GameObject.Find("NpcPanel_Enchanter/b_weapon");
        beforeImg = GameObject.Find("NpcPanel_Enchanter/b_weapon/before_img");
        afterCard = GameObject.Find("NpcPanel_Enchanter/a_weapon");
        afterImg = GameObject.Find("NpcPanel_Enchanter/a_weapon/after_img");
        enchantButton = GameObject.Find("NpcPanel_Enchanter/EnchantButton");
        enchantButton.GetComponent<Button>().onClick.AddListener(CoinButton);
    }

    void OnEnable()
    {
        var weaponList = GameState.Instance.player?.GetWeaponList();
        if (weaponList != null)
        {
            for (int i = 0; i < WeaponCards.Length; i++)
            {
                var WeaponCard = GameObject.Find($"WeaponSelect/Weapon{i + 1}");
                var weapon = weaponList[i];
                var enchant = new EnchantInfo(weapon, i);
                WeaponCard.GetComponent<Button>().onClick.AddListener(() => OnClickWeaponCard(enchant));
                WeaponCard.GetComponentInChildren<Text>().text = $"{weapon.statEffect.attack}";
                WeaponCards[i] = WeaponCard;
            }
        }
        if ((beforeCard != null) && (afterCard != null))
        {
            beforeCard.GetComponentInChildren<Text>().text = "선택된 카드 없음";
            afterCard.GetComponentInChildren<Text>().text = "선택된 카드 없음";
        }
    }
}
