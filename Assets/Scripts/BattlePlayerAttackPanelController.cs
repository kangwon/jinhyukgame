using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Battle 
{
    const int HAND_MAX = 4; //최대 핸드 수
    public List<Weapon> CardList = new List<Weapon>(); //CardList는 플레이어의 웨폰리스트를 가져와야함.
    public List<Weapon> CardDeck = new List<Weapon>();
    public List<Weapon> CardHand = new List<Weapon>();

    //기존의 카드댁,핸드 초기화하고 리스트에서 댁을 불러와 섞는것까지 하는 메서드
    public void BattleStart() 
    {
        CardDeck.Clear();
        CardHand.Clear();
        CardDeck.AddRange(CardList);
        Shuffle(CardDeck);
        Draw(CardDeck, CardHand, HAND_MAX);
    }

    //카드를 랜덤으로 섞기위한 메서드
    public static void Shuffle<T>(List<T> list) 
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    //카드를 드로우하는 매소드 (카드댁,핸드,드로우의 수) , 리턴값은 부족한 카드의 수
    public static int Draw<T>(List<T> deckList, List<T> handList, int n)
    {
        if (deckList.Count < n) // n 장 드로우 해야하는 데, 댁의 카드가 그보다 적을 경우
        {
            int count = 0;
            while (deckList.Count != 0)
            {
                handList.AddRange(deckList.GetRange(0, 1));
                deckList.RemoveRange(0, 1);
                count++;
            }
            Debug.Log($"카드 부족 !!! {n - count}장 ");
            return n - count; //부족한 카드 수를 리턴
        }
        else
        {
            handList.AddRange(deckList.GetRange(0, n));
            deckList.RemoveRange(0, n);
            return 0; //부족한 카드 없이 모두 뽑았으므로 0을 리턴
        }
    }

    public int DeckCount()
    {
        return CardDeck.Count();
    }

}

public class BattlePlayerAttackPanelController : MonoBehaviour
{ 
    const int HAND_MAX = 4; //최대 핸드 수
    const int SELECT_MAX = 3; // 선택가능한 핸드의 카드 수
    GameObject[] handCard = new GameObject[HAND_MAX] ;
    public GameObject deckCount;
    public bool[] selectCard = new bool[HAND_MAX];
    public List<Weapon> playerWeapons =new List<Weapon>();
    
    public Battle battle = new Battle();
    public StageChoice stageChoice;
    bool firstActive = false;
    //버튼이 토글처럼 되도록 했고, 최대 HAND_MAX(=3)만큼만 선택이 되도록 함.

    GameObject WorldClearPanel;
    GameObject GameOverPanel;

    Player player;

    GameObject RewardPanel;
    public MonsterCard MonsterCard;
    Monster monster;
    Text MonsterName;
    Text MonsterHp;

    private readonly float[] comboList ={0.3f, 0.8f, 0.5f }; //종류,등급,수식어 콤보 배수
    private bool[] comboCheck = new bool[3] { false, false, false };
    public int cardDamageSum; // 카드 선택한것 총 데미지
    public float comboPercentSum;
    public bool OnClickAttackPressed = false; // 카드 선택하고 attack 버튼을 누름.
    public bool turnTriggered; //SpeedGaugeUI에서 쓰일bool

    public void OnClickHandCard(int index)
    {
        if ((from n in selectCard where n == true select n).Count() < SELECT_MAX || selectCard[index] ==true)
        {
            selectCard[index] = !(selectCard[index]);

            if (selectCard[index])
            {
                ColorBlock colorBlock = handCard[index].GetComponent<Button>().colors;
                colorBlock.selectedColor = colorBlock.normalColor = colorBlock.highlightedColor = Color.gray;
                handCard[index].GetComponent<Button>().colors = colorBlock;
            }
            else
            {
                ColorBlock colorBlock = handCard[index].GetComponent<Button>().colors;
                colorBlock.selectedColor = colorBlock.normalColor = colorBlock.highlightedColor = Color.white;
                handCard[index].GetComponent<Button>().colors = colorBlock;
            }
        }
    }
   
