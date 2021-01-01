using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// location 0 - 좌 / 1 - 중 / 2 - 우
public enum CardLocation
{
    Left, Middle, Right,
}

// typeNum 0 - 미정 / 1 - 몬스터 / 2 - 보물
// 3 - 버프 / 4 - 이벤트 / 5 - 마을 / 6 - 보스
public enum CardType
{
    Undecided, Monster, Chest,
    Buff, Random, Npc, Boss
}

public class CardState
{
    public CardLocation Location;
    public CardType Type;

    public CardState(CardLocation location, CardType type)
    {
        Location = location;
        Type = type;
    }
}

public class StageChoice : MonoBehaviour
{
    public int StageNum = 0;
    public int WorldNum = 1;
    public bool BossClear = false;
    CardState CurrentState;

    List<int> WorldBossStage = new List<int>() { 0, 15, 15, 20, 20, 25, 25, 30, 30, 50 };

    void StageCheck()
    {
        if (BossClear)
        {
            WorldNum += 1;
            StageNum = 1;
            BossClear = false;
        }
        else
        {
            StageNum += 1;
        }
    }

    public GameObject UserScreen;

    Vector3 PanelDisplayPosition = new Vector3(0, 100, 0);
    public GameObject CardSelectPanel;
    public GameObject NpcPanel;

    public System.Random ran = new System.Random();

    List<CardState> CardStates = new List<CardState>()
    {
        new CardState(CardLocation.Left, CardType.Undecided), 
        new CardState(CardLocation.Middle, CardType.Undecided), 
        new CardState(CardLocation.Right, CardType.Undecided),

        new CardState(CardLocation.Left, CardType.Undecided), 
        new CardState(CardLocation.Middle, CardType.Undecided), 
        new CardState(CardLocation.Right, CardType.Undecided),
    };

    void DeactiveAllPanel()
    {
        NpcPanel.SetActive(true);
        CardSelectPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        DeactiveAllPanel();
        UpdateGamePanel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Card1Click(int index)
    {
        CurrentState = CardStates[index];
        UpdateGamePanel();
    }

    public void UpdateGamePanel()
    {
        switch (CurrentState.Type)
        {
            case CardType.Undecided:
                CardSelectPanel.SetActive(true);
                CardSelectPanel.transform.localPosition = PanelDisplayPosition;

                CardStates[0] = CardStates[3];
                CardStates[1] = CardStates[4];
                CardStates[2] = CardStates[5];
                CardStates[3] = new CardState(CardLocation.Left, CardType.Undecided);
                CardStates[4] = new CardState(CardLocation.Middle, CardType.Undecided);
                CardStates[5] = new CardState(CardLocation.Right, CardType.Undecided);

                int typeInt;

                for (int i = 0; i < 6; i++)
                {
                    if (CardStates[i].Type == CardType.Undecided)
                    {
                        if (StageNum < WorldBossStage[WorldNum])
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
                break;
            case CardType.Monster:
                break;
            case CardType.Chest:
                break;
            case CardType.Buff:
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
