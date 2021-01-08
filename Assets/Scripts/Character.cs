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

public class StatPercent
{
    public float maxHp;
    public float attack;
    public float defense;
    public float speed;
    
    public StatPercent() 
    {
       maxHp=0;
       attack=0;
       defense=0;
       speed=0;
    }
    public StatPercent(float maxHp, float attack, float defense, float speed)
    {
        this.maxHp = maxHp;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
    }
    public static StatPercent operator +(StatPercent a, StatPercent b)
    {
        return new StatPercent
        {
            maxHp = a.maxHp + b.maxHp,
            attack = a.attack + b.attack,
            defense = a.defense + b.defense,
            speed = a.speed + b.speed,
        };
    }
    public override string ToString()
    {
        return $"Stat(hp:{maxHp}%, atk:{attack}%, def:{defense}%, spd:{speed}%)";
    }
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

        if (this.buff.debuffImmune == true || buff.IsDebuff()) // 1회 디버프무효화가 있고, 들어오는 버프가 디버프일때. 디버프를 무효화한다.
        {
            this.buff.debuffImmune = false;
            return;
        }
        this.buff = buff;
    }
    public class SynergyStat {
        public int attack;
        public StatPercent statPercent;
        public SynergyStat() 
        {
            attack = 0;
            statPercent = new StatPercent();
        }
        public SynergyStat(int x, StatPercent y)
        {
            attack = x;
            statPercent = y;
        }
        public static SynergyStat operator+(SynergyStat a, SynergyStat b)
        {
            return new SynergyStat
            { 
                attack = a.attack+b.attack,
                statPercent =a.statPercent+b.statPercent,
            };
        }
        public override string ToString()
        {
            return $"(atk:{attack},stat:{statPercent})";
        }
    }
    public void Synergy()
    { 
        var weaponlist = equipmentSlot.GetWeaponsList();
        Dictionary<WeaponType,List<SynergyStat>> dict = new Dictionary<WeaponType, List<SynergyStat>>();
        dict.Add(WeaponType.sword, new List<SynergyStat> { new SynergyStat(2, new StatPercent()), new SynergyStat(5, new StatPercent()), new SynergyStat(10, new StatPercent()), new SynergyStat(15, new StatPercent()) });
        dict.Add(WeaponType.blunt, new List<SynergyStat> { new SynergyStat(0, new StatPercent(0,0,0.03f,0)), new SynergyStat(0, new StatPercent(0, 0, 0.05f, 0)), new SynergyStat(0, new StatPercent(0, 0, 0.1f, 0)), new SynergyStat(0, new StatPercent(0, 0, 0.2f, 0)) });
        dict.Add(WeaponType.spear, new List<SynergyStat> { new SynergyStat(0, new StatPercent(0.03f, 0,0, 0)), new SynergyStat(0, new StatPercent(0.05f, 0, 0, 0)), new SynergyStat(0, new StatPercent(0.1f, 0, 0, 0)), new SynergyStat(0, new StatPercent(0.2f, 0, 0, 0)) });
        dict.Add(WeaponType.dagger, new List<SynergyStat> { new SynergyStat(0, new StatPercent(0, 0, 0,0.03f)), new SynergyStat(0, new StatPercent(0, 0,0, 0.05f)), new SynergyStat(0, new StatPercent(0, 0,0, 0.1f)), new SynergyStat(0, new StatPercent(0, 0,0, 0.2f)) });
        dict.Add(WeaponType.wand, new List<SynergyStat> { new SynergyStat(0, new StatPercent(0.02f, 0, 0.02f, 0.02f)), new SynergyStat(1, new StatPercent(0.03f, 0, 0.03f, 0.03f)), new SynergyStat(3, new StatPercent(0.05f, 0, 0.05f, 0.05f)), new SynergyStat(8, new StatPercent(0.05f, 0, 0.05f, 0.05f)) });
        int weaponTypeCount;
        int weaponType2Count = 0;
        SynergyStat totalSynergyStat = new SynergyStat(0, new StatPercent());
        for(WeaponType i = WeaponType.sword; i <= WeaponType.wand; i++)
        {
            weaponTypeCount = weaponlist.Where(x => x.weaponType == i).Count();
            if (weaponTypeCount == 10)
                totalSynergyStat += dict[i].ElementAt(3);
            else if (weaponTypeCount >= 7)
                totalSynergyStat+= dict[i].ElementAt(2);
            else if (weaponTypeCount >= 5)
                totalSynergyStat += dict[i].ElementAt(1);
            else if (weaponTypeCount >= 3)
                totalSynergyStat += dict[i].ElementAt(0);
            else if (weaponTypeCount == 2)
                weaponType2Count++;
        }
        if (weaponType2Count == 5)
            totalSynergyStat += new SynergyStat(2, new StatPercent());
        Debug.Log(totalSynergyStat);
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
        if(this.GetStat().defense >= afterDamage) 
        {
            hp = hp - 1;
        } 
        else 
        {
            hp = hp + this.GetStat().defense - (int)afterDamage;
        }
        //Debug.Log($"이제 플레이어 피 : {hp}임.");
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
}
