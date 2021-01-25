using System.Collections;
using System.Collections.Generic;

class GameConstant
{
    // 플레이어 초기 스탯
    public static Stat PlayerInitialStat = new Stat()
    {
        maxHp = 40,
        attack = 10,
        defense = 0,
        speed = 10,
        startSpeedGauge = 1,
        evasion = 0.05f,
        critical = 0.05f,
    };
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

    // 시작 월드
    public static int InitialWorldNumber = 1;

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
    // 무기, 방어구
    public static List<double> RewardEquipmentType = new List<double> { 0.7, 0.3 };
    
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

    // 상인 NPC 판매 장비 수식어 확률
    // broken, weak, normal, strong, amazing
    public static List<double> MerchantPrefix = new List<double>() { 0.05, 0.25, 0.70, 0, 0 };
}
