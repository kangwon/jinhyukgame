using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public int maxHp;
    public int attack;
    public int defense;
    public int speed;
    public int startSpeedGauge;
    public float critical;
    public float evasion;
    public float vampire = 0;       // 흡혈률

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
            evasion = a.evasion + b.evasion,
            vampire = a.vampire + b.vampire
        };
    }

    public override string ToString()
    {
        return $"Stat(hp:{maxHp}, atk:{attack}, def:{defense}, spd:{speed})";
    }

    public Stat DeepCopy() => (Stat)this.MemberwiseClone();
}

[System.Serializable]
public class CharacterBase : JsonItem
{
    public Stat baseStat;
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

[System.Serializable]
public class Monster : CharacterBase
{
    public string name;
    public bool isBoss;
    public int worldNumber;

    public Monster(Stat stat) : base(stat) { }
    public Monster(string name, Stat stat) : base(stat)
    {
        this.name = name;
    }

    public Monster DeepCopy()
    {
       return new Monster(this.name, this.baseStat.DeepCopy());
    }

    public override void TakeHit(float rawDamage) 
    {
        float afterDamage = CalcDamage(rawDamage); 
        if(this.GetStat().defense >= afterDamage) 
        {
            hp = hp - 1;
        } 
        else 
        {
            hp = hp + this.GetStat().defense - (int)afterDamage;
        }
        //Debug.Log($"이제 몬스터 피 : {hp}임.");
    }

    public override float AttackFoe() 
    {
        float finalDamage = this.GetStat().attack; // TODO : 공격 기믹 추가
        return finalDamage;
    }

    float CalcDamage(float incomingDmg) 
    {
        return incomingDmg; // TODO : 몬스터 데미지 계산식
    }
}

public class Player : CharacterBase
{

    StatBuff buff = new StatBuff();
    EquipmentSlot equipmentSlot = new EquipmentSlot();
    public float hpDrain = 0f; // 일단은 곱연산
    public int money = 100;
    
    public List<Weapon> GetWeaponList()
    {
       return equipmentSlot.GetWeaponsList();
    }
    public void SetWeaponList(List<Weapon> weapons)
    {
        equipmentSlot.SetWeaponsList(weapons);
    }
    public void ResetWeaponList()
    {
        equipmentSlot.ResetWeaponsList();
    }

    public Player(Stat stat) : base(stat) {}

    public StatBuff GetBuff()
    {
        return buff;
    }
    public void AddBuff(StatBuff buff)
    {
        if (this.buff.debuffImmune == true && buff.IsDebuff()) // 1회 디버프무효화가 있고, 들어오는 버프가 디버프일때. 디버프를 무효화한다.
        {
            this.buff.debuffImmune = false;
            return;
        }
        this.buff = buff;
    }
    public void Synergy()
    {
        var weaponlist = equipmentSlot.GetWeaponsList();
        if (weaponlist.Where(x => x.weaponType == WeaponType.sword).Count() == 10)
            Debug.Log("대검 4단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.sword).Count() >= 7)
            Debug.Log("대검 3단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.sword).Count() >= 5)
            Debug.Log("대검 2단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.sword).Count() >= 3)
            Debug.Log("대검 1단계 시너지");

        if (weaponlist.Where(x => x.weaponType == WeaponType.blunt).Count() == 10)
            Debug.Log("둔기 4단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.blunt).Count() >= 7)
            Debug.Log("둔기 3단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.blunt).Count() >= 5)
            Debug.Log("둔기 2단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.blunt).Count() >= 3)
            Debug.Log("둔기 1단계 시너지");

        if (weaponlist.Where(x => x.weaponType == WeaponType.spear).Count() == 10)
            Debug.Log("창 4단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.spear).Count() >= 7)
            Debug.Log("창 3단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.spear).Count() >= 5)
            Debug.Log("창 2단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.spear).Count() >= 3)
            Debug.Log("창 1단계 시너지");

        if (weaponlist.Where(x => x.weaponType == WeaponType.dagger).Count() == 10)
            Debug.Log("단검 4단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.dagger).Count() >= 7)
            Debug.Log("단검 3단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.dagger).Count() >= 5)
            Debug.Log("단검 2단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.dagger).Count() >= 3)
            Debug.Log("단검 1단계 시너지");

        if (weaponlist.Where(x => x.weaponType == WeaponType.wand).Count() == 10)
            Debug.Log("지팡이 4단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.wand).Count() >= 7)
            Debug.Log("지팡이 3단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.wand).Count() >= 5)
            Debug.Log("지팡이 2단계 시너지");
        else if (weaponlist.Where(x => x.weaponType == WeaponType.wand).Count() >= 3)
            Debug.Log("지팡이 1단계 시너지");

        if((weaponlist.Where(x => x.weaponType == WeaponType.sword).Count()==2) && 
            (weaponlist.Where(x => x.weaponType == WeaponType.blunt).Count() == 2)&& 
            (weaponlist.Where(x => x.weaponType == WeaponType.spear).Count() == 2)&& 
            (weaponlist.Where(x => x.weaponType == WeaponType.dagger).Count() == 2)&& 
            (weaponlist.Where(x => x.weaponType == WeaponType.wand).Count() == 2))
            Debug.Log("불쌍해서 주는 추공+2");

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

    public override void TakeHit(float rawDamage) 
    {
        float afterDamage = CalcDamage(rawDamage);
        int damage = Math.Max((int)afterDamage - this.GetStat().defense, 1);
        this.Damage(damage);
    }

    public override float AttackFoe() 
    {
        float finalDamage = this.GetStat().attack;  //TODO : 공격 기믹 추가
        return finalDamage;
    }

    float CalcDamage(float incomingDmg) 
    {
        return incomingDmg;  // TODO : 유물 등의 추가 방어 기믹 추후 추가 
    }

    public void Damage(int damage)
    {
        this.hp = Math.Max(this.hp - damage, 0);
    }

    public void Heal(int amount)
    {
        this.hp = Math.Min(this.hp + amount, this.GetStat().maxHp);
    }
}
