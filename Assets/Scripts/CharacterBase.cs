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
        //this.nowHp = this.baseStat.maxHp; //�ν����Ϳ��� ����
    }

    public virtual Stat GetStat()
    {
        return baseStat;
    }

    public virtual float AttackFoe() {return 0;} // �����ϴ� �Լ�

    public virtual void TakeHit(float rawDamage) {} // ������ �޴� �Լ�
    
}