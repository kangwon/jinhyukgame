using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//TODO:아직 배틀 클래스가 없어서 임시로 만듬 나중에 정섭이 코드 나오면 합쳐야함
public partial class Battle 
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
    //버튼이 토글처럼 되도록 했고, 최대 HAND_MAX(=3)만큼만 선택이 되도록 함.
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
        int sum = 0;
        int maxCount = (from n in selectCard where n == true select n).Count();
        for(int i= HAND_MAX-1; i>=0;i--)
        {
            if (selectCard[i] == true) //손에서 정해진 카드를 battle 클래스에 전달
            {
                //TODO: 나중에 데미지 관련(시너지)하여 추가해야함
                //정해진 카드를 모아서 attack매서드에 전달 후, 핸드에 있는 카드를 제거
                sum += battle.CardHand.ElementAt(i).statEffect.attack;
                battle.CardHand.RemoveAt(i); // 지금은 제거만 했음.
                OnClickHandCard(i); //버튼을 다시 눌러서 초기화           
            }          
        }
        Debug.Log($"attack : {sum}");
        int checkDraw = 0;      
        checkDraw =Battle.Draw(battle.CardDeck,battle.CardHand,maxCount);
        if (0 != checkDraw)
        {
            battle.CardDeck.AddRange(battle.CardList);
            Battle.Shuffle(battle.CardDeck);
            Battle.Draw(battle.CardDeck, battle.CardHand, checkDraw);
        }
    }
    //해당 패널이 활성화 될때 실행되는 메서드
    private void OnEnable()
    {
        // TODO:나중에 플레이어 클래스에 웨폰리스트가 추가되면 이 코드 삭제하기
        var stat = new Stat()
        {
            attack = 5
        };
        var weapon = new Weapon() { statEffect = stat };

        playerWeapons.Clear();
        for (int i = 0; i < 5; i++)
        {
            playerWeapons.Add(weapon);
        }
        var stat1 = new Stat()
        {
            attack = 10
        };
        var weapon1 = new Weapon() { statEffect = stat1 };
        for (int i = 0; i < 5; i++)
        {
            playerWeapons.Add(weapon1);
        }

        battle.CardList = playerWeapons;
        battle.BattleStart();      
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < HAND_MAX; i++) 
        {
            handCard[i] = GameObject.Find($"HandCard{i + 1}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < HAND_MAX; i++)
        {
            handCard[i].transform.GetChild(0).GetComponent<Text>().text = $"{battle.CardHand.ElementAt(i).statEffect}";
        }
        deckCount.GetComponent<Text>().text = $"{battle.DeckCount()}";
    }
}
