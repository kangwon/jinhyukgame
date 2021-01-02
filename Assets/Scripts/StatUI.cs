using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    //public Stat stat;
    public CharacterBase character;
    public Text ATK, DEF, HP, SPD;

    // Start is called before the first frame update
    void Start()
    {

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
        ATK.text = "공 : " + character.baseStat.attack;
        DEF.text = "방 : " + character.baseStat.defense;
        HP.text = "현재체력 : " + character.baseStat.nowHp;
        SPD.text = "속 : " + character.baseStat.speed;
        Debug.Log("와우시발");
    }
}
