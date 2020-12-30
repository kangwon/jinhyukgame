using System;

public class StatBuff : Stat
{
    public Stat baseBuffStat = new Stat(); //절대치 증가
    public int buffcount; // 스테이지마다 카운트되게 설정 -> buffcount-- //-1이면 무한지속.
    public float maxHpPercent = 0; // 퍼센트로 증가할때 필요. 30% = 0.3f
    public float attackPercent = 0;
    public float defensePercent = 0;
    public float speedPercent = 0;
    public float discountPercent = 0;
    public float hpDrainPercent = 0;
    public bool isDebuff = false;
    public bool debuffImmune = false;
    public Stat GetTotalStat(CharacterBase cb)
    {
        //*퍼센트로 붙은 버프를 적용하기전에 해당 캐릭터를 가져와 수치를 구한다.
        //반드시 적용하기 전에 이 함수를 써서 수치를 구해야함. 
       return this.GetTotalStat(cb.baseStat);
    }
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
    public bool IsEndBuff()
    {
        if (buffcount == 0) return true;
        else return false;
    }

    public int CountBuff()
    {
        //스테이지가 지날때마다 지속시간이 감소.(스테이지 지날때 이 메서드를 넣으면 됨)
        if (!IsEndBuff()||!(buffcount==-1))
        {
            buffcount--;
        }
        return buffcount;
    }
}