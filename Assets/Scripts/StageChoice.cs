using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class StageChoice : MonoBehaviour
{
    CardType CurrentCardType;
    int currentStage;

    public GameObject UserScreen;

    Vector3 PanelDisplayPosition = new Vector3(0, 100, 0);
    public GameObject CardSelectPanel;
    public GameObject NpcPanel;
    public GameObject BuffPanel;
    public GameObject ChestPanel;
    public GameObject BattlePanel;

    Text StageText { get => GameObject.Find("Stage Text").GetComponent<Text>(); }

    public Text card1_text;
    public Text card2_text;
    public Text card3_text;    

    // Start is called before the first frame update
    void Start()
    {
        GameState.Instance.World = new World(1, "테스트 월드");
        currentStage = 0;
        MoveToNextStage();
        
        var stageCards = GameState.Instance.Stage.Cards;
        card1_text.text = stageCards[0].Type.ToString();
        card2_text.text = stageCards[1].Type.ToString();
        card3_text.text = stageCards[2].Type.ToString();
    }

    // Update is called once per frame
    void Update()
    {

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
