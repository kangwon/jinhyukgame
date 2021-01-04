using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public bool isBattle = false; //private으로 숨기기?

    Player player = GameState.Instance.player;

    Monster monster = new Monster(new Stat() 
    { //보스 1의 스탯. 추후에 몬스터 리스트 받아오기로.
        maxHp = 120,
        attack = 6,
        defense = 10,
        speed = 15}
    );

    public const float MaxSpeedGauge = 200.0f; //스피드게이지 최댓값

    private float playerGauge, monsterGauge; //플레이어와 몬스터의 현재 스피드게이지

    public enum combatState {
        Dead, //죽음
        Idle, //게이지 채우는 중
        Turn, //현재 턴
        Unable // IDEA : 스턴 등의 행동불가
    }

    private combatState playerState, monsterState;


/*---------상태 변경하며 전투 진행---------*/

    public void UpdateState() {
        if(playerState == combatState.Idle && monsterState == combatState.Idle) { //둘 다 게이지 채우는 중일 때
            SpeedUntilTurn(); //행동게이지 증가, 둘 중 하나의 combatState가 Turn으로 변경.
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
        defender.TakeHit(Dmg);
        //Debug.Log("defender hp : " + attacker.nowHp);

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
        while(playerGauge < MaxSpeedGauge && monsterGauge < MaxSpeedGauge) { //둘다 행동게이지가 최대 게이지에 이르지 못했을때
            
            playerGauge += (player.GetStat().speed * Time.deltaTime); //흐른 시간만큼 속도에 곱해 게이지를 채움
            
            Debug.Log("playerGauge is :" + playerGauge);

            monsterGauge += (monster.GetStat().speed * Time.deltaTime);

             Debug.Log("monsterGauge is :" + monsterGauge);

            totalElapsedTime += Time.deltaTime; // IDEA : 추후에 쓰일 걸릴시간?
        }

        if(playerGauge >= MaxSpeedGauge) {
            playerGauge -= MaxSpeedGauge;
            playerState = combatState.Turn;
        }
        if(monsterGauge >= MaxSpeedGauge) {
            monsterGauge -= MaxSpeedGauge;
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
        if(player != null && monster != null) {
            isBattle = true;
            playerState = combatState.Idle;
            monsterState = combatState.Idle;
            playerGauge = player.GetStat().startSpeedGauge;
            monsterGauge = monster.GetStat().startSpeedGauge;
        } else {
            Debug.Log("No game objects found!");
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

    public float getPlayerSpeedGaugeVal() {
        return playerGauge;
    }

    public float getMonsterSpeedGaugeVal() {
        return monsterGauge;
    }
}
