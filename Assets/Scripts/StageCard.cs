using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// location 0 - 좌 / 1 - 중 / 2 - 우
public enum CardLocation
{
    Left, Middle, Right,
}

// typeNum 
// 0 - 몬스터 / 1 - 보물 / 2 - 버프
// 3 - 마을 / 4 - 이벤트 / 5 - 보스
public enum CardType
{
    Monster, Chest, Buff,
    Npc, Random, Boss
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
    public Monster monster;
    public MonsterCard(CardType type, Monster monster) : base(type) 
    {
        this.monster = monster;
    }
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
    public readonly Random Random;

    public World(int number, string name)
    {
        this.Number = number;
        this.Name = name;
        this.Random = new Random();
    }

    public StageCard GetRandomCard()
    {
        var type = CustomRandom<CardType>.WeightedChoice
        (
            Enum.GetValues(typeof(CardType)).Cast<CardType>().ToList(),
            new List<double> { 0.7, 0.05, 0.05, 0.1, 0.1, 0 },
            this.Random
        );
        
        switch (type)
        {
            case CardType.Monster:
                var worldMonsters = JsonDB.GetWorldMonsters(this.Number);
                var monster = CustomRandom<Monster>.WeightedChoice
                (
                    worldMonsters,
                    Enumerable.Repeat(1.0, worldMonsters.Count).ToList(),
                    this.Random
                );
                return new MonsterCard(type, monster);
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

    public WorldStage GetStage(int stageNum)
    {
        var stage = new WorldStage(stageNum);
        for (int location = 0; location < WorldStage.NUM_OF_CARDS; location++)
        {
            stage.Cards.Add(this.GetRandomCard());
        }
        return stage;
    }
}