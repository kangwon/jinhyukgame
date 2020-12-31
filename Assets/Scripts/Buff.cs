using System;

[System.Serializable]
public class StatBuff :JsonItem
{
    public Stat baseBuffStat = new Stat(); //절대치 증가
    public float maxHpPercent = 0; // 퍼센트로 증가할때 필요. 30% = 0.3f
    public float attackPercent = 0;
    public float defensePercent = 0;
    public float speedPercent = 0;
    public float discountPercent = 0;
    public float hpDrainPercent = 0;
    public bool isDebuff = false;
    public bool debuffImmune = false;

    public Stat GetTotalStat(Stat stat)
    {
        //*퍼센트로 붙은 버프를 적용하기전에 해당 캐릭터의 baseStat을 가져와 수치를 구한다.
        //반드시 적용하기 전에 이 함수를 써서 수치를 구해야함. 
        return new Stat
        {
            maxHp = (int)(maxHpPercent * stat.maxHp) + baseBuffStat.maxHp,
            attack = (int)(attackPercent * stat.attack) + baseBuffStat.attack,
            defense = (int)(defensePercent * stat.defense) + baseBuffStat.defense,
            speed = (int)(speedPercent * stat.speed) + baseBuffStat.speed,
            startSpeedGauge = baseBuffStat.startSpeedGauge,
            evasion = baseBuffStat.evasion,
            critical = baseBuffStat.critical
        };
    }
    public bool IsDebuff() => this.isDebuff;
    public bool IsBuff() => !this.isDebuff;

}