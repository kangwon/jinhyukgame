using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public bool isBattle = false; //private���� �����?

    Player player = GameState.Instance.player;

    Monster monster = new Monster(new Stat() 
    { //���� 1�� ����. ���Ŀ� ���� ����Ʈ �޾ƿ����.
        maxHp = 120,
        attack = 6,
        defense = 10,
        speed = 15}
    );

    public const float MaxSpeedGauge = 200.0f; //���ǵ������ �ִ�

    private float playerGauge, monsterGauge; //�÷��̾�� ������ ���� ���ǵ������

    public enum combatState {
        Dead, //����
        Idle, //������ ä��� ��
        Turn, //���� ��
        Unable // IDEA : ���� ���� �ൿ�Ұ�
    }

    private combatState playerState, monsterState;


/*---------���� �����ϸ� ���� ����---------*/

    public void UpdateState() {
        if(playerState == combatState.Idle && monsterState == combatState.Idle) { //�� �� ������ ä��� ���� ��
            SpeedUntilTurn(); //�ൿ������ ����, �� �� �ϳ��� combatState�� Turn���� ����.
        }

        if(playerState == combatState.Turn) { //player�� Turn �� ���� ��
            CombatPhase(player, monster); //������ �°�
            playerState = combatState.Idle; //�ٽ� ������ ä��� ������
        } 

        else if (monsterState == combatState.Turn) { //monster�� Turn �� ���� ��
            CombatPhase(monster, player);
            monsterState = combatState.Idle;
        }

        if(playerState == combatState.Dead || monsterState == combatState.Dead) {
            EndCombat();
        }
    }

/*---------���� �ְ�ޱ�---------*/

    public void CombatPhase(CharacterBase attacker, CharacterBase defender) {//IDEA : C# Delegate ���� ��밡��?
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

/*---------��� �� ���� �������� 1000�� �� ������ ����---------*/
    public void SpeedUntilTurn() { 

        float totalElapsedTime = 0.0f;
        while(playerGauge < MaxSpeedGauge && monsterGauge < MaxSpeedGauge) { //�Ѵ� �ൿ�������� �ִ� �������� �̸��� ��������
            
            playerGauge += (player.GetStat().speed * Time.deltaTime); //�帥 �ð���ŭ �ӵ��� ���� �������� ä��
            
            Debug.Log("playerGauge is :" + playerGauge);

            monsterGauge += (monster.GetStat().speed * Time.deltaTime);

             Debug.Log("monsterGauge is :" + monsterGauge);

            totalElapsedTime += Time.deltaTime; // IDEA : ���Ŀ� ���� �ɸ��ð�?
        }

        if(playerGauge >= MaxSpeedGauge) {
            playerGauge -= MaxSpeedGauge;
            playerState = combatState.Turn;
        }
        if(monsterGauge >= MaxSpeedGauge) {
            monsterGauge -= MaxSpeedGauge;
            monsterState = combatState.Turn;
        } else {
            //���ÿ� �ൿ������ 1000?
        }

    }

/*----------���� ��(���� ��?)----------*/

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
