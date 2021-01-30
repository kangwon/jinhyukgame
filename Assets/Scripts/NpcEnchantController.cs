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

    public Prefix NextPrefix 
    {
        get => (Prefix)Math.Min((int)(Weapon.prefix + 1), (int)Prefix.amazing);
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
    List<int> enchantWeaponMny = new List<int>(new int[] { 25, 100, 250, 500, 750, 1000, });

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

        Debug.Log($"selectedEnchant.Index {selectedEnchant.Index}");

        Player player = GameState.Instance.player;
        var weaponList = player.GetWeaponList();
        weaponList[selectedEnchant.Index] = JsonDB.GetWeapon(selectedEnchant.NextWeaponId);
        player.SetWeaponList(weaponList);

        stageChoice.MoveToNextStage();
    }

    void OnClickWeaponCard(EnchantInfo enchantInfo)
    {
        this.selectedEnchant = enchantInfo;
        var weapon = selectedEnchant.Weapon;

        if (weapon.id == "bare_fist")
        {
            Debug.Log("맨주먹인데..");
            return;
        }

        beforeImg.GetComponent<Image>().sprite = weapon.weaponImg;
        afterImg.GetComponent<Image>().sprite = weapon.weaponImg;

        beforeCard.GetComponentInChildren<Text>().text = weapon.prefix.ToString();
        afterCard.GetComponentInChildren<Text>().text = selectedEnchant.NextPrefix.ToString();

        if (weapon.prefix == Prefix.amazing)
            enchantButton.GetComponentInChildren<Text>().text = $"강화 불가";
        else if (weapon.rank == Rank.legendary && weapon.prefix == Prefix.strong)
            enchantButton.GetComponentInChildren<Text>().text = $"{enchantWeaponMny[5]} Coin  확인";
        else
            enchantButton.GetComponentInChildren<Text>().text = $"{enchantWeaponMny[(int)weapon.rank]} Coin  확인";
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
                var enchangInfo = new EnchantInfo(weapon, i);
                WeaponCard.GetComponent<Button>().onClick.AddListener(() => OnClickWeaponCard(enchangInfo));
                WeaponCard.GetComponentInChildren<Text>().text = $"{weapon.statEffect.attack}";
                WeaponCards[i] = WeaponCard;
            }
            
            beforeCard.GetComponentInChildren<Text>().text = "선택된 카드 없음";
            afterCard.GetComponentInChildren<Text>().text = "선택된 카드 없음";
        }
    }
}
