using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{

    Player player;
    
    public MonsterCard MonsterCard;
    public Monster monster;

    BattlePlayerAttackPanelController BattlePanel;
    SpeedGaugeUI SGUI;

    public int GAUGE_SIZE = 200;
    public float playerGauge, monsterGauge;

    public bool isPlayersFirstTurn; // firstAttackCritical = false; 관련 첫 번째 턴 
    public bool isMonstersFirstTurn; // firstDamagedImmune = false; 첫번째 피격 데미지 무시 관련

    public bool hp1Undied; //bool hp1Left 전투마다 체력1에서 버티기 관련

    public int playerTurnCount = 0; // 플레이어가 턴 몇번 진행했는지 계산

    public int battleNumber = 0; //몇 번째 배틀인지. 나서스 q데미지 여기서하면될듯?

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
        SGUI = GameObject.Find("SpeedGaugeUI").GetComponent<SpeedGaugeUI>();

        if(SGUI == null) {
            Debug.Log("SGUI not found!");
        }
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
            break;

            case BattleState.Waiting:
            SpeedUntilTurn();
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
            monsterGauge = monster?.GetStat().startSpeedGauge ?? 0;
        }

        isPlayersFirstTurn = true;
        isMonstersFirstTurn = true;

        hp1Undied = false;

        playerTurnCount = 0; // 초기화
        battleNumber++;

        battleState = BattleState.Waiting;
    }

    public void SpeedUntilTurn()
    {
        Stat tempStat = new Stat();

        float hpSpeedMultiplier = 1.0f;
        
        if(player.GetBuff().lostHpRatioSpeedUp != 0)
        {
            hpSpeedMultiplier += (1 - player.hp / player.GetStat().maxHp) * player.GetBuff().lostHpRatioSpeedUp * 100.0f; // 잃은 체력 비율 * 1퍼당 올라가는 속도율
        }

        while(playerGauge < GAUGE_SIZE && monsterGauge < GAUGE_SIZE) { //둘다 행동게이지가 최대 게이지에 이르지 못했을때
            playerGauge += (player.GetStat().speed * Time.deltaTime) * hpSpeedMultiplier;
            monsterGauge += (monster.GetStat().speed * Time.deltaTime);
        }

        if(playerGauge >= GAUGE_SIZE) 
        {
            battleState = BattleState.PlayerTurn; //플레이어 턴으로
            SGUI.QueueSpeedGauge();
        }

        else if(monsterGauge >= GAUGE_SIZE) 
        {
            battleState = BattleState.MonsterTurn; //몬스터 턴으로
            SGUI.QueueSpeedGauge();
        }
    }

    public void PlayerAttack()
    {
        if(BattlePanel.OnClickAttackPressed)
        {
            playerTurnCount++; //플레이어 진행 턴에 1카운트 추가

            Stat tempStat = new Stat();
            tempStat.attack = BattlePanel.cardDamageSum;
            
            float finalAttack;

            if(player.GetBuff().lostHpRatioDamageUp != 0) // 잃은 체력 비례 데미지 증가가 있다면
            {
                tempStat.attack += (int)((1.00f - player.hp / player.GetStat().maxHp) * player.GetBuff().lostHpRatioDamageUp * 100.0f * player.GetStat().attack);
            }
            else
            {
                tempStat.attack += player.GetStat().attack;
            }

            if (!GameState.Instance.player.GetBuff().iCantUsedCombo) //콤보 불가 디버프시
            {
                finalAttack = (tempStat.attack + player.Synergy().attack) * (1f + BattlePanel.comboPercentSum);
            }
            else
            {
                finalAttack = tempStat.attack + player.Synergy().attack;
            }

            //여기까지 finalAttack은 기본적으로 플레이어 기본공 + 콤보 + 시너지

            finalAttack = player.ReturnCritAttack(finalAttack , player.GetBuff().firstAttackCritical); //첫타격 확정치명타 관련

            if(monster.isBoss && (player.GetBuff().bossAddDamage != 0)) //몬스터가 보스고 보스전 추가 뎀이 있을경우
            {
                finalAttack = finalAttack * (1 + player.GetBuff().bossAddDamage);
            }

            if(player.GetBuff().enemyHpPercentDamage != 0) //상대 체력 퍼센트 추가 고정 뎀이 있을경우
            {
                if(monster.isBoss) //보스면 뎀감해서 적용
                {
                    float BOSSPERCENTDMGDIVIDEND = 0.5f;
                    finalAttack += monster.hp * player.GetBuff().enemyHpPercentDamage * BOSSPERCENTDMGDIVIDEND;
                }
                else
                {
                    finalAttack += monster.hp * player.GetBuff().enemyHpPercentDamage;
                }
            }

            if(player.GetBuff().attack3AddDamage != 0 && playerTurnCount % 3 == 0) //3번마다 추가 고정데미지가 있다면
            {
                finalAttack += player.GetBuff().attack3AddDamage; //3번마다 추가 고정데미지
            }

            if(player.GetBuff().checkNasusQ) //잡은몬스터 * n 만큼 추가데미지
            {
                finalAttack += player.GetBuff().nasusQ * battleNumber;
            }

            monster.TakeHit(finalAttack);

          //  player.Heal((int)((tempStat.attack + player.Synergy().attack) * (1f + BattlePanel.comboPercentSum) * player.GetStat().hpDrain));
           
            Debug.Log($"최종데미지 : {finalAttack}");

            if(player.GetBuff().doubleAttackPercent != 0) //확률적으로 턴 두번 진행 유물이 있을경우
            {
                float doubleAttackRand = UnityEngine.Random.Range(0.0f, 1.0f); //턴 추가 진행확률
                if(doubleAttackRand < player.GetBuff().doubleAttackPercent)
                {
                    //게이지 변함없음
                }
                else
                {
                    playerGauge = 0; //게이지 소비(초기화)
                }
            }
            else
            {
                playerGauge = 0; //게이지 소비(초기화)
            }

            BattlePanel.OnClickAttackPressed = false; // 버튼 bool 다시 초기화.

            isPlayersFirstTurn = false;
            
            battleState = BattleState.TurnDone;
        }
    }

    public void MonsterAttack()
    {
        float rawMonterInflictingdmg = monster.AttackFoe();
        float finalMonsterDmg = rawMonterInflictingdmg;

        if(monster.isBoss && (player.GetBuff().bossDamageDecrease != 0)) //몬스터가 보스고 보스전 피격데미지 감소 유물일경우
        {
            finalMonsterDmg = (1 - player.GetBuff().bossDamageDecrease) * rawMonterInflictingdmg;
        }
        
        if(isMonstersFirstTurn && player.GetBuff().firstDamagedImmune) //몬스터 첫타격이고 첫번째 피격 데미지 무시 유물일경우
        {
            finalMonsterDmg = 0;
        }

        player.TakeHit((int)finalMonsterDmg);

        if(player.GetBuff().reflectionDamage != 0) //플레이어가 반사데미지 있을경우
        {
            monster.TakeHit(rawMonterInflictingdmg * player.GetBuff().reflectionDamage);
        }

        monsterGauge = 0;

        isMonstersFirstTurn = false;
        battleState = BattleState.TurnDone;
    }

    public void TurnFinish() 
    {
        if(player.isDead) 
        {
            if (GameState.Instance.player.GetBuff().hp1Left && !hp1Undied) //전투마다 피1에서 버티기
            {
                player.hp = 1;
                hp1Undied = true;
                battleState = BattleState.Waiting;
            }
            else if(GameState.Instance.player.GetBuff().resurrection) //죽으면 부활
            {
                int healAmount = player.GetStat().maxHp / 2;
                player.Heal(player.GetStat().maxHp);

                player.buff.resurrection = false; //RemoveBuff 메소드 추가?
                //아티팩트 remove로 해야할거같음
                battleState = BattleState.Waiting;
            }
            else
            {
                battleState = BattleState.BattleOver;
            }

        } 
        else if(monster.isDead)
        {
            battleState = BattleState.BattleOver;
        } 
        else 
        {
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
            if(player.GetBuff().battleEndHealPercent != 0) //전투 종료시 체력 퍼센트 회복
            {
                player.Heal((int)(player.GetStat().maxHp * player.GetBuff().battleEndHealPercent));
            }

            if(player.GetBuff().rewardBonusPercent != 0)
            {
                float additonalRewardRand = UnityEngine.Random.Range(0.0f, 1.0f); //0.0~1.0사이 임의의값

                if(additonalRewardRand < player.GetBuff().rewardBonusPercent) //추가보상
                {
                    BattlePanel.RewardStage(2);
                }
                else
                {
                    BattlePanel.RewardStage();
                }
            }
            else
            {
                BattlePanel.RewardStage();
            }

            AchievementManager.BeatMonster(monster);
        }    
    }
}
