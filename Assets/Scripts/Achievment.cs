using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum AchievementCode
{
    CollectFirstLegendary,
    CollectAllEquipmentBroken,
    CollectAllEquipmentAmazing,
    CollectAllWeaponAtk0,
    CollectAllBossArtifact,
    CollectLovyLovelyArtifact,
    CollectAllEquipmentMascotCostume,
    ClearLilly,
    ClearHorlyKnight,
    ClearSpinaRosa,
    ClearSnoopB,
    ClearUrePoppung,
    ClearDeepOcean,
    ClearDoppelganger,
    ClearMasterTiger,
    ClearJupiterQueen,
    ClearBloodyWulf,
    ClearDragRyan,
    ClearHeartHeartz,
}

[System.Serializable]
public class Achievement : JsonItem
{
    public AchievementCode code 
    {
        get => (AchievementCode) Enum.Parse(typeof(AchievementCode), this.id, true); 
    }
    public string name;
    public string description;
}

[System.Serializable]
public class AchievementBoard
{
    private Dictionary<AchievementCode, bool> data = new Dictionary<AchievementCode, bool>();

    public AchievementBoard()
    {
        // TODO: 업적 완료 여부를 저장하고 불러올 수 있어야 함
        foreach (AchievementCode code in Enum.GetValues(typeof(AchievementCode)))
        {
            this.data[code] = false;
        }
    }

    public void Achieve(AchievementCode code) => data[code] = true;
    public bool HasAchieved(AchievementCode code) => data[code];
}

public class AchievementManager
{
    public AchievementBoard Board;
    public AchievementPanelController panelController;

    private static readonly AchievementManager instance = new AchievementManager();
    static AchievementManager() {}
    private AchievementManager() 
    {
        Board = new AchievementBoard();
    }
    public static AchievementManager Instance { get => instance; }

    void CheckAndAchieve(AchievementCode code)
    {
        if (!Board.HasAchieved(code))
        {
            Board.Achieve(code);

            Achievement achievment = JsonDB.GetAchievment(code);
            panelController?.Show(achievment);
        }
    }
    
    public static void CollectEquipments(EquipmentSlot equipmentSlot)
    {
        int countEquipmentBroken = 0;
        int countEquipmentAmazing = 0;
        int countWeaponAtk0 = 0;
        if ((equipmentSlot.GetWeaponsList().Count() <= 10) && (equipmentSlot.GetArtifacts().Count <=3))
        {
            foreach(var w in equipmentSlot.GetWeaponsList())
            {
                if (w.rank == Rank.legendary) Instance.CheckAndAchieve(AchievementCode.CollectFirstLegendary);
                if (w.prefix == Prefix.broken) countEquipmentBroken++;
                if (w.prefix == Prefix.amazing) countEquipmentAmazing++;
                if ((w.id !="bare_fist") && (w.statEffect.attack == 0)) countWeaponAtk0++;
            }
            if (countWeaponAtk0 == 10) Instance.CheckAndAchieve(AchievementCode.CollectAllWeaponAtk0);
            foreach (var e in equipmentSlot.GetEquipments())
            {
                if (e.prefix == Prefix.broken) countEquipmentBroken++;
                if (e.prefix == Prefix.amazing) countEquipmentAmazing++;
            }
            if (countEquipmentBroken == 13) Instance.CheckAndAchieve(AchievementCode.CollectAllEquipmentBroken);
            if (countEquipmentAmazing == 13) Instance.CheckAndAchieve(AchievementCode.CollectAllEquipmentAmazing);
            if (equipmentSlot.GetEquipments().Where(e=>(e.name== "인형탈 머리") || (e.name == "인형탈 몸통")||(e.name== "인형탈 신발")).Count()==3) Instance.CheckAndAchieve(AchievementCode.CollectAllEquipmentMascotCostume);
            if (equipmentSlot.GetArtifacts().Where(a => (a.id == "artifact30") || (a.id == "artifact31")).Count() ==2) Instance.CheckAndAchieve(AchievementCode.CollectLovyLovelyArtifact);
            if (equipmentSlot.GetArtifacts().Where(a => a.isBossItem).Count() ==3) Instance.CheckAndAchieve(AchievementCode.CollectAllBossArtifact);


        }
    }

    public static void BeatMonster(Monster monster)
    {
        AchievementCode? achieveCode = null;
        if (monster.isBoss)
        {
            switch (monster.worldId)
            {
                case WorldId.W1:
                    achieveCode = AchievementCode.ClearLilly;
                    break;
                case WorldId.W2:
                    achieveCode = AchievementCode.ClearHorlyKnight;
                    break;
                case WorldId.W3:
                    achieveCode = AchievementCode.ClearSpinaRosa;
                    break;
                case WorldId.W4:
                    achieveCode = AchievementCode.ClearSnoopB;
                    break;
                case WorldId.W5_1:
                    achieveCode = AchievementCode.ClearUrePoppung;
                    break;
                case WorldId.W5_2:
                    achieveCode = AchievementCode.ClearDeepOcean;
                    break;
                case WorldId.W6:
                    achieveCode = AchievementCode.ClearDoppelganger;
                    break;
                case WorldId.W7_1:
                    achieveCode = AchievementCode.ClearMasterTiger;
                    break;
                case WorldId.W7_2:
                    achieveCode = AchievementCode.ClearJupiterQueen;
                    break;
                case WorldId.W8:
                    achieveCode = AchievementCode.ClearDragRyan;
                    break;
                case WorldId.WX:
                    achieveCode = AchievementCode.ClearHeartHeartz;
                    break;
            }
        }
        if (achieveCode is AchievementCode code)
            Instance.CheckAndAchieve(code);
    }
}
