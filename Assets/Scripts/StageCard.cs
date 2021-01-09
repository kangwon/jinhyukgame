using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// typeNum 
// 0 - 몬스터 / 1 - 보물 / 2 - 버프
// 3 - 마을 / 4 - 이벤트 / 5 - 보스
public enum CardType
{
    Monster, Chest, Buff,
    Npc, Random, Boss
}

public enum ChestType
{
    Equipment, Heal, Dispel, Damage, Debuff
}

public class StageCard
{
    public CardType Type;
}

public class MonsterCard : StageCard 
{
    public Monster monster;
    
    public MonsterCard(Monster monster)
    {
        this.Type = CardType.Monster;
        this.monster = monster;
    }
}
public class ChestCard : StageCard 
{
    public ChestType ChestType;

    public float HealPercent;
    public float DamagePercent;

    public ChestCard(ChestType chestType)
    {
        this.Type = CardType.Chest;
        this.ChestType = chestType;
    }

    public string ToString()
    {
        switch (this.ChestType)
        {
            case ChestType.Equipment:
                return $"Equipment Chest";
            case ChestType.Heal:
                return $"Heal {(int)(this.HealPercent * 100)}%";
            case ChestType.Dispel:
                return $"Dispel";
            case ChestType.Damage:
                return $"Damage {(int)(this.DamagePercent * 100)}%";
            case ChestType.Debuff:
                return $"Debuff";
            default:
                throw new NotImplementedException($"Invalid chest type: {this.ChestType.ToString()}");
        }
    }
}
public class BuffCard : StageCard {}
public class NpcCard : StageCard {}
public class RandomCard : StageCard {}

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
                var worldMonsters = JsonDB
                    .GetWorldMonsters(this.Number)
                    .Select(m => m.DeepCopy())
                    .ToList();
                var monster = CustomRandom<Monster>.WeightedChoice
                (
                    worldMonsters,
                    Enumerable.Repeat(1.0, worldMonsters.Count).ToList(),
                    this.Random
                );
                return new MonsterCard(monster);
            case CardType.Chest:
                var chestType = CustomRandom<ChestType>.WeightedChoice
                (
                    Enum.GetValues(typeof(ChestType)).Cast<ChestType>().ToList(),
                    new List<double> { 0.5, 0.3, 0.1, 0.075, 0.025 },
                    this.Random
                );
                switch (chestType)
                {
                    case ChestType.Equipment:
                        // TODO: 몬스터 보상과 동일
                        return new ChestCard(chestType);
                    case ChestType.Heal:
                        return new ChestCard(chestType) { HealPercent = 0.3f };
                    case ChestType.Dispel:
                        return new ChestCard(chestType);
                    case ChestType.Damage:
                        return new ChestCard(chestType) 
                        { 
                            DamagePercent = CustomRandom<float>.WeightedChoice
                            (
                                new List<float> { 0.1f, 0.05f },
                                new List<double> { 1.0, 1.0 },
                                this.Random
                            )
                        };
                    case ChestType.Debuff:
                        return new ChestCard(chestType);
                    default:
                        throw new NotImplementedException($"Invalid chest type: {chestType.ToString()}");
                }
            case CardType.Buff:
                return new BuffCard();
            case CardType.Npc:
                return new NpcCard();
            case CardType.Random:
                return new RandomCard();
            default:
                throw new NotImplementedException($"Invalid card type: {type.ToString()}");
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
