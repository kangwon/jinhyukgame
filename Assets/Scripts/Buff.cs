using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[System.Serializable]
public class StatBuff :JsonItem
{
    public string name = "버프 없음";
    public string description = "버프 없음"; 
    public Stat baseBuffStat = new Stat(); //절대치 증가
    public StatPercent percentStat = new StatPercent(); // 퍼센트 증가
    public float discountPercent = 0;
    public float evasionPercent = 0;
    public float criticalPercent = 0;
    public float vampirePercent = 0; // 흡혈 회복량 변수
    public float stageHpDrainPercent = 0; // 스테이지 당 회복량 변수
    public float hpDrainPercent = 0; // 회복용 변수
    public float cashPercent = 0; // 전투 시 추가 재화용 변수
    public float purchasePercent = 0; // 상점 할인용 변수
    public bool isDebuff = false;
    public bool debuffImmune = false;
    public bool iCantUsedCombo = false;
    public int preemptiveUserTurn = 0;
    public int preemptiveMonTurn = 0;
    public bool handReroll = false;

    public float GetHP(CharacterBase characterBase)       // 스테이지마다 함수 구현 필요
    {
        return characterBase.baseStat.maxHp*stageHpDrainPercent + characterBase.hp;
    }

    public Stat GetTotalStat(Stat stat)
    {
        //*퍼센트로 붙은 버프를 적용하기전에 해당 캐릭터의 baseStat을 가져와 수치를 구한다.
        //반드시 적용하기 전에 이 함수를 써서 수치를 구해야함.
        return new Stat
        {
            maxHp = (int)(percentStat.maxHp * stat.maxHp) + baseBuffStat.maxHp,
            attack = (int)(percentStat.attack * stat.attack) + baseBuffStat.attack,
            defense = (int)(percentStat.defense * stat.defense) + baseBuffStat.defense,
            speed = (int)(percentStat.speed * stat.speed) + baseBuffStat.speed,
            startSpeedGauge = baseBuffStat.startSpeedGauge + preemptiveUserTurn*(200 - baseBuffStat.startSpeedGauge),
            evasion = evasionPercent + baseBuffStat.evasion,
            critical = criticalPercent + baseBuffStat.critical,
            vampire = vampirePercent + baseBuffStat.vampire
        };
    }

    public bool IsDebuff() => this.isDebuff;
    public bool IsBuff() => !this.isDebuff;

}