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
    //아티펙트용 변수------------------------------------------------------
    public float npcPurchasePercent = 0; //마을 할인용 변수
    public bool firstAttackCritical = false; //첫타격 확정치명타
    public bool resurrection = false; //1회 부활
    public bool firstDamagedImmune = false; //첫번째 피격 데미지 무시
    public float bossDamageDecrease = 0; //보스전 피격대미지 감소
    public float battleEndHealPercent = 0; //전투 종료시 체력 ()% 회복
    public float doubleAttackPercent = 0; //()%확률로 2번공격
    public int continueBattleAddDamage = 0; //연속전투시 추가데미지
    public int continueBattleCoin = 0; //연속전투시 추가재화
    public int attack3AddDamage = 0; //세번째 타격마다 추가고정데미지
    public float enemyHpPercentDamage = 0; //상대체력퍼센트()% 추가고정데미지
    public int nasusQ =0; // 잡은몬스터*n 만큼 추가데미지 (스택제한이 있다)
    public float reflectionDamage = 0; //받은피해에 ()% 반사 데미지
    public int bossAddDamage = 0; //보스전 추가 데미지
    public bool statusEffectImmune = false; //상태이상 면역
    public bool hp1Left = false; // 전투시마다 피1로 1회 버티기
    public bool resetStageCard = false; // 스테이지 카드 리셋 충전식 (5스테이지마다)
    public float lostHpRatioDamageUp = 0; // 잃은 체력 비례 데미지 증가
    public float lostHpRatioSpeedUp = 0; // 잃은 체력 비례 스피드 증가
    public float monsterChanceDecrease = 0; //몬스터 조우확률 ()% 감소
    public float monsterChanceIncrease = 0; //몬스터 조우확룰 ()% 증가
    public float rewardBonusPercent = 0; //몬스터 처치시 ()% 추가보상기회
    public float battleTurnRatioStatUp = 0; //한턴당 스탯증가
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