using System.Collections;
using System.Collections.Generic;

class GameConstant
{
    // 크리티컬 배수
    public const float CRITMULTIPLIER = 2.0f;

    //앙기모띠 방어구 기본 상수
    public const int DEFENSE_RATIO = 40;
    
    // 플레이어 초기 스탯
    public static Stat PlayerInitialStat = new Stat()
    {
        maxHp = 40,
        attack = 10,
        defense = 3,
        speed = 10,
        startSpeedGauge = 1,
        evasion = 0.05f,
        critical = 0.05f,
    };
    public static int PlayerInitialMoney = 100;
    // 플레이어 초기 장비
    public static string[] PlayerInitialWeapon = new string[10]
    {
        "bare_fist", "bare_fist", "bare_fist", "bare_fist", "bare_fist",
        "bare_fist", "bare_fist", "bare_fist", "bare_fist", "bare_fist",
    };
    public static string[] PlayerInitialEquipment = new string[3]
    {
        "helmet", "armor", "shoes",
    };
  
    // 시작 월드 ID
    public static WorldId InitialWorldId = WorldId.W1;

    // 월드별 보스 등장 스테이지
    public static int[] BossStage = new int[9] { 15, 20, 20, 20, 25 ,25, 30, 30, 30 };

    // 스테이지 카드 등장 확률
    // Monster, Chest, Buff, Npc, Random, Boss
    public static List<double> StageCardType = new List<double> { 0.7, 0.05, 0.05, 0.1, 0.1, 0 };

    // 상자 이벤트 등장 확률
    // Equipment, Heal, Dispel, Damage, Debuff
    public static List<double> ChestType = new List<double> { 0.5, 0.3, 0.1, 0.075, 0.025 };


    // 몬스터 보상 코인 범위
    // 월드별 최소, 최대
    public static int[] RewardCoinMin = new int[12] { 0, 15, 25, 35, 50, 75, 75, 90,  100, 100, 120, 120 };
    public static int[] RewardCoinMax = new int[12] { 0, 25, 35, 45, 60, 90, 90, 100, 110, 110, 130, 150 };

    // 몬스터 보상 장비 종류 확률
    // 무기, 방어구, 힐
    public static List<double> RewardType = new List<double> { 0.7, 0.27, 0.03 };
    
    // 몬스터 보상 장비 등급 확률
    // common, uncommon, rare, unique, legendary
    public static List<List<double>> RewardRank = new List<List<double>>
    {
        new List<double> { 1,    0,    0,    0,    0 },
        new List<double> { 0.75, 0.25, 0,    0,    0 },
        new List<double> { 0.58, 0.4,  0.02, 0,    0 },
        new List<double> { 0.35, 0.5,  0.15, 0,    0 },
        new List<double> { 0.15, 0.43, 0.4,  0.02, 0 },
        new List<double> { 0.1,  0.3,  0.54, 0.05, 0.01 },
        new List<double> { 0.03, 0.25, 0.6,  0.1,  0.02 },
        new List<double> { 0.01, 0.2,  0.64, 0.12, 0.03 },
        new List<double> { 0.01, 0.15, 0.64, 0.15, 0.05 },
    };

    // 몬스터 보상 장비 수식어 확률
    // broken, weak, normal, strong, amazing
    public static List<double> RewardPrefix = new List<double>() { 0.05, 0.25, 0.40, 0.25, 0.05 };


    // 상인 NPC 판매 장비 종류 확률
    // 무기, 방어구
    public static List<double> MerchantEquipmentType = new List<double> { 0.7, 0.3 };
    
    // 상인 NPC 판매 장비 등급 확률
    // common, uncommon, rare, unique, legendary
    public static List<List<double>> MerchantRank = new List<List<double>>
    {
        new List<double> { 1,    0,    0,    0,    0 },
        new List<double> { 0.75, 0.25, 0,    0,    0 },
        new List<double> { 0.58, 0.4,  0.02, 0,    0 },
        new List<double> { 0.35, 0.5,  0.15, 0,    0 },
        new List<double> { 0.15, 0.43, 0.4,  0.02, 0 },
        new List<double> { 0.1,  0.3,  0.54, 0.06, 0 },
        new List<double> { 0.03, 0.25, 0.6,  0.3,  0 },
        new List<double> { 0.01, 0.2,  0.64, 0.15, 0 },
        new List<double> { 0.01, 0.15, 0.64, 0.2,  0 },
    };
    //월드 보스 보상으로 주는 코인
    //{월드,코인}
    public static Dictionary<int, int> bossRewardCoin = new Dictionary<int, int> { { 1, 200 }, { 2, 450 }, { 3, 700 }, { 4, 1000 }, { 5, 1500 }, { 6, 1800 }, { 7, 2000 }, { 8, 2500 }, { 9, 0 }, { 10, 0 } };

    //월드 보스 보상으로 주는 보스아티펙트
    //{월드id,아티펙트id}
    public static Dictionary<WorldId, string> bossArtifacts = new Dictionary<WorldId, string> 
    {
        { WorldId.W1,"artifact0" }, { WorldId.W2, "artifact1" }, { WorldId.W3, "artifact2" },
        { WorldId.W4, "artifact3" }, { WorldId.W5_1, "artifact4" }, { WorldId.W5_2, "artifact5" },
        { WorldId.W6,"artifact6"}, { WorldId.W7_1, "artifact7" }, { WorldId.W7_2, "artifact8" }, { WorldId.W8, "artifact9" },
    };

    // 상인 NPC 판매 장비 수식어 확률
    // broken, weak, normal, strong, amazing
    public static List<double> MerchantPrefix = new List<double>() { 0.05, 0.25, 0.70, 0, 0 };
  

    // 강화 NPC 강화 비용
    public static Dictionary<Rank, Dictionary<Prefix, int>> WeaponEnchantPrice = new Dictionary<Rank, Dictionary<Prefix, int>>
    {
        { 
            Rank.common, new Dictionary<Prefix, int> 
            {
                {Prefix.broken, 25}, {Prefix.weak, 25}, {Prefix.normal, 25}, {Prefix.strong, 25},
            }
        },
        { 
            Rank.uncommon, new Dictionary<Prefix, int> 
            {
                {Prefix.broken, 100}, {Prefix.weak, 100}, {Prefix.normal, 100}, {Prefix.strong, 100},
            }
        },
        { 
            Rank.rare, new Dictionary<Prefix, int> 
            {
                {Prefix.broken, 250}, {Prefix.weak, 250}, {Prefix.normal, 250}, {Prefix.strong, 250},
            }
        },
        { 
            Rank.unique, new Dictionary<Prefix, int> 
            {
                {Prefix.broken, 500}, {Prefix.weak, 500}, {Prefix.normal, 500}, {Prefix.strong, 500},
            }
        },
        { 
            Rank.legendary, new Dictionary<Prefix, int> 
            {
                {Prefix.broken, 750}, {Prefix.weak, 750}, {Prefix.normal, 750}, {Prefix.strong, 1000},
            }
        },
    };


    // 랜덤 이벤트 종류별 확률
    // 긍정, 중립, 부정
    public static List<double> RandomEventType = new List<double> { 1, 1, 1 };
}
