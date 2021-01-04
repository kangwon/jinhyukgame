using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
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
        ATK.text = "공 : " + character.GetStat().attack;
        DEF.text = "방 : " + character.GetStat().defense;
        HP.text = "현재체력 : " + character.hp;
        SPD.text = "속 : " + character.GetStat().speed;
    }
}
