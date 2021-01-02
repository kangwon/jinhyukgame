using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
            nowHp = nowHp - 1;
        } else {
            nowHp = nowHp + this.baseStat.defense - afterDamage;
        }
    }

    public override float AttackFoe() {
        float finalDamage = this.baseStat.attack;  //TODO : 공격 기믹 추가
        return finalDamage;
    }
}
