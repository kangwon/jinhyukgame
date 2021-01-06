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
    int changeCardIndex;
    
    void PrintCard()
    {
        for (int i = 0; i < 11; i++)
        {
            card[i].transform.GetChild(0).GetComponent<Text>().text = $"{weaponList.ElementAt(i).name}\n{weaponList.ElementAt(i).statEffect.attack}";
        }
    }

    void OnClickCard(int index)
    {
        changeCardIndex = index;
        changeCard.transform.GetChild(0).GetComponent<Text>().text = $"{weaponList.ElementAt(index).name}\n{weaponList.ElementAt(index).statEffect.attack}";
    }
    void OnClickOkButton()
    {
        weaponList.RemoveAt(changeCardIndex);
        if (weaponList.Count <= 10)
        {
            player.SetWeaponList(weaponList);
            ChangePanel.SetActive(false);
        }
        PrintCard();
    }

    private void OnEnable()
    {
       weaponList = GameState.Instance.player.GetWeaponList();
       PrintCard();
    }
    // Start is called before the first frame update
    void Start()
    {      
       
        for (int i = 0; i < 11; i++)
        {
            card[i] = GameObject.Find($"Card{i + 1}");
            card[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClickCard(i);
            });
        }
        changeCard = GameObject.Find("ChangeCard");
        okButton = GameObject.Find("OkButton");
        okButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            OnClickOkButton();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
