using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public bool isBattle = true;

    public Player player;
    public Monster monster;

    public const float MaxSpeedGauge = 1000.0f;

    private float playerGauge = 0.0f;
    private float monsterGauge = 0.0f;

    public enum combatState {
        Dead, //����
        Idle, //������ ä��� ��
        Turn, //���� ��
        Unable // IDEA : ���� ���� �ൿ�Ұ�
    }

    private combatState playerState = combatState.Idle;
    private combatState monsterState = combatState.Idle;


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
            
            playerGauge += (player.baseStat.speed * Time.deltaTime); //�帥 �ð���ŭ �ӵ��� ���� �������� ä��
            
            Debug.Log("playerGauge is :" + playerGauge);

            monsterGauge += (monster.baseStat.speed * Time.deltaTime);

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
        if(player != null) {
            playerGauge = player.baseStat.startSpeedGauge; //TODO : baseStat�� getStat()���� �ٲٱ�. ���� Aggregate���� ArgumentNullException.
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

    public float getPlayerSpeedGaugeVal() {
        return playerGauge;
    }

    public float getMonsterSpeedGaugeVal() {
        return monsterGauge;
    }
}
