using System;

public class StatBuff : Stat
{
    public int buffcount; // 스테이지마다 카운트되게 설정 -> buffcount--
    public float maxHpPercent = 0; // 퍼센트로 증가할때 필요. 30% = 0.3f
    public float attackPercent = 0;
    public float defensePercent = 0;
    public float speedPercent = 0;
    public float criticalPercent = 0;
    public float evasionPercent = 0;
    public float discountPercent = 0;
    public float hpDrainPercent = 0;
    public int maxHpAbsolute = 0; // 절대치로 증가할때
    public int attackAbsolute = 0;
    public int defenseAbsolute = 0;
    public int speedAbsolute = 0;
    public int startSpeedGaugeAbsolute = 0;
    public bool isDebuff = false;
    public bool debuffImmune = false;
    public void CalcStat(CharacterBase cb)
    {
        //*퍼센트로 붙은 버프를 적용하기전에 해당 캐릭터를 가져와 수치를 구한다.
        //반드시 적용하기 전에 이 함수를 써서 수치를 구해야함. 
        this.CalcStat(cb.baseStat);
    }
    public void CalcStat(Stat stat)
    {
        //*퍼센트로 붙은 버프를 적용하기전에 해당 캐릭터의 baseStat을 가져와 수치를 구한다.
        //반드시 적용하기 전에 이 함수를 써서 수치를 구해야함. 
        maxHp = (int)(maxHpPercent * stat.maxHp) + maxHpAbsolute;
        attack = (int)(attackPercent * stat.attack) + attackAbsolute;
        defense = (int)(defensePercent * stat.defense) + defenseAbsolute;
        speed = (int)(speedPercent * stat.speed) + speedAbsolute;
        startSpeedGauge = startSpeedGaugeAbsolute;
        evasion = evasionPercent;
        critical = criticalPercent;
    }
    public bool IsDebuff() => this.isDebuff;
    public bool IsBuff() => !this.isDebuff;
    public bool IsEndBuff()
    {
        if (buffcount == 0) return true;
        else return false;
    }
}