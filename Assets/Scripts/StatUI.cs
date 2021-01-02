using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    public Stat stat;
    public CharacterBase character;
    public Text ATK, DEF, HP, SPD;

    // Start is called before the first frame update
    void Start()
    {
        // ATK = this.transform.Find("ATK").Text;
        // DEF = this.transform.Find("DEF").Text;
        // HP = this.transform.Find("HP").Text;
        // SPD = this.transform.Find("SPD").Text;
    }

    // Update is called once per frame
    void Update()
    {
        if(character != null) {
            UpdateText();
        }
    }

    void UpdateText() {
        //ATK.text = "공 : " + character.GetStat().attack; //character stat과 연동
        ATK.text = "공 : " + stat.attack;
        DEF.text = "방 : " + stat.defense;
        HP.text = "현재체력 : " + character.nowHp;
        SPD.text = "속 : " + stat.speed;
    }
}
