using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Debug = UnityEngine.Debug;

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
public enum RandomEventType
{
    Positive, Neuturality, Negative
}

public class StageCard
{
    public CardType Type;
}

public class MonsterCard : StageCard 
{
    public readonly Monster monster;
    public readonly Equipment[] rewardEquipments;
    public int rewardCoin;
    
    public MonsterCard(Monster monster, Equipment[] equipments, int coin)
    {
        this.Type = CardType.Monster;
        this.monster = monster;
        this.rewardEquipments = equipments;
        this.rewardCoin = coin;
    }
}
public class ChestCard : StageCard 
{
    public ChestType ChestType;

    public float HealPercent;
    public float DamagePercent;
    public StatBuff Debuff;

    public ChestCard(ChestType chestType)
    {
        this.Type = CardType.Chest;
        this.ChestType = chestType;
    }

    public override string ToString()
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
                return $"{Debuff.name}\n{Debuff.description}";
            default:
                throw new NotImplementedException($"Invalid chest type: {this.ChestType.ToString()}");
        }
    }
}
public class BuffCard : StageCard 
{
    public StatBuff Buff;

    public BuffCard(StatBuff buff)
    {
        this.Type = CardType.Buff;
        this.Buff = buff;
    }
}
public class NpcCard : StageCard 
{
    public readonly Equipment[] EquipmentsOnSale;

    public NpcCard(Equipment[] equipmentsOnSale)
    {
        this.Type = CardType.Npc;
        this.EquipmentsOnSale = equipmentsOnSale;
    }
}
public class RandomCard : StageCard 
{
    public RandomEventType randomEventType;
    public RandomCard(RandomEventType randomEventType)
    {
        this.Type = CardType.Random;
        this.randomEventType = randomEventType;
    }
}
public class BossCard : MonsterCard 
{    
    public BossCard(Monster monster, Equipment[] equipments, int coin) : base(monster, equipments, coin)
    {
        this.Type = CardType.Boss;
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
    public readonly int BossStage;
    public readonly Random Random;

    List<List<double>> rankPercentage = new List<List<double>>
    {
        new List<double> {1,    0,    0,    0,    0},
        new List<double> {0.75, 0.25, 0,    0,    0},
        new List<double> {0.58, 0.4,  0.02, 0,    0},
        new List<double> {0.35, 0.5,  0.15, 0,    0},
        new List<double> {0.15, 0.43, 0.4,  0.02, 0},
        new List<double> {0.1,  0.3,  0.54, 0.05, 0.01},
        new List<double> {0.03, 0.25, 0.6,  0.1,  0.02},
        new List<double> {0.01, 0.2,  0.64, 0.12, 0.03},
        new List<double> {0.01, 0.15, 0.64, 0.15, 0.05}
    };
    int[] coinMin = new int[12] {0, 15, 25, 35, 50, 75, 75, 90,  100, 100, 120, 120};
    int[] coinMax = new int[12] {0, 25, 35, 45, 60, 90, 90, 100, 110, 110, 130, 150};
    List<double> prefixPercentage = new List<double>() { 0.05, 0.25, 0.40, 0.25, 0.05 };

    public World(int number, string name, int bossStage=5)
    {
        this.Number = number;
        this.Name = name;
        this.BossStage = bossStage;
        this.Random = new Random();
    }

    Equipment GetRewardEquipment()
    {
        int worldNum = this.Number;
        int rewardPrefixIndex = CustomRandom<int>.WeightedChoice
        (
            new List<int> { 0, 1, 2, 3, 4 }, 
            prefixPercentage,
            this.Random
        );
        int rewardRankIndex = CustomRandom<int>.WeightedChoice
        (
            new List<int> { 0, 1, 2, 3, 4 }, 
            rankPercentage[worldNum - 1],
            this.Random
        );
        var rewardTypeRand = CustomRandom<int>.WeightedChoice
        (
            new List<int> {0, 1},
            new List<double> {0.7, 0.3},
            this.Random
        );
        if (rewardTypeRand == 0)
        {
            var weaponType = (int)CustomRandom<WeaponType>.Choice
            (
                Enum.GetValues(typeof(WeaponType)).Cast<WeaponType>().Where(x=>x!=WeaponType.none).ToList(), 
                this.Random
            );
            string weaponId = $"weapon_{weaponType}{rewardRankIndex}{rewardPrefixIndex}";
            return JsonDB.GetWeapon(weaponId);
        }
        else 
        {
            var idBase = CustomRandom<string>.Choice
            (
                JsonDB.GetEquipmentIdBases(),
                this.Random
            );
            string equipId = $"{idBase}_{rewardRankIndex}{rewardPrefixIndex}";
            return JsonDB.GetEquipment(equipId);
        }
    }

    int GetRewardCoin()
    {
        int worldNum = this.Number;
        return this.Random.Next(coinMin[worldNum], coinMax[worldNum]);
    }

    public StageCard GetRandomCard()
    {
        var type = CustomRandom<CardType>.WeightedChoice
        (
            Enum.GetValues(typeof(CardType)).Cast<CardType>().ToList(),
            new List<double> { 0.7, 0.05, 0.05, 0.1, 50.1, 0 },
            this.Random
        );
        
        switch (type)
        {
            case CardType.Monster:
                var worldMonsters = JsonDB.GetWorldMonsters(this.Number);
                var monster = CustomRandom<Monster>.Choice(worldMonsters, this.Random);
                var rewardEquipments = new Equipment[3] 
                {
                    GetRewardEquipment(),
                    GetRewardEquipment(),
                    GetRewardEquipment(),
                };
                var rewardCoin = GetRewardCoin();
                return new MonsterCard(monster, rewardEquipments, rewardCoin);
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
                            DamagePercent = CustomRandom<float>.Choice
                            (
                                new List<float> { 0.1f, 0.05f },
                                this.Random
                            )
                        };
                    case ChestType.Debuff:
                        return new ChestCard(chestType)
                        {
                            Debuff = CustomRandom<StatBuff>.Choice
                            (
                                JsonDB.GetDebuffs(), 
                                this.Random
                            )
                        };
                    default:
                        throw new NotImplementedException($"Invalid chest type: {chestType.ToString()}");
                }
            case CardType.Buff:
                var buff = CustomRandom<StatBuff>.Choice(JsonDB.GetBuffs(), this.Random);
                return new BuffCard(buff);
            case CardType.Npc:
                var equipmentsOnSale = new Equipment[3] 
                {
                    GetRewardEquipment(),
                    GetRewardEquipment(),
                    GetRewardEquipment(),
                };
                return new NpcCard(equipmentsOnSale);
            case CardType.Random:
                var  randomType = CustomRandom<int>.Choice(new List<int> {0, 1, 2}, this.Random);
                switch (randomType)
                {
                    case 0:
                        return new RandomCard(RandomEventType.Positive);
                    case 1:
                        return new RandomCard(RandomEventType.Neuturality);
                    case 2:
                        return new RandomCard(RandomEventType.Negative);
                    default:
                        throw new NotImplementedException($"Invalid Random type ");
                }
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
        if (stageNum >= this.BossStage)
        {
            var boss = JsonDB.GetWorldBoss(this.Number);
            // TODO: 보스 보상을 보스에 맞춰 바꿔야 함
            var rewardEquipments = new Equipment[3] 
            {
                GetRewardEquipment(),
                GetRewardEquipment(),
                GetRewardEquipment(),
            };
            var rewardCoin = GetRewardCoin();
            stage.Cards[1] = new BossCard(boss, rewardEquipments, rewardCoin);
        }
        return stage;
    }
}
