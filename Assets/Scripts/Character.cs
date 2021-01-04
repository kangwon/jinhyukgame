using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Stat : MonoBehaviour
{
    public int maxHp;
    public int attack;
    public int defense;
    public int speed;
    public int startSpeedGauge;
    public float critical;
    public float evasion;

    public static Stat operator +(Stat a, Stat b)
    {
        return new Stat
        {
            maxHp = a.maxHp + b.maxHp,
            attack = a.attack + b.attack,
            defense = a.defense + b.defense,
            speed = a.speed + b.speed,
            startSpeedGauge = a.startSpeedGauge + b.startSpeedGauge,
            critical = a.critical + b.critical,
            evasion = a.evasion + b.evasion
        };
    }
    public override string ToString()
    {
        return $"Stat(hp:{maxHp}, atk:{attack}, def:{defense}, spd:{speed})";
    }
}

public class CharacterBase
{
    public readonly Stat baseStat;
    public int hp;
    public bool isDead
    {
        get => hp <= 0;
    }

    public CharacterBase(Stat stat)
    {
        this.baseStat = stat;
        this.hp = this.baseStat.maxHp;
    }

    public virtual Stat GetStat()
    {
        return baseStat;
    }

    public virtual float AttackFoe() {return 0;} // 공격하는 함수

    public virtual void TakeHit(float rawDamage) {} // 데미지 받는 함수 TODO : IntFloat 해결
}

public class Monster : CharacterBase
{
    public Monster(Stat stat) : base(stat) { }
    public string monsterName {get; set;}
    public bool isBoss;

    public override void TakeHit(float rawDamage) {
        float afterDamage = CalcDamage(rawDamage); 
        if(this.GetStat().defense >= afterDamage) {
            hp = hp - 1;
        } else {
            hp = hp + this.GetStat().defense - (int)afterDamage;
        }
    }

    public override float AttackFoe() {
        float finalDamage = this.GetStat().attack; //TODO : 공격 기믹 추가
        return finalDamage;
    }

    float CalcDamage(float incomingDmg) {
        return incomingDmg; // TODO : 몬스터 데미지 계산식
    }
}

public class Player : CharacterBase
{
    StatBuff buff = new StatBuff();
    EquipmentSlot equipmentSlot = new EquipmentSlot();
    public float hpDrain = 0f; // 일단은 곱연산
    public int money = 100;
    
    public Player(Stat stat) : base(stat) {}

    public void AddBuff(StatBuff buff)
    {
        if (this.buff.debuffImmune == true || buff.IsDebuff()) // 1회 디버프무효화가 있고, 들어오는 버프가 디버프일때. 디버프를 무효화한다.
        {
            this.buff.debuffImmune = false;
            return;
        }
        this.buff = buff;
    }

    public override Stat GetStat()
    {      
        Stat currentstat = this.baseStat + buff.GetTotalStat(this.baseStat) + equipmentSlot.GetTotalStat();
        return currentstat;
    }

    public void SetEquipment(Equipment equip)
    {
        equipmentSlot.SetEquipment(equip);
    }

    public void BuyItem(Equipment item)
    {
        if (money >= item.price)
        {
            money -= item.price;
            this.SetEquipment(item);
        }
    }

    public override void TakeHit(float rawDamage) {
        float afterDamage = CalcDamage(rawDamage);
        if(this.GetStat().defense >= afterDamage) {
            hp = hp - 1;
        } else {
            hp = hp + this.GetStat().defense - (int)afterDamage;
        }
    }

    public override float AttackFoe() {
        float finalDamage = this.GetStat().attack;  //TODO : 공격 기믹 추가
        return finalDamage;
    }

    float CalcDamage(float incomingDmg) {
        return incomingDmg;  // TODO : 유물 등의 추가 방어 기믹 추후 추가 
    }
}
