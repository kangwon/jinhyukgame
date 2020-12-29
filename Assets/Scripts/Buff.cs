using System;

public class StatBuff : Stat
{

    public float maxHpPercent = 0; // 퍼센트로 증가할때 필요. 30% = 0.3f
    public float attackPercent = 0;
    public float defensePercent = 0;
    public float speedPercent = 0;
    public int maxHpAbsolute = 0; // 절대치로 증가할때
    public int attackAbsolute = 0;
    public int defenseAbsolute = 0;
    public int speedAbsolute = 0;

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
    }
}