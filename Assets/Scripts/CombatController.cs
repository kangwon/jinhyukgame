using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public bool isBattle = false;

    public Player player;
    public Monster monster;

    public const int MaxSpeedGauge = 1000;

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
            SpeedUntilTurn(); //행동게이지 증가, 둘 중 하나의 combatState가 Turn으로 변경.
            Debug.Log("빠져나왔대");
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

    public void CombatPhase(CharacterBase attacker, CharacterBase defender) {//IDEA : C# Delegate 여기 사용가능?
        float Dmg = attacker.AttackFoe();
        //Debug.Log("attacker hp : " + attacker.nowHp);
        defender.TakeHit(Dmg);

        if(player.isDead) {
            playerState = combatState.Dead;
        }

        if(monster.isDead) {
            monsterState = combatState.Dead;
        }        
    }

/*---------어느 한 쪽의 게이지가 1000이 될 때까지 진행---------*/
    public void SpeedUntilTurn() { 

        float totalElapsedTime = 0.0f;
        while(playerGauge <= MaxSpeedGauge && monsterGauge <= MaxSpeedGauge) { //둘다 행동게이지가 최대 게이지에 이르지 못했을때
            
            playerGauge += (int)(player.baseStat.speed * Time.deltaTime); //흐른 시간만큼 속도에 곱해 게이지를 채움
            
            //Debug.Log("playerGauge is :" + playerGauge);
            monsterGauge += (int)(monster.baseStat.speed * Time.deltaTime);
            totalElapsedTime += Time.deltaTime; // IDEA : 추후에 쓰일 걸릴시간?
        }
        Debug.Log("3");
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
        isBattle = false;
        Debug.Log("Battle Done!");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("CombatController called!");
        if(player != null) {
            playerGauge = player.baseStat.startSpeedGauge; //TODO : baseStat을 getStat()으로 바꾸기. 지금 Aggregate에서 ArgumentNullException.
        } else {
            Debug.Log("No game object player found!");
        }
       
        if(monster != null) {
            monsterGauge = monster.baseStat.startSpeedGauge;
        } else {
            Debug.Log("No game object monster found!");
        }   
    }

    // Update is called once per frame
    void Update()
    {
        if(isBattle) {
            Debug.Log("Battle Start!");
            UpdateState();
        }
    }
}
