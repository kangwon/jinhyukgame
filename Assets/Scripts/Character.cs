using System.Collections;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class Stat
{
    public int maxHp; //최대체력
    public int attack; //공격력
    public int defense; //방어력
    public int speed; //속도
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
    public float hp;
    public bool isMyTurn;
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

    public virtual void TakeHit(float rawDamage) {} // 데미지 받는 함수
    
}

public class Monster : CharacterBase 
{ 
    public Monster(Stat stat) : base(stat) { }
    public string MonsterName; //몬스터 이름
    public bool isBoss; //보스인지 아닌지

    public override void TakeHit(float rawDamage) {
        float afterDamage = rawDamage; // TODO : 추가 몬스터 기믹 추후 추가 
        if(this.baseStat.defense >= rawDamage) {
            hp = hp - 1;
        } else {
            hp = hp + this.baseStat.defense - rawDamage;
        }
    }

    public override float AttackFoe() {
        float finalDamage = this.baseStat.attack; //TODO : 공격 기믹 추가
        return finalDamage;
    }

}

public class Player : CharacterBase
{
    List<Stat> buffs = new List<Stat>();
    List<Stat> items = new List<Stat>();

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

    public override void TakeHit(float rawDamage) {
        float afterDamage = rawDamage; // TODO : 유물 등의 추가 방어 기믹 추후 추가 
        if(this.baseStat.defense >= afterDamage) {
            hp = hp - 1;
        } else {
            hp = hp + this.baseStat.defense - afterDamage;
        }
    }

    public override float AttackFoe() {
        float finalDamage = this.baseStat.attack;  //TODO : 공격 기믹 추가
        return finalDamage;
    }
}