    public void OnClickAttack()
    {
        cardDamageSum = 0;
        comboPercentSum = 0;
        Weapon[] selectWeapons = new Weapon[3];
        for(int i=0;i<comboCheck.Count();i++)
            comboCheck[i] = false;

        var maxCount = (from n in selectCard where n == true select n).Count();
        if (maxCount == 0) return; //선택된 카드가 없으면 버튼이 작동안하게 설정
        int j = 0;
        for(int i = HAND_MAX - 1; i >= 0; i--)
        {
            if (selectCard[i] == true) //손에서 정해진 카드를 battle 클래스에 전달
            {
                if (maxCount == 3)
                {
                    selectWeapons[j] = battle.CardHand.ElementAt(i);
                    j++;
                }
                cardDamageSum += battle.CardHand.ElementAt(i).statEffect.attack;
                battle.CardHand.RemoveAt(i); 
                OnClickHandCard(i); //버튼을 다시 눌러서 초기화           
            }          
        }
        if (maxCount == 3) //콤보 체크
        {
            
            if ((selectWeapons[0].weaponType != WeaponType.none) && (selectWeapons[0].weaponType == selectWeapons[1].weaponType) && (selectWeapons[1].weaponType == selectWeapons[2].weaponType)) comboCheck[0] = true;
            if ((selectWeapons[0].rank != Rank.none) && (selectWeapons[0].rank == selectWeapons[1].rank) && (selectWeapons[1].rank == selectWeapons[2].rank)) comboCheck[1] = true;
            if ((selectWeapons[0].prefix != Prefix.none) && (selectWeapons[0].prefix == selectWeapons[1].prefix) && (selectWeapons[1].prefix == selectWeapons[2].prefix)) comboCheck[2] = true;
        }
        for (int i = 0; i < 3; i++) 
        {
            if(comboCheck[i])comboPercentSum += comboList[i];
        }
        int checkDraw = 0;      
        checkDraw =Battle.Draw(battle.CardDeck,battle.CardHand,maxCount);
        if (0 != checkDraw)
        {
            battle.CardDeck.AddRange(battle.CardList);
            Battle.Shuffle(battle.CardDeck);
            Battle.Draw(battle.CardDeck, battle.CardHand, checkDraw);
        }
        OnClickAttackPressed = true;
    }

    public void ShowGameOver()
    {
        GameOverPanel.transform.localPosition = new Vector3(0, 0, 0);
        GameOverPanel.SetActive(true);
    }

    public void RewardStage()
    {
        var controller = RewardPanel.GetComponent<RewardPanelController>();
        controller.MonsterCard = this.MonsterCard;
        RewardPanel.transform.localPosition = StageChoice.PanelDisplayPosition;
        RewardPanel.SetActive(true);

        stageChoice.MoveToNextStage();
    }

    //해당 패널이 활성화 될때 실행되는 메서드
    private void OnEnable()
    {
        //플레이어의 무기10장을 가져와서 cardlist에 복사한다.
        if (firstActive)
        {
            player = GameState.Instance.player;
            playerWeapons.Clear();
            playerWeapons.AddRange(player.GetWeaponList());
            battle.CardList = playerWeapons;
            monster = MonsterCard.monster;
            battle.BattleStart();
        }
        firstActive = true;
    }

    // Start is called before the first frame update
    void Start()
    {   
        MonsterName = GameObject.Find("/Canvas/BattlePlayerAttackPanel/MonsterName").GetComponent<Text>();
        MonsterHp = GameObject.Find("/Canvas/BattlePlayerAttackPanel/MonsterHp").GetComponent<Text>();

        RewardPanel = GameObject.Find("Canvas").transform.Find("RewardPanel").gameObject;
        WorldClearPanel = GameObject.Find("Canvas").transform.Find("WorldClearPanel").gameObject;
        GameOverPanel = GameObject.Find("Canvas").transform.Find("GameOverPanel").gameObject;

        for (int i = 0; i < HAND_MAX; i++) 
        {
            handCard[i] = GameObject.Find($"HandCard{i + 1}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (monster != null)
        {
            MonsterName.text = monster.name;
            MonsterHp.text = $"{monster.hp} / {monster.GetStat().maxHp}";
        }

        for (int i = 0; i < HAND_MAX; i++)
        {
            handCard[i].transform.GetChild(0).GetComponent<Text>().text = $"{battle.CardHand.ElementAt(i).name}\n{battle.CardHand.ElementAt(i).statEffect.attack}";
        }
        deckCount.GetComponent<Text>().text = $"남은 덱: {battle.DeckCount()}";
    }
}
