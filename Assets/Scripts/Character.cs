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
    public float critical = 0.00f;
    public float evasion = 0.00f;
    public float hpDrain = 0;       // 흡혈률
    public float stageHpDrain = 0;  // 스테이지 회복량
    public float discount = 0;      // 상점 할인가
    public float rewardCoinPer = 0; // 전투 재화 보상 증가

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
            hpDrain = a.hpDrain + b.hpDrain,
            stageHpDrain = a.stageHpDrain + b.stageHpDrain,
            discount = a.discount + b.discount,
            rewardCoinPer = a.rewardCoinPer + b.rewardCoinPer
        };
    }

    public override string ToString()
    {
        return $"(hp:{maxHp}, atk:{attack}, def:{defense}, spd:{speed})";
    }

    public Stat DeepCopy() => (Stat)this.MemberwiseClone();
}

[System.Serializable]
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
        return $"StatPercent(hp:{maxHp}%, atk:{attack}%, def:{defense}%, spd:{speed}%)";
    }
}

public class SynergyStat 
{
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
            attack = a.attack + b.attack,
            statPercent =a.statPercent + b.statPercent,
        };
    }
    public override string ToString()
    {
        return $"SynergyStat(atk:{attack},stat:{statPercent.ToString()})";
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

    public virtual float ReturnCritAttack(float dmg, bool alwaysCrit) {return 0;} // 데미지에 크리티컬 확률 곱해서 돌려줌

    public virtual void TakeHit(float rawDamage) {} //
}

[System.Serializable]
public class Monster : CharacterBase
{
    public string name;
    public bool isBoss;
    public WorldId worldId;

    public Monster(Stat stat) : base(stat) { }
    public Monster(string name, Stat stat,bool isboss) : base(stat)
    {
        this.name = name;
        this.isBoss = isboss;
    }

    public Monster Spawn()
    {
       return new Monster(this.name, this.baseStat.DeepCopy(),this.isBoss);
    }

