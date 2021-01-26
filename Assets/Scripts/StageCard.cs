using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Debug = UnityEngine.Debug;


public enum WorldId
{
    W1 = 1, W2 = 2, W3 = 3, W4 = 4, W5_1 = 51, W5_2 = 52, 
    W6 = 6, W7_1 = 71, W7_2 = 72, W8 = 8, WX = 9,
}

// 0 - 몬스터 / 1 - 보물 / 2 - 버프 / 3 - 마을 / 4 - 이벤트 / 5 - 보스
public enum CardType
{
    Monster, Chest, Buff, Npc, Random, Boss
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

    public Equipment Equipment;
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
                return $"Equipment: {Equipment.ToString()}";
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
    public Equipment equipment;
    public Artifact artifact;
    public int money;
    public RandomCard(RandomEventType randomEventType, Equipment equipment, Artifact artifact, int money)
    {
        this.Type = CardType.Random;
        this.randomEventType = randomEventType;
        this.equipment = equipment;
        this.artifact = artifact;
        this.money = money;
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
    public readonly WorldId Id;
    public int Number { get => (int)Id < 10 ? (int)Id : ((int)Id / 10); }
    public readonly int BossStage;
    public readonly Random Random;

    public World(WorldId id)
    {
        this.Id = id;
        this.BossStage = GameConstant.BossStage[this.Number - 1];
        this.Random = new Random();
    }

    int GetRewardCoin()
    {
        int worldNum = this.Number;
        return this.Random.Next(GameConstant.RewardCoinMin[worldNum], GameConstant.RewardCoinMax[worldNum]);
    }

    Equipment GetRewardEquipment()
    {
        var shouldWeapon = CustomRandom<bool>.WeightedChoice
        (
            new List<bool> { true, false },
            GameConstant.RewardEquipmentType,
            this.Random
        );
        int rankIndex = CustomRandom<int>.WeightedChoice
        (
            new List<int> { 0, 1, 2, 3, 4 }, 
            GameConstant.RewardRank[this.Number - 1],
            this.Random
        );
        int prefixIndex = CustomRandom<int>.WeightedChoice
        (
            new List<int> { 0, 1, 2, 3, 4 }, 
            GameConstant.RewardPrefix,
            this.Random
        );
        return GetRandomEquipment(shouldWeapon, rankIndex, prefixIndex);
    }

    Equipment GetMerchantEquipment()
    {
        var shouldWeapon = CustomRandom<bool>.WeightedChoice
        (
            new List<bool> { true, false },
            GameConstant.MerchantEquipmentType,
            this.Random
        );
        int rankIndex = CustomRandom<int>.WeightedChoice
        (
            new List<int> { 0, 1, 2, 3, 4 }, 
            GameConstant.MerchantRank[this.Number - 1],
            this.Random
        );
        int prefixIndex = CustomRandom<int>.WeightedChoice
        (
            new List<int> { 0, 1, 2, 3, 4 }, 
            GameConstant.MerchantPrefix,
            this.Random
        );
        return GetRandomEquipment(shouldWeapon, rankIndex, prefixIndex);
    }

    Equipment GetRandomEquipment(bool shouldWeapon, int rankIndex, int prefixIndex)
    {
        if (shouldWeapon)
        {
            var weaponType = (int)CustomRandom<WeaponType>.Choice
            (
                Enum.GetValues(typeof(WeaponType)).Cast<WeaponType>().Where(x=> x != WeaponType.none).ToList(), 
                this.Random
            );
            string weaponId = $"weapon_{weaponType}{rankIndex}{prefixIndex}";
            return JsonDB.GetWeapon(weaponId);
        }
        else 
        {
            var idBase = CustomRandom<string>.Choice
            (
                JsonDB.GetEquipmentIdBases(),
                this.Random
            );
            string equipId = $"{idBase}_{rankIndex}{prefixIndex}";
            return JsonDB.GetEquipment(equipId);
        }
    }

    Artifact GetRewardArtifact()
    {
        return CustomRandom<Artifact>.Choice(JsonDB.GetNotBossArtifacts(), this.Random);
    }
    
    public StageCard GetRandomCard()
    {
        var type = CustomRandom<CardType>.WeightedChoice
        (
            Enum.GetValues(typeof(CardType)).Cast<CardType>().ToList(),
            GameConstant.StageCardType,
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
                    GameConstant.ChestType,
                    this.Random
                );
                switch (chestType)
                {
                    case ChestType.Equipment:
                        return new ChestCard(chestType) { Equipment = GetRewardEquipment() };
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
                    GetMerchantEquipment(),
                    GetMerchantEquipment(),
                    GetMerchantEquipment(),
                };
                return new NpcCard(equipmentsOnSale);
            case CardType.Random:
                var  randomType = CustomRandom<int>.Choice(new List<int> {0, 1, 2}, this.Random);
                var equipmentRand = GetRewardEquipment();
                var artifactRand = GetRewardArtifact();
                var money = 5*GetRewardCoin(); // 보상 획득재화의 5배 (임의설정)
                switch (randomType)
                {
                    case 0:
                        return new RandomCard(RandomEventType.Positive, equipmentRand, artifactRand, money);
                    case 1:
                        return new RandomCard(RandomEventType.Neuturality, equipmentRand, artifactRand, money);
                    case 2:
                        return new RandomCard(RandomEventType.Negative, equipmentRand, artifactRand, money);
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

    public WorldId GetNextWorldId()
    {
        switch(Number)
        {
            case 4:
                return WorldId.W5_1;
            case 52:
                return WorldId.W6;
            case 6:
                return WorldId.W7_1;
            case 72:
                return WorldId.W8;
            case 9: // TODO: the next world of the last world
                return WorldId.WX;
            default:
                return (WorldId)(Number + 1);
        }
    }
}
