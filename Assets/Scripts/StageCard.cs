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

public class MonsterCard : StageCard 
{
    public MonsterCard(CardType type) : base(type) {}
}
public class ChestCard : StageCard 
{
    public ChestCard(CardType type) : base(type) {}
}
public class BuffCard : StageCard 
{
    public BuffCard(CardType type) : base(type) {}
}
public class NpcCard : StageCard 
{
    public NpcCard(CardType type) : base(type) {}
}
public class RandomCard : StageCard 
{
    public RandomCard(CardType type) : base(type) {}
}

public class StageCardGenerator
{
    public static StageCard GetRandomCard(int world, int stage, int location)
    {
        int seed = GameState.Instance.GlobalSeed + world + stage + location;
        var rand = new Random(seed);

        var type = CustomRandom<CardType>.WeightedChoice
        (
            Enum.GetValues(typeof(CardType)).Cast<CardType>().ToList(),
            new List<double> { 0.7, 0.05, 0.05, 0.1, 0.1 },
            seed
        );
        
        switch (type)
        {
            case CardType.Monster:
                return new MonsterCard(type);
            case CardType.Chest:
                return new ChestCard(type);
            case CardType.Buff:
                return new BuffCard(type);
            case CardType.Npc:
                return new NpcCard(type);
            case CardType.Random:
                return new RandomCard(type);
            default:
                throw new NotImplementedException($"Invalid card type: {type.GetType().ToString()}");
        }
    }
}

public class WorldStage
{
    public const int NUM_OF_CARDS = 3;

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