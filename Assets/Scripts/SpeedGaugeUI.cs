using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpeedGaugeUI : MonoBehaviour
{
    public CombatController cmbt;
    public Image PlayerSpeedGaugeImage, MonsterSpeedGaugeImage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeedGaugeImage();
    }
    void UpdateSpeedGaugeImage() {
        float playerGaugeRatio = cmbt.getPlayerSpeedGaugeVal() / 1000.0f;
        PlayerSpeedGaugeImage.transform.localPosition = new Vector3((playerGaugeRatio * -400) + 200f,0f,0f);

        float monsterGaugeRatio = cmbt.getMonsterSpeedGaugeVal() / 1000.0f;
        MonsterSpeedGaugeImage.transform.localPosition = new Vector3((monsterGaugeRatio * -400) + 200f,0f,0f);
      
    }
}
