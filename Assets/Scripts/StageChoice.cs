using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class StageChoice : MonoBehaviour
{
    public bool BossClear = false;
    CardType CurrentCardType;

    List<int> WorldBossStage = new List<int>() { 0, 15, 15, 20, 20, 25, 25, 30, 30, 50 };

    void StageCheck()
    {
        if (BossClear)
        {
            GameState.Instance.WorldNum += 1;
            GameState.Instance.StageNum = 1;
            BossClear = false;
        }
        else
        {
            GameState.Instance.StageNum += 1;
        }
    }

    public GameObject UserScreen;

    Vector3 PanelDisplayPosition = new Vector3(0, 100, 0);
    public GameObject CardSelectPanel;
    public GameObject NpcPanel;
    public GameObject BuffPanel;
    public GameObject ChestPanel;
    public GameObject BattlePanel;
    public System.Random ran = new System.Random();

    List<StageCard> CardStates = new List<StageCard>()
    {
        new StageCard(CardLocation.Left, CardType.Buff), 
        new StageCard(CardLocation.Middle, CardType.Npc), 
        new StageCard(CardLocation.Right, CardType.Chest),

        new StageCard(CardLocation.Left, CardType.Undecided), 
        new StageCard(CardLocation.Middle, CardType.Undecided), 
        new StageCard(CardLocation.Right, CardType.Undecided)
    };

    public Text card1_text;
    public Text card2_text;
    public Text card3_text;    

    // Start is called before the first frame update
    void Start()
    {
        CurrentCardType = CardType.Undecided;
        DeactiveAllPanel();
        UpdateGamePanel();

        // move forward
        // TODO: Not yet completed
        CardStates[0] = CardStates[3];
        CardStates[1] = CardStates[4];
        CardStates[2] = CardStates[5];
        CardStates[3] = new StageCard(CardLocation.Left, CardType.Undecided);
        CardStates[4] = new StageCard(CardLocation.Middle, CardType.Undecided);
        CardStates[5] = new StageCard(CardLocation.Right, CardType.Undecided);

        int typeInt;

        for (int i = 0; i < 6; i++)
        {
            if (CardStates[i].Type == CardType.Undecided)
            {
                if (GameState.Instance.StageNum < WorldBossStage[GameState.Instance.WorldNum])
                {
                    typeInt = ran.Next(1, 101);
                    if (typeInt <= 70) CardStates[i].Type = CardType.Monster;
                    else if (typeInt <= 75) CardStates[i].Type = CardType.Chest;
                    else if (typeInt <= 80) CardStates[i].Type = CardType.Buff;
                    else if (typeInt <= 90) CardStates[i].Type = CardType.Random;
                    else if (typeInt <= 100) CardStates[i].Type = CardType.Npc;
                }
                else if (CardStates[i].Location == CardLocation.Middle)
                {
                    CardStates[i].Type = CardType.Boss;
                }
                else
                {
                    typeInt = ran.Next(1, 101);
                    if (typeInt <= 73) CardStates[i].Type = CardType.Monster;
                    else if (typeInt <= 80) CardStates[i].Type = CardType.Chest;
                    else if (typeInt <= 87) CardStates[i].Type = CardType.Buff;
                    else if (typeInt <= 100) CardStates[i].Type = CardType.Random;
                }
            }
        }

         card1_text.text = CardStates[0].Type.ToString();
         card2_text.text = CardStates[1].Type.ToString();
         card3_text.text = CardStates[2].Type.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickCard(int index)
    {
        ActivatePannel(CardStates[index].Type);
    }
    
    public void ActivatePannel(CardType type)
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
