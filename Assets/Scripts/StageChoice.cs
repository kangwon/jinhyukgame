using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class StageChoice : MonoBehaviour
{
    StageCard selectedCard;

    public static Vector3 PanelDisplayPosition = new Vector3(0, 100, 0);
    
    GameObject CardSelectPanel;
    GameObject NpcPanel;
    GameObject NpcPanel_Merchant;
    GameObject NpcPanel_Healer;
    GameObject NpcPanel_Enchanter;
    GameObject BuffPanel;
    GameObject ChestPanel;
    GameObject BattlePanel;
    GameObject RandomPanel;
    GameObject WeaponPopupView;
    GameObject WeaponChangePanel;

    Text WorldText;
    Text StageText;
    Text WorldText;

    Text CardText1;
    Text CardText2;
    Text CardText3;

    void Start()
    {
        CardSelectPanel = GameObject.Find("CardSelectPanel");
        NpcPanel = GameObject.Find("NpcPanel");
        NpcPanel_Merchant = GameObject.Find("NpcPanel_Merchant");
        NpcPanel_Healer = GameObject.Find("NpcPanel_Healer");
        NpcPanel_Enchanter = GameObject.Find("NpcPanel_Enchanter");
        BuffPanel = GameObject.Find("BuffPanel");
        ChestPanel = GameObject.Find("ChestPanel");
        BattlePanel = GameObject.Find("BattlePlayerAttackPanel");
        RandomPanel = GameObject.Find("RandomPanel");
        WeaponChangePanel = GameObject.Find("WeaponChangePanel");
        WeaponPopupView = GameObject.Find("WeaponPopupView/WeaponPopupScreen");

        WorldText = GameObject.Find("World Text").GetComponent<Text>();
        StageText = GameObject.Find("Stage Text").GetComponent<Text>();
        WorldText = GameObject.Find("World Txt").GetComponent<Text>();

        CardText1 = GameObject.Find("Card1 Text").GetComponent<Text>();
        CardText2 = GameObject.Find("Card2 Text").GetComponent<Text>();
        CardText3 = GameObject.Find("Card3 Text").GetComponent<Text>();

        GameState.Instance.ResetPlayer();
        GameState.Instance.StartWorld(GameConstant.InitialWorldNumber, "테스트 월드");
        ActivatePannel();
    }

    void Update()
    {
        var stageCards = GameState.Instance.Stage.Cards;
        CardText1.text = stageCards[0].Type.ToString();
        CardText2.text = stageCards[1].Type.ToString();
        CardText3.text = stageCards[2].Type.ToString();
    }

    public void OnClickCard(int index)
    {
        selectedCard = GameState.Instance.Stage.Cards[index];
        ActivatePannel();
    }

    public void MoveToNextStage()
    {
        selectedCard = null;
        GameState.Instance.MoveToNextStage();
        GameState.Instance.player.Heal((int)(GameState.Instance.player.GetStat().maxHp * GameState.Instance.player.GetStat().stageHpDrain));
        GameState.Instance.player.HpOver();
        ActivatePannel();
    }
    
    public void MoveToNextWorld()
    {
        selectedCard = null;
        GameState.Instance.StartWorld(GameState.Instance._world.Number+1,"테스트 월드");
        GameState.Instance.player.Heal(GameState.Instance.player.GetStat().maxHp);
        GameState.Instance.player.HpOver();
        StageText.text = $"Stage {GameState.Instance.Stage.Number}";
        WorldText.text = $"W{GameState.Instance.World.Number}";
        ActivatePannel();
    }


    void ActivatePannel()
    {
        DeactiveAllPanel();
        UpdateGamePanel();
    }

    void DeactiveAllPanel()
    {
        CardSelectPanel.SetActive(false);
        NpcPanel.SetActive(false);
        NpcPanel_Merchant.SetActive(false);
        NpcPanel_Healer.SetActive(false);
        NpcPanel_Enchanter.SetActive(false);
        BuffPanel.SetActive(false);
        ChestPanel.SetActive(false);
        BattlePanel.SetActive(false);
        RandomPanel.SetActive(false);
        WeaponPopupView.SetActive(false);
    }

    public void UpdateGamePanel()
    {
        WorldText.text = $"W{GameState.Instance.World.Number}";
        StageText.text = $"Stage {GameState.Instance.Stage.Number}";
        
        switch (selectedCard?.Type ?? null)
        {
            case null:
                CardSelectPanel.SetActive(true);
                CardSelectPanel.transform.localPosition = PanelDisplayPosition;
                break;
            case CardType.Monster:
            case CardType.Boss:
                var battleController = BattlePanel.GetComponent<BattleController>();
                var battlePlayerAttackPanelController = BattlePanel.GetComponent<BattlePlayerAttackPanelController>();
                battleController.MonsterCard = (selectedCard as MonsterCard);
                battlePlayerAttackPanelController.MonsterCard = (selectedCard as MonsterCard);
                BattlePanel.SetActive(true);
                BattlePanel.transform.localPosition = PanelDisplayPosition;
                break;
            case CardType.Chest:
                var chestController = ChestPanel.GetComponent<ChestPanelController>();
                chestController.ChestCard = (selectedCard as ChestCard);
                ChestPanel.SetActive(true);
                ChestPanel.transform.localPosition = PanelDisplayPosition;
                break;
            case CardType.Buff:
                var buffController = BuffPanel.GetComponent<BuffPanelController>();
                buffController.BuffCard = (selectedCard as BuffCard);
                BuffPanel.SetActive(true);
                BuffPanel.transform.localPosition = PanelDisplayPosition;
                break;
            case CardType.Random:
                var RandomController = RandomPanel.GetComponent<RandomPanelController>();
                RandomController.RandomCard = (selectedCard as RandomCard);
                RandomPanel.SetActive(true);
                RandomPanel.transform.localPosition = PanelDisplayPosition;
                break;
            case CardType.Npc:
                var NpcController = NpcPanel.GetComponent<NpcPanelController>();
                NpcController.NpcCard = (selectedCard as NpcCard);
                NpcPanel.SetActive(true);
                NpcPanel.transform.localPosition = PanelDisplayPosition;
                NpcPanel_Merchant.transform.localPosition = PanelDisplayPosition;
                NpcPanel_Healer.transform.localPosition = PanelDisplayPosition;
                NpcPanel_Enchanter.transform.localPosition = PanelDisplayPosition;
                break;
        }
    }
}
