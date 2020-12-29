using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardType
{
    // location 0 - 좌 / 1 - 중 / 2 - 우
    public int Location { get; set; }
    // typeNum 0 - 미정 / 1 - 몬스터 / 2 - 보물
    // 3 - 버프 / 4 - 이벤트 / 5 - 마을 / 6 - 보스
    public int TypeNum { get; set; }

    private void Clear()
    {
        Location = 0;
        TypeNum = 0;
    }

    public CardType()
    {
        Clear();
    }
    public CardType(int location, int typeNum) : this()
    {
        Location = location;
        TypeNum = typeNum;
    }
}

public class StageChoice : MonoBehaviour
{

    public int GameState = 0;
    public int StageNum = 0;
    public int WorldNum = 1;
    public bool BossClear = false;

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

    public GameObject card1, card2, card3;
    public GameObject nextcard1, nextcard2, nextcard3;

    public System.Random ran = new System.Random();

    public GameObject cardtxt1, cardtxt2, cardtxt3;

    List<CardType> Cards = new List<CardType>()
    {
        new CardType(0,0), new CardType(1,0), new CardType(2,0),
        new CardType(0,0), new CardType(1,0), new CardType(2,0)
    };

    void Active(bool B)
    {
        card1.SetActive(B);
        card2.SetActive(B);
        card3.SetActive(B);
        nextcard1.SetActive(B);
        nextcard2.SetActive(B);
        nextcard3.SetActive(B);
    }

    public void Card1Click()
    {
        Active(false);
        GameState = Cards[0].TypeNum;
        Start();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameState == 0)
        {
            Active(true);
            int typeInt;

            Cards[0] = Cards[3];
            Cards[1] = Cards[4];
            Cards[2] = Cards[5];
            Cards[3] = new CardType(0, 0);
            Cards[4] = new CardType(1, 0);
            Cards[5] = new CardType(2, 0);

            for (int i = 0; i < 6; i++)
            {
                if (Cards[i].TypeNum == 0)
                {
                    if (StageNum < WorldBossStage[WorldNum])
                    {
                        typeInt = ran.Next(1, 101);
                        if (typeInt <= 70) Cards[i].TypeNum = 1;
                        else if (typeInt <= 75) Cards[i].TypeNum = 2;
                        else if (typeInt <= 80) Cards[i].TypeNum = 3;
                        else if (typeInt <= 90) Cards[i].TypeNum = 4;
                        else if (typeInt <= 100) Cards[i].TypeNum = 5;
                    }
                    else if (Cards[i].Location == 1)
                    {
                        Cards[i].TypeNum = 6;
                    }
                    else
                    {
                        typeInt = ran.Next(1, 101);
                        if (typeInt <= 73) Cards[i].TypeNum = 1;
                        else if (typeInt <= 80) Cards[i].TypeNum = 2;
                        else if (typeInt <= 87) Cards[i].TypeNum = 3;
                        else if (typeInt <= 100) Cards[i].TypeNum = 4;
                    }
                }
            }
        }
        else if (GameState == 1)
        {

        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
