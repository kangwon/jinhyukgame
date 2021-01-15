using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{

    Player player;
    Monster monster;

    enum BattleState {
        Idle,//기본 상태
        Gauging, //게이지 채우는 중
        PlayerTurn, //플레이어의 턴
        MonsterTurn, //몬스터의 턴
        Terminated// 배틀 종료
    }

    private BattleState battleState;

    void Awake() 
    {
        player = GameState.Instance.player;

        if(player == null || monster == null) {
            Debug.Log("No game objects found!");
        } else {
            battleState = BattleState.Idle;
        }
    }

    void OnEnable() 
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBattleStates(); //s 주의
    }

    void UpdateBattleStates() {
        
    }
}
