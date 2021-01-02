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
            Debug.Log("�������Դ�");
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
        //Debug.Log("attacker hp : " + attacker.nowHp);
        defender.TakeHit(Dmg);

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
        while(playerGauge <= MaxSpeedGauge && monsterGauge <= MaxSpeedGauge) { //�Ѵ� �ൿ�������� �ִ� �������� �̸��� ��������
            
            playerGauge += (int)(player.baseStat.speed * Time.deltaTime); //�帥 �ð���ŭ �ӵ��� ���� �������� ä��
            
            //Debug.Log("playerGauge is :" + playerGauge);
            monsterGauge += (int)(monster.baseStat.speed * Time.deltaTime);
            totalElapsedTime += Time.deltaTime; // IDEA : ���Ŀ� ���� �ɸ��ð�?
        }
        Debug.Log("3");
        if(playerGauge > monsterGauge) {
            playerState = combatState.Turn;
        } else if(playerGauge < monsterGauge) {
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
}
