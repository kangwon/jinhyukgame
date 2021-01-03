using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

public class StageCard
{
    public CardLocation Location;
    public CardType Type;

    public StageCard(CardLocation location, CardType type)
    {
        Location = location;
        Type = type;
    }
}

public class StageCardGenerator
{
    public StageCard GetRandomCard(CardLocation location)
    {
        int seed = GameState.Instance.StageSeed + (int)location;
        var rand = new Random(seed);

        CardType type;
        double typeProb = rand.NextDouble();
        if (typeProb < 0.7)
            type = CardType.Monster;
        else if (typeProb < 0.75)
            type = CardType.Chest;
        else if (typeProb < 0.8)
            type = CardType.Buff;
        else if (typeProb < 0.9)
            type = CardType.Random;
        else
            type = CardType.Npc;

        return new StageCard(location, type);
    }
}