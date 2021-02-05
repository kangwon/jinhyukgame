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

    public void BeatMonster(Monster monster)
    {
        if (monster.isBoss)
        {
            switch (monster.worldId)
            {
                case WorldId.W1:
                    break;
                case WorldId.W2:
                    break;
                case WorldId.W3:
                    break;
                case WorldId.W4:
                    break;
                case WorldId.W5_1:
                    break;
                case WorldId.W5_2:
                    break;
                case WorldId.W6:
                    break;
                case WorldId.W7_1:
                    break;
                case WorldId.W7_2:
                    break;
                case WorldId.W8:
                    break;
                case WorldId.WX:
                    break;
            }
        }
    }
}
