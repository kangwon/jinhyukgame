using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class StageChoice : MonoBehaviour
{
    CardType CurrentCardType;
    int currentStage;

    Vector3 PanelDisplayPosition = new Vector3(0, 100, 0);
    
    GameObject CardSelectPanel;
    GameObject NpcPanel;
    GameObject BuffPanel;
    GameObject ChestPanel;
    GameObject BattlePanel;

    Text StageText;

    Text CardText1;
    Text CardText2;
    Text CardText3;

    // Start is called before the first frame update
    void Start()
    {
        CardSelectPanel = GameObject.Find("CardSelectPanel");
        NpcPanel = GameObject.Find("NpcPanel");
        BuffPanel = GameObject.Find("BuffPanel");
        ChestPanel = GameObject.Find("ChestPanel");
        BattlePanel = GameObject.Find("BattlePlayerAttackPanel");

        StageText = GameObject.Find("Stage Text").GetComponent<Text>();

        CardText1 = GameObject.Find("Card1 Text").GetComponent<Text>();
        CardText2 = GameObject.Find("Card2 Text").GetComponent<Text>();
        CardText3 = GameObject.Find("Card3 Text").GetComponent<Text>();

        GameState.Instance.World = new World(1, "테스트 월드");
        currentStage = 0;
        MoveToNextStage();

        Debug.Log($"{GameObject.Find("CardSelectPanel")}");
        
        var stageCards = GameState.Instance.Stage.Cards;
        CardText1.text = stageCards[0].Type.ToString();
        CardText2.text = stageCards[1].Type.ToString();
        CardText3.text = stageCards[2].Type.ToString();
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
        var selectedCard = GameState.Instance.Stage.Cards[index];
        ActivatePannel(selectedCard.Type);
    }

    public void MoveToNextStage()
    {
        currentStage += 1;
        StageText.text = $"Stage {currentStage}";
        GameState.Instance.Stage = GameState.Instance.World.GetStage(currentStage);
        ActivatePannel(CardType.Undecided);
    }
    
    void ActivatePannel(CardType type)
    {
        CurrentCardType = type;
        DeactiveAllPanel();
        UpdateGamePanel();
    }

    void DeactiveAllPanel()
    {
        CardSelectPanel.SetActive(false);
        NpcPanel.SetActive(false);
        BuffPanel.SetActive(false);
        ChestPanel.SetActive(false);
        BattlePanel.SetActive(false);
    }

    public void UpdateGamePanel()
    {
        Debug.Log($"UpdateGamePanel: {CurrentCardType}");
        switch (CurrentCardType)
        {
            case CardType.Undecided:
                CardSelectPanel.SetActive(true);
                CardSelectPanel.transform.localPosition = PanelDisplayPosition;
                break;
            case CardType.Monster:
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
                break;
            case CardType.Npc:
                NpcPanel.SetActive(true);
                NpcPanel.transform.localPosition = PanelDisplayPosition;
                break;
        }
    }
}
