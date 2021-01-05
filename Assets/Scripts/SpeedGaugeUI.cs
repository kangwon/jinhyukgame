using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedGaugeUI : MonoBehaviour //BattlePlayerAttackPanel의 child
{
    BattlePlayerAttackPanelController BPAPC;
    
    [HideInInspector]
    public RectTransform rectTran;
    [HideInInspector]
    public Image PlayerSpeedGaugeImage, MonsterSpeedGaugeImage;

    public const int GAUGE_SIZE = 200;
    private float SpeedGaugeWidth, SpeedGaugeHeight; //스피드게이지 UI의 넓이와 길이(높이)

    private float playerGaugeRatio;
    private float monsterGaugeRatio;

    void Awake() {

        BPAPC = this.transform.parent.GetComponent<BattlePlayerAttackPanelController>();

        rectTran = this.GetComponent<RectTransform>();

        PlayerSpeedGaugeImage = rectTran.transform.Find("PlayerGauge").GetComponent<Image>();
        MonsterSpeedGaugeImage = rectTran.transform.Find("MonsterGauge").GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpeedGaugeWidth = rectTran.rect.width;
        SpeedGaugeHeight = rectTran.rect.height;

        PlayerSpeedGaugeImage.transform.localPosition = new Vector3(0,SpeedGaugeHeight/2,0);
        MonsterSpeedGaugeImage.transform.localPosition = new Vector3(0,SpeedGaugeHeight/2,0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeedGaugeImage();
    }
    void UpdateSpeedGaugeImage() {
        
        playerGaugeRatio = BPAPC.playerGauge / GAUGE_SIZE;
        monsterGaugeRatio = BPAPC.monsterGauge / GAUGE_SIZE; //비율계산

        PlayerSpeedGaugeImage.transform.localPosition = new Vector3(0,SpeedGaugeHeight/2 - playerGaugeRatio * SpeedGaugeHeight,0);
        MonsterSpeedGaugeImage.transform.localPosition = new Vector3(0,SpeedGaugeHeight/2 - monsterGaugeRatio * SpeedGaugeHeight,0);
      
    }
}
