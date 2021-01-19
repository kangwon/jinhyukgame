using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedGaugeUI : MonoBehaviour //BattlePlayerAttackPanel의 child
{
    BattleController battleController;
    
    [HideInInspector]
    public RectTransform rectTran;
    [HideInInspector]
    public Image PlayerSpeedGaugeImage, MonsterSpeedGaugeImage;

    private float SpeedGaugeWidth, SpeedGaugeHeight; //스피드게이지 UI의 넓이와 길이(높이) 현재 30과 700.

    /*---*/

    [HideInInspector]
    public struct SpeedGaugeValues {
        public float GaugeRatio; //현재 게이지상에서의 비율(0.0~1.0)
        public Vector3 PrevPos; //재생하기 이전 위치
        public Vector3 NowPos; //도달해야 할 위치
    }

    public SpeedGaugeValues initSpeedGaugeValue = new SpeedGaugeValues();

    public Queue<SpeedGaugeValues> PlayerQueue = new Queue<SpeedGaugeValues>();
    public Queue<SpeedGaugeValues> MonsterQueue = new Queue<SpeedGaugeValues>();

    private bool playerGaugeUpdateFinished, monsterGaugeUpdateFinished;
    private bool isPlayerTrashQueue, isMonsterTrashQueue;

    private int howManyUpdates;

    float speed = 0.02f;

    /*---*/

    void Awake() {
        battleController = this.transform.parent.GetComponent<BattleController>();

        rectTran = this.GetComponent<RectTransform>();

        PlayerSpeedGaugeImage = rectTran.transform.Find("PlayerGauge").GetComponent<Image>();
        MonsterSpeedGaugeImage = rectTran.transform.Find("MonsterGauge").GetComponent<Image>();

        SpeedGaugeWidth = rectTran.rect.width;
        SpeedGaugeHeight = rectTran.rect.height;
    }

    //활성화마다 실행
    void OnEnable()
    {
        initSpeedGaugeValue.GaugeRatio = 0;
        initSpeedGaugeValue.PrevPos = new Vector3(0,SpeedGaugeHeight/2,0); //가장 위로 초기화
        initSpeedGaugeValue.NowPos = new Vector3(0,SpeedGaugeHeight/2,0); //가장 위로 초기화

        PlayerQueue.Enqueue(initSpeedGaugeValue);
        MonsterQueue.Enqueue(initSpeedGaugeValue);

        PlayerSpeedGaugeImage.transform.localPosition = new Vector3(0,SpeedGaugeHeight/2,0); //가장 위로 초기화
        MonsterSpeedGaugeImage.transform.localPosition = new Vector3(0,SpeedGaugeHeight/2,0); //가장 위로 초기화

        isPlayerTrashQueue = true; //첫번째 queue는 trash
        isMonsterTrashQueue = true;

        playerGaugeUpdateFinished = true;
        monsterGaugeUpdateFinished = true;

        howManyUpdates = 0;
    }

    void OnDisable() 
    {
        PlayerQueue.Clear();
        MonsterQueue.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if((howManyUpdates >= 1) && playerGaugeUpdateFinished && monsterGaugeUpdateFinished) { //큐에 남은 목록이 있고 재생이 끝남
            
            StartCoroutine(UpdatePlayerSpeedGaugeAnim(speed));
            StartCoroutine(UpdateMonsterSpeedGaugeAnim(speed));

            if(playerGaugeUpdateFinished) //한번이상 업데이트함
            {
                isPlayerTrashQueue = false;
            }

            if(monsterGaugeUpdateFinished)
            {
                isMonsterTrashQueue = false;
            }

            howManyUpdates--;
        }
    }

    public void QueueSpeedGauge() { //battleController에서 턴이 진행될때 마다 queue에 차곡차곡 넣어주기

        /*---------플레이어-----------*/
        SpeedGaugeValues PlayerSpeedGaugeValue = new SpeedGaugeValues();

        float PGRatio = battleController.playerGauge / battleController.GAUGE_SIZE;//현재 플레이어가 턴까지의 비율을 계산
        PGRatio = RoundTrimRatio(PGRatio); //소수 두자리수까지 round, 0이하 1이상 다듬기
        PlayerSpeedGaugeValue.GaugeRatio = PGRatio;

        PlayerSpeedGaugeValue.PrevPos = PlayerQueue.Peek().NowPos; // 현재 계산할 위치의 이전 위치는 이전 위치의 현재 위치.

        if(PlayerQueue.Peek().NowPos == new Vector3(0,-SpeedGaugeHeight/2,0)) { // 가장 아래라면
            PlayerSpeedGaugeValue.PrevPos = new Vector3(0,SpeedGaugeHeight/2,0);// 가장 위로 초기화
        }

        Vector3 PPos = new Vector3(0,SpeedGaugeHeight/2 - PGRatio * SpeedGaugeHeight,0); //현재 위치를 계산
        PlayerSpeedGaugeValue.NowPos = PPos;

        PlayerQueue.Enqueue(PlayerSpeedGaugeValue); //계산한 현재 위치를 큐에 넣음

        /*----------몬스터----------*/
        SpeedGaugeValues MonsterSpeedGaugeValue = new SpeedGaugeValues();

        float MGRatio = battleController.monsterGauge / battleController.GAUGE_SIZE;
        MGRatio = RoundTrimRatio(MGRatio);
        MonsterSpeedGaugeValue.GaugeRatio = MGRatio;

        MonsterSpeedGaugeValue.PrevPos = MonsterQueue.Peek().NowPos; // 현재 계산할 위치의 이전 위치는 이전 위치의 현재 위치.

        if(MonsterQueue.Peek().NowPos == new Vector3(0,-SpeedGaugeHeight/2,0)) {
            MonsterSpeedGaugeValue.PrevPos = new Vector3(0,SpeedGaugeHeight/2,0);// 가장 위로 초기화
        }

        Vector3 MPos = new Vector3(0,SpeedGaugeHeight/2 - MGRatio * SpeedGaugeHeight,0);
        MonsterSpeedGaugeValue.NowPos = MPos;

        MonsterQueue.Enqueue(MonsterSpeedGaugeValue);

        howManyUpdates++;
    }

    IEnumerator UpdatePlayerSpeedGaugeAnim(float speed) {

        if(isPlayerTrashQueue)
        {
            PlayerQueue.Dequeue();
        }

        playerGaugeUpdateFinished = false;
        if(PlayerSpeedGaugeImage.transform.localPosition.y < PlayerQueue.Peek().NowPos.y) {
            PlayerSpeedGaugeImage.transform.localPosition = new Vector3(0,SpeedGaugeHeight/2,0);
        }

        while(PlayerSpeedGaugeImage.transform.localPosition.y > PlayerQueue.Peek().NowPos.y) { //도달해야 할 위치보다 더 위에 있을 때(미도달)
            PlayerSpeedGaugeImage.transform.localPosition += new Vector3(0,(PlayerQueue.Peek().NowPos - PlayerQueue.Peek().PrevPos).y * speed,0);//가야할 거리에 비례해 아래로 더 내리기
            yield return null;
        }
        playerGaugeUpdateFinished = true; //다 종료 후 true

        if(!isPlayerTrashQueue)
        {
            PlayerQueue.Dequeue();
        }
    }

    IEnumerator UpdateMonsterSpeedGaugeAnim(float speed) {

        if(isMonsterTrashQueue)
        {
            MonsterQueue.Dequeue();
        }

        monsterGaugeUpdateFinished = false;

        if(MonsterSpeedGaugeImage.transform.localPosition.y < MonsterQueue.Peek().NowPos.y) {
            MonsterSpeedGaugeImage.transform.localPosition = new Vector3(0,SpeedGaugeHeight/2,0);
        }

        while(MonsterSpeedGaugeImage.transform.localPosition.y > MonsterQueue.Peek().NowPos.y) { //더 위에 있을 때
            MonsterSpeedGaugeImage.transform.localPosition += new Vector3(0,(MonsterQueue.Peek().NowPos - MonsterQueue.Peek().PrevPos).y * speed,0);
            yield return null;
        }
        monsterGaugeUpdateFinished = true;

        if(!isMonsterTrashQueue)
        {
            MonsterQueue.Dequeue();
        }
    }

    float RoundTrimRatio(float ratio) {
        ratio = Mathf.Round(ratio * 100.0f) * 0.01f; //소숫점 두자리 수까지 반올림
        if(ratio >= 1.0f || ratio == 0.0f) { //턴을 살짝 넘어갔거나 아예 battleController에서 빼기당한경우
            ratio = 1.0f;
        }
        return ratio;
    }
}

//PlayerSpeedGaugeImage.transform.localPosition 현재위치가져오는코드