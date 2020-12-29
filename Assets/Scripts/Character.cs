using System.Collections;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class Stat
{
    public int maxHp;
    public int attack;
    public int defense;
    public int speed;
    public int startSpeedGauge;

    public static Stat operator +(Stat a, Stat b)
    {
        return new Stat
        {
            maxHp = a.maxHp + b.maxHp,
            attack = a.attack + b.attack,
            defense = a.defense + b.defense,
            speed = a.speed + b.speed,
            startSpeedGauge = a.startSpeedGauge + b.startSpeedGauge
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
}

public class Monster : CharacterBase 
{ 
    public Monster(Stat stat) : base(stat) { }
}

public class Player : CharacterBase
{
    List<Stat> buffs = new List<Stat>();
    EquipmentSlot equipmentSlot = new EquipmentSlot();

    public Player(Stat stat) : base(stat) { }

    public void AddBuff(Stat buff)
    {
        buffs.Add(buff);
    }

    public override Stat GetStat()
    {
        Stat currentstat = this.baseStat;
        currentstat = buffs.Aggregate(currentstat, (stat, buff) => stat + buff);
        currentstat = items.Aggregate(currentstat, (stat, buff) => stat + buff);
        return currentstat;
    }
}
