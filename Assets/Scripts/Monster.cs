using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : CharacterBase 
{ 
    public Monster(Stat stat) : base(stat) { }
    public string MonsterName; //몬스터 이름
    public bool isBoss; //보스인지 아닌지

    public override void TakeHit(float rawDamage) {
        float afterDamage = rawDamage; // TODO : 추가 몬스터 기믹 추후 추가 
        if(this.baseStat.defense >= rawDamage) {
            nowHp = nowHp - 1;
        } else {
            nowHp = nowHp + this.baseStat.defense - rawDamage;
        }
    }

    public override float AttackFoe() {
        float finalDamage = this.baseStat.attack; //TODO : 공격 기믹 추가
        return finalDamage;
    }
}