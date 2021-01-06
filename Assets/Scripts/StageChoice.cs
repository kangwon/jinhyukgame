using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class StageChoice : MonoBehaviour
{
    StageCard selectedCard;
    int currentStage;

    Vector3 PanelDisplayPosition = new Vector3(0, 100, 0);
    
    GameObject CardSelectPanel;
    GameObject NpcPanel;
    GameObject NpcPanel_Merchant;
    GameObject NpcPanel_Healer;
    GameObject NpcPanel_Enchanter;
    GameObject BuffPanel;
    GameObject ChestPanel;
    GameObject BattlePanel;
    GameObject RandomPanel;

    Text StageText;

    Text CardText1;
    Text CardText2;
    Text CardText3;

    // Start is called before the first frame update
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
        
        StageText = GameObject.Find("Stage Text").GetComponent<Text>();

        CardText1 = GameObject.Find("Card1 Text").GetComponent<Text>();
        CardText2 = GameObject.Find("Card2 Text").GetComponent<Text>();
        CardText3 = GameObject.Find("Card3 Text").GetComponent<Text>();

        GameState.Instance.World = new World(1, "테스트 월드");
        currentStage = 0;
        MoveToNextStage();
    }

    // Update is called once per frame
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
        currentStage += 1;
        StageText.text = $"Stage {currentStage}";
        GameState.Instance.Stage = GameState.Instance.World.GetStage(currentStage);
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
    }

    public void UpdateGamePanel()
    {
        switch (selectedCard?.Type ?? null)
        {
            case null:
                CardSelectPanel.SetActive(true);
                CardSelectPanel.transform.localPosition = PanelDisplayPosition;
                break;
            case CardType.Monster:
                var controller = BattlePanel.GetComponent<BattlePlayerAttackPanelController>();
                controller.MonsterCard = (selectedCard as MonsterCard);
                BattlePanel.SetActive(true);
                BattlePanel.transform.localPosition = PanelDisplayPosition;
                break;
            case CardType.Chest:
                ChestPanel.SetActive(true);
                ChestPanel.transform.localPosition = PanelDisplayPosition;
                break;
            case CardType.Buff:
                BuffPanel.SetActive(true);
                BuffPanel.transform.localPosition = PanelDisplayPosition;
                break;
            case CardType.Random:
                RandomPanel.SetActive(true);
                RandomPanel.transform.localPosition = PanelDisplayPosition;
                break;
            case CardType.Npc:
                NpcPanel.SetActive(true);
                NpcPanel.transform.localPosition = PanelDisplayPosition;
                NpcPanel_Merchant.transform.localPosition = PanelDisplayPosition;
                NpcPanel_Healer.transform.localPosition = PanelDisplayPosition;
                NpcPanel_Enchanter.transform.localPosition = PanelDisplayPosition;
                break;
        }
    }
}
