using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterBase : MonoBehaviour
{
    public Stat baseStat;
    public bool isMyTurn;
    public bool isDead
    {
        get => baseStat.nowHp <= 0;
    }
    
    public CharacterBase(Stat stat)
    {
        this.baseStat = stat;
        //this.nowHp = this.baseStat.maxHp; //인스펙터에서 설정
    }

    public virtual Stat GetStat()
    {
        return baseStat;
    }

    public virtual float AttackFoe() {return 0;} // 공격하는 함수

    public virtual void TakeHit(float rawDamage) {} // 데미지 받는 함수
    
}