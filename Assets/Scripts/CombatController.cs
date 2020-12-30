using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    Player player;
    Monster monster;

    public const int SpeedGauge = 1000;

    private int playerGauge = 0;
    private int monsterGauge = 0;

    public enum combatState {
        Dead, //죽음
        Idle, //게이지 채우는 중
        Turn, //현재 턴
        Unable // IDEA : 스턴 등의 행동불가
    }

    private combatState playerState = combatState.Idle;
    private combatState monsterState = combatState.Idle;


/*---------상태 변경하며 전투 진행---------*/

    public void UpdateState() {
        if(playerState == combatState.Idle && monsterState == combatState.Idle) { //둘 다 게이지 채우는 중일 때
            SpeedUntilTurn(player, monster); //행동게이지 증가, 둘 중 하나의 combatState가 Turn으로 변경.
        }

        if(playerState == combatState.Turn) { //player의 Turn 이 왔을 때
            CombatPhase(player, monster); //때리고 맞고
            playerState = combatState.Idle; //다시 게이지 채우는 중으로
        } 

        else if (monsterState == combatState.Turn) { //monster의 Turn 이 왔을 때
            CombatPhase(monster, player);
            monsterState = combatState.Idle;
        }

        if(playerState == combatState.Dead || monsterState == combatState.Dead) {
            EndCombat();
        }
    }

/*---------공격 주고받기---------*/

    public void CombatPhase(CharacterBase attacker, CharacterBase defender) {// TODO : call-by-ref로 넘겨야할거같음 IDEA : C# Delegate 여기 사용가능?
        float Dmg = attacker.AttackFoe();
        defender.TakeHit(Dmg);

        if(player.isDead) {
            playerState = combatState.Dead;
        }

        if(monster.isDead) {
            monsterState = combatState.Dead;
        }        
    }

/*---------어느 한 쪽의 게이지가 1000이 될 때까지 진행---------*/
    public void SpeedUntilTurn(Player p, Monster m) { 
        //UpdateGaugeImage() // TODO : 스피드게이지의 이미지 조정
        
        float totalElapsedTime = 0.0f;
        
        while(playerGauge <= SpeedGauge && monsterGauge <= SpeedGauge) { //둘다 행동게이지가 최대 게이지에 이르지 못했을때
            playerGauge += (int)(p.GetStat().speed * Time.deltaTime); //흐른 시간만큼 속도에 곱해 게이지를 채움
            monsterGauge += (int)(m.GetStat().speed * Time.deltaTime);
            totalElapsedTime += Time.deltaTime; // IDEA : 추후에 쓰일 걸릴시간?
        }

        if(playerGauge > monsterGauge) {
            playerState = combatState.Turn;
        } else if(playerGauge < monsterGauge) {
            monsterState = combatState.Turn;
        } else {
            //동시에 행동게이지 1000?
        }

    }

/*----------전투 끝(정산 등?)----------*/

    public void EndCombat() {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }
}
