using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class WeaponChangePanelController : MonoBehaviour
{
    List<Weapon> weaponList;
    GameObject[] card = new GameObject[11];
    GameObject changeCard;
    GameObject okButton;
    int changeCardIndex=0;
    bool firstActive = false;

    void changeview()
    {
        changeCard.GetComponent<Image>().sprite = weaponList[changeCardIndex].weaponImg;
        if (changeCardIndex < weaponList.Count)
            changeCard.transform.GetChild(0).GetComponent<Text>().text = $"{weaponList.ElementAt(changeCardIndex).name}\n{weaponList.ElementAt(changeCardIndex).statEffect.attack}";
        else
            changeCard.transform.GetChild(0).GetComponent<Text>().text = $"선택된 \n 카드 없음";
    }

    void PrintCard()
    {
        for (int i = 0; i < 11; i++)
        {
            card[i].GetComponent<Image>().sprite = weaponList[i].weaponImg;
            int temp = i;
            if(temp < weaponList.Count)
                card[temp].transform.GetChild(0).GetComponent<Text>().text = $"{weaponList.ElementAt(temp).name}\n{weaponList.ElementAt(temp).statEffect.attack}";
            else
                card[temp].transform.GetChild(0).GetComponent<Text>().text = $"카드 없음";
        }
        changeview();
    }

    void OnClickCard(int index)
    {
        changeCardIndex = index;
        changeview();
    }
    void OnClickOkButton()
    {
        if(changeCardIndex<weaponList.Count)
            weaponList.RemoveAt(changeCardIndex);
        if (weaponList.Count <= 10)
        {
            var sortList = weaponList.OrderBy(x => x.id).ToList();
            GameState.Instance.player.SetWeaponList(sortList);
            this.gameObject.SetActive(false);
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

        this.gameObject.transform.localPosition = StageChoice.PanelDisplayPosition;
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        weaponList = GameState.Instance.player?.GetWeaponList();
        if (firstActive) PrintCard(); //맨 처음에 onEnable 될 때, 11번째 카드는 없으므로 이 함수가 호출 안되게 했음.
        firstActive = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
