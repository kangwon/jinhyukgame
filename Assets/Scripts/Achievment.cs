using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum AchievementCode
{
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
