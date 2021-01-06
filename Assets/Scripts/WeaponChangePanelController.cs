using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class WeaponChangePanelController : MonoBehaviour
{
    Player player = GameState.Instance.player;
    List<Weapon> weaponList;
    GameObject[] card = new GameObject[11];
    GameObject changeCard;
    GameObject okButton;
    public GameObject ChangePanel;
    int changeCardIndex=0;
    bool firstActive = false;



    void PrintCard()
    {
        for (int i = 0; i < 11; i++)
        {
            int temp = i;
            if(temp < weaponList.Count)
                card[temp].transform.GetChild(0).GetComponent<Text>().text = $"{weaponList.ElementAt(temp).name}\n{weaponList.ElementAt(temp).statEffect.attack}";
            else
                card[temp].transform.GetChild(0).GetComponent<Text>().text = $"카드 없음";
        }
     if(changeCardIndex<weaponList.Count)   changeCard.transform.GetChild(0).GetComponent<Text>().text = $"{weaponList.ElementAt(changeCardIndex).name}\n{weaponList.ElementAt(changeCardIndex).statEffect.attack}";
    else changeCard.transform.GetChild(0).GetComponent<Text>().text = $"선택된 \n 카드 없음";
    }

    void OnClickCard(int index)
    {
        changeCardIndex = index;
        if (index < weaponList.Count)  
            changeCard.transform.GetChild(0).GetComponent<Text>().text = $"{weaponList.ElementAt(index).name}\n{weaponList.ElementAt(index).statEffect.attack}";
        else 
            changeCard.transform.GetChild(0).GetComponent<Text>().text = $"선택된 \n 카드 없음";
    }
    void OnClickOkButton()
    {
        if(changeCardIndex<weaponList.Count)
            weaponList.RemoveAt(changeCardIndex);
        if (weaponList.Count <= 10)
        {
           
            player.SetWeaponList(weaponList);
            ChangePanel.SetActive(false);
        }
       else PrintCard();
    }


    // Start is called before the first frame update
    void Start()
    {
        okButton = GameObject.Find("OkButton");
        okButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            OnClickOkButton();
        });
        for (int i = 0; i < 11; i++)
        {
            int temp = i;
            card[temp] = GameObject.Find($"WeaponCard{temp + 1}");
            card[temp].GetComponent<Button>().onClick.AddListener(() =>
            { 
                OnClickCard(temp);
            });
        }
        changeCard = GameObject.Find("ChangeCard");
        ChangePanel.transform.localPosition = StageChoice.PanelDisplayPosition;
        ChangePanel.SetActive(false);
    }
    private void OnEnable()
    {
        weaponList = GameState.Instance.player.GetWeaponList();
        if (firstActive) PrintCard(); //맨 처음에 onEnable 될 때, 11번째 카드는 없으므로 이 함수가 호출 안되게 했음.
        firstActive = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
