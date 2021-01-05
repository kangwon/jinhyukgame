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

    public StageCard(CardType type)
    {
        Type = type;
    }
}

public class StageCardGenerator
{
    public static StageCard GetRandomCard(int world, int stage, int location)
    {
        int seed = 14 + GameState.Instance.GlobalSeed + world + stage + location;
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

        return new StageCard(type);
    }
}

public class WorldStage
{
    public static int NUM_OF_CARDS = 3;

    public readonly int Number;
    
    public List<StageCard> Cards;

    public WorldStage(int number)
    {
        this.Number = number;
        this.Cards = new List<StageCard>();
    }
}

public class World
{
    public readonly int Number;
    public readonly string Name;

    public World(int number, string name)
    {
        this.Number = number;
        this.Name = name;
    }

    public WorldStage GetStage(int stageNum)
    {
        var stage = new WorldStage(stageNum);
        for (int location = 0; location < WorldStage.NUM_OF_CARDS; location++)
        {
            stage.Cards.Add(StageCardGenerator.GetRandomCard(this.Number, stage.Number, location));
        }
        return stage;
    }
}