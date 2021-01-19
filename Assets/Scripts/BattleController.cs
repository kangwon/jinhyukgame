using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{

    Player player;
    
    public MonsterCard MonsterCard;
    public Monster monster;

    BattlePlayerAttackPanelController BattlePanel;

    public int GAUGE_SIZE = 200;
    public float playerGauge, monsterGauge;

    public int gaugeQueueCount = 0;

    enum BattleState {
        Sleeping, //시작 안함
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
        BattlePanel = GameObject.Find("BattlePlayerAttackPanel").GetComponent<BattlePlayerAttackPanelController>();
    }

    void OnEnable() 
    {
        player = GameState.Instance.player;
        battleState = BattleState.Started;
    }

    // Update is called once per frame
    void Update()
    {
        switch(battleState) 
        {
            case BattleState.Started:
            BattleInit();
            Debug.Log("1");
            break;

            case BattleState.Waiting:
            SpeedUntilTurn();
            Debug.Log("2");
            break;

            case BattleState.PlayerTurn:
            PlayerAttack();
            Debug.Log("3");
            break;

            case BattleState.MonsterTurn:
            MonsterAttack();
            Debug.Log("4");
            break;

            case BattleState.TurnDone:
            TurnFinish();
            Debug.Log("5");
            break;

            case BattleState.BattleOver:
            Debug.Log("Battle Done!");
            BattleFinish();
            break;
        }
    }

    public void BattleInit()
    {
        monster = MonsterCard?.monster;

        if(player == null) {
            Debug.Log("플레이어 로딩 안됨.");
        }
        else if(monster == null) {
            Debug.Log("몬스터 로딩 안됨.");
        } else {
            playerGauge = player?.GetStat().startSpeedGauge ?? 0;
            //Debug.Log($"플레이어 게이지 : {playerGauge}");
            monsterGauge = monster?.GetStat().startSpeedGauge ?? 0;
            //Debug.Log($"몬스터 게이지 : {monsterGauge}");
        }

        battleState = BattleState.Waiting;
    }

    public void SpeedUntilTurn()
    {
        while(playerGauge < GAUGE_SIZE && monsterGauge < GAUGE_SIZE) { //둘다 행동게이지가 최대 게이지에 이르지 못했을때
            playerGauge += (player.GetStat().speed * 0.1f);
            //Debug.Log($"플레이어 게이지 : {playerGauge}");
            monsterGauge += (monster.GetStat().speed * 0.1f);
        }

        if(monsterGauge >= GAUGE_SIZE) 
        {
            battleState = BattleState.MonsterTurn; //몬스터 턴으로
            //Debug.Log($"몬스터 게이지 : {monsterGauge}");
            gaugeQueueCount++;
        }

        if(playerGauge >= GAUGE_SIZE) 
        {
            battleState = BattleState.PlayerTurn; //플레이어 턴으로
            //Debug.Log($"플레이어 게이지 : {playerGauge}");
            gaugeQueueCount++;
        }
    }

    public void PlayerAttack()
    {
        if(BattlePanel.OnClickAttackPressed)
        {
            Stat tempStat = new Stat();
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