    public override void TakeHit(float rawDamage) 
    {
        float afterDamage = CalcDamage(rawDamage);
        var totalDamage = (int)afterDamage - this.GetStat().defense;
        if (totalDamage < 1) 
        {
            hp = hp - 1;
        } 
        else 
        {
            hp = hp - totalDamage;
        }
        GameState.Instance.player.Heal((int)(totalDamage * GameState.Instance.player.GetStat().hpDrain)); //TODO:나중에 요거 코드 위치 바꾸는게 좋을듯함.
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
    public StatBuff buff = new StatBuff();
    EquipmentSlot equipmentSlot = new EquipmentSlot();
    public int money = 100;
    
    public void HpOver()
    {
        if (this.GetStat().maxHp < this.hp)
            this.hp = this.GetStat().maxHp;
    }
    public List<Weapon> GetWeaponList()
    {
       return equipmentSlot.GetWeaponsList();
    }
    public Armor GetArmor()
    {
        return equipmentSlot.GetArmorE();
    }
    public Helmet GetHelmet()
    {
        return equipmentSlot.GetHelmetE();
    }
    public Shoes GetShoes()
    {
        return equipmentSlot.GetShoesE();
    }
    public void SetWeaponList(List<Weapon> weapons)
    {
        equipmentSlot.SetWeaponsList(weapons);
    }
    public void SetEquipment(Artifact artifact)
    {
        equipmentSlot.SetEquipment(artifact);
    }
    public void ResetWeaponList()
    {
        equipmentSlot.ResetWeaponsList();
        foreach (string weaponId in GameConstant.PlayerInitialWeapon)
        {
            SetEquipment(JsonDB.GetWeapon(weaponId));
        }
    }
    public void ResetEquipment()
    {
        foreach (string equipmentId in GameConstant.PlayerInitialEquipment)
        {
            SetEquipment(JsonDB.GetEquipment(equipmentId));
        }
    }
    public int ArtifactsCount()
    {
        return equipmentSlot.ArtifactCount();
    }
    public List<Artifact> GetArtifacts()
    {
       return equipmentSlot.GetArtifacts();
    }
    public void RemoveAtArtifact(int index)
    {
        equipmentSlot.RemoveAtArtifact(index);
    }
    public void ChangeAtArtifact(int index, Artifact artifact)
    {
        equipmentSlot.ChangeAtArtifact(index, artifact);
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
        HpOver();
    }
    public SynergyStat Synergy()
    { 
        var weaponlist = equipmentSlot.GetWeaponsList();
        Dictionary<WeaponType,List<SynergyStat>> dict = new Dictionary<WeaponType, List<SynergyStat>>();
        dict.Add(WeaponType.sword, new List<SynergyStat> 
        { 
            new SynergyStat(2, new StatPercent()), 
            new SynergyStat(5, new StatPercent()), 
            new SynergyStat(10, new StatPercent()), 
            new SynergyStat(15, new StatPercent()) 
        });
        dict.Add(WeaponType.blunt, new List<SynergyStat> 
        { 
            new SynergyStat(0, new StatPercent(0, 0, 0.03f, 0)), 
            new SynergyStat(0, new StatPercent(0, 0, 0.05f, 0)), 
            new SynergyStat(0, new StatPercent(0, 0, 0.1f, 0)), 
            new SynergyStat(0, new StatPercent(0, 0, 0.2f, 0)) 
        });
        dict.Add(WeaponType.spear, new List<SynergyStat> 
        { 
            new SynergyStat(0, new StatPercent(0.03f, 0,0, 0)), 
            new SynergyStat(0, new StatPercent(0.05f, 0, 0, 0)), 
            new SynergyStat(0, new StatPercent(0.1f, 0, 0, 0)), 
            new SynergyStat(0, new StatPercent(0.2f, 0, 0, 0)) 
        });
        dict.Add(WeaponType.dagger, new List<SynergyStat> 
        { 
            new SynergyStat(0, new StatPercent(0, 0, 0, 0.03f)), 
            new SynergyStat(0, new StatPercent(0, 0, 0, 0.05f)), 
            new SynergyStat(0, new StatPercent(0, 0, 0, 0.1f)), 
            new SynergyStat(0, new StatPercent(0, 0, 0, 0.2f)) 
        });
        dict.Add(WeaponType.wand, new List<SynergyStat> 
        { 
            new SynergyStat(0, new StatPercent(0.02f, 0, 0.02f, 0.02f)), 
            new SynergyStat(1, new StatPercent(0.03f, 0, 0.03f, 0.03f)), 
            new SynergyStat(3, new StatPercent(0.05f, 0, 0.05f, 0.05f)), 
            new SynergyStat(8, new StatPercent(0.05f, 0, 0.05f, 0.05f)) 
        });
        int weaponTypeCount;
        int weaponType2Count = 0;
        SynergyStat totalSynergyStat = new SynergyStat();
        foreach (WeaponType weaponType in Enum.GetValues(typeof(WeaponType)))
        {
            if (weaponType != WeaponType.none) //없는경우(맨주먹)는 적용안되게 설정
            {
                weaponTypeCount = weaponlist.Where(x => x.weaponType == weaponType).Count();
                if (weaponTypeCount == 10)
                    totalSynergyStat += dict[weaponType].ElementAt(3);
                else if (weaponTypeCount >= 7)
                    totalSynergyStat += dict[weaponType].ElementAt(2);
                else if (weaponTypeCount >= 5)
                    totalSynergyStat += dict[weaponType].ElementAt(1);
                else if (weaponTypeCount >= 3)
                    totalSynergyStat += dict[weaponType].ElementAt(0);
                else if (weaponTypeCount == 2)
                    weaponType2Count++;
            }
        }
        if (weaponType2Count == 5)
            totalSynergyStat += new SynergyStat(2, new StatPercent());      
        return totalSynergyStat;
    }

    public override Stat GetStat()
    {
        Stat currentstat = this.baseStat + equipmentSlot.GetTotalStat() + buff.GetTotalStat(this.baseStat+ equipmentSlot.GetTotalStat());
        return currentstat;
    }

    public void SetEquipment(Equipment equip)
    {
        equipmentSlot.SetEquipment(equip);
        HpOver();
    }

    public bool BuyItem(Equipment item)
    {

        if (money >= (int)(item.price * (1 - GameState.Instance.player.GetStat().discount)))
        {
            money -= (int)(item.price * (1 - GameState.Instance.player.GetStat().discount));
            this.SetEquipment(item);
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void TakeHit(float rawDamage) 
    {
        int damage = Math.Max((int)rawDamage - this.GetStat().defense, 1);
        
        float evasionRand = UnityEngine.Random.Range(0.0f, 1.0f); //0.0~1.0사이 임의의값

        if(evasionRand < this.GetStat().evasion) //회피하면
        {
            damage = 0; //데미지 0
            Debug.Log("회피함!");
        }

        this.Damage(damage);
    }

    public override float ReturnCritAttack(float rawDamage, bool alwaysCrit) //크리티컬 계산
    {
        if(alwaysCrit == false)
        {
            float critRand = UnityEngine.Random.Range(0.0f, 1.0f); //0.0~1.0사이 임의의값

            if(critRand < this.GetStat().critical) //크리티컬
            {
                Debug.Log("크리티컬! : " + rawDamage * GameConstant.CRITMULTIPLIER);
                return rawDamage * GameConstant.CRITMULTIPLIER;
            }
            else
            {
                return rawDamage;
            }
        }
        else
        {
            Debug.Log("크리티컬! : " + rawDamage * GameConstant.CRITMULTIPLIER);
            return rawDamage * GameConstant.CRITMULTIPLIER;
        }
    }

    public float ReturnCritAttack(float rawDamage) //BattleController에서 받아온 값을 크리확률계산
    {
        return ReturnCritAttack(rawDamage, false);
    }

    public void Damage(int damage)
    {
        this.hp = Math.Max(this.hp - damage, 0);
    }

    public void Heal(int amount)
    {
        this.hp = Math.Min(this.hp + amount, this.GetStat().maxHp);
    }

    public void Dispel()
    {
        if (this.GetBuff().IsDebuff())
            this.buff = new StatBuff();
    }
    public void DispelBuff()
    {
        if (this.GetBuff().IsBuff())
            this.buff = new StatBuff();
    }
}
