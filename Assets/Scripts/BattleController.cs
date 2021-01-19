using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{

    Player player;
    Monster monster;

    BattlePlayerAttackPanelController BattlePanel;

    public float GAUGE_SIZE = 200.0f;
    public float playerGauge, monsterGauge;

    public int gaugeQueueCount = 0;

    enum BattleState {
        Started, //시작함
        Waiting, //대기 상태(게이지 채우는 중)
        PlayerTurn, //플레이어의 턴
        MonsterTurn, //몬스터의 턴
        TurnDone, //누군가의 턴이 끝남
        BattleOver// 배틀 종료
    }

    private BattleState battleState;

    void Awake() 
    {
        player = GameState.Instance.player;

        BattlePanel = GameObject.Find("BattlePlayerAttackPanel").GetComponent<BattlePlayerAttackPanelController>();
    }

    void OnEnable() 
    {
        if(player == null || monster == null) 
        {
            Debug.Log("No game objects found!");
        } else {
            PlayerMonsterInit();
            battleState = BattleState.Started;
        }
    }

    void PlayerMonsterInit() 
    {
        monster = BattlePanel.monster;
        playerGauge = player?.GetStat().startSpeedGauge ?? 0;
        monsterGauge = monster?.GetStat().startSpeedGauge ?? 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(battleState) 
        {
            case BattleState.Started:
            Debug.Log("new battle started!");
            battleState = BattleState.Waiting;
            break;

            case BattleState.Waiting:
            StartCoroutine(SpeedTilTurn());
            break;

            case BattleState.PlayerTurn:
            PlayerAttack();
            break;

            case BattleState.MonsterTurn:
            MonsterAttack();
            break;

            case BattleState.TurnDone:
            TurnFinish();
            break;

            case BattleState.BattleOver:
            Debug.Log("Battle Done!");
            BattleFinish();
            break;
        }
    }

    IEnumerator SpeedTilTurn()
    {
        while(playerGauge < GAUGE_SIZE && monsterGauge < GAUGE_SIZE) { //둘다 행동게이지가 최대 게이지에 이르지 못했을때
            playerGauge += (player.GetStat().speed * Time.deltaTime); //흐른 시간만큼 속도에 곱해 게이지를 채움
            monsterGauge += (monster.GetStat().speed * Time.deltaTime);
            yield return null;
        }

        if(monsterGauge >= GAUGE_SIZE) 
        {
            battleState = BattleState.MonsterTurn; //몬스터 턴으로
            Debug.Log("몬스터턴");
        }

        if(playerGauge >= GAUGE_SIZE) 
        {
            battleState = BattleState.PlayerTurn; //플레이어 턴으로
            Debug.Log("플레이어턴");
        }
    }

    public void PlayerAttack()
    {
        if(BattlePanel.OnClickAttackPressed)
        {
            Stat tempStat = new Stat();
            Debug.Log($"카드 총합 데미지:{BattlePanel.cardDamageSum}, 플레이어 공격력:{player.GetStat().attack}");
            tempStat.attack = BattlePanel.cardDamageSum + player.GetStat().attack;
            monster.TakeHit((tempStat.attack+player.Synergy().attack)*(1f+BattlePanel.comboPercentSum));
            
            playerGauge = 0; //게이지 소비(초기화)

            BattlePanel.OnClickAttackPressed = false; // 버튼 bool 다시 초기화.
            battleState = BattleState.TurnDone;
        }
    }

    public void MonsterAttack()
    {
        int dmg = (int)monster.AttackFoe();
        player.TakeHit(dmg);

        monsterGauge = 0;

        battleState = BattleState.TurnDone;
    }

    public void TurnFinish() 
    {
        gaugeQueueCount++;

        if(player.isDead || monster.isDead) 
        {
            battleState = BattleState.BattleOver;
        } else {
            battleState = BattleState.Waiting;
        }
    }

    public void BattleFinish()
    {
        if(player.isDead) 
        {
            BattlePanel.ShowGameOver();
        }

        if(monster.isDead) 
        {
            BattlePanel.RewardStage();
        }    
    }
}
