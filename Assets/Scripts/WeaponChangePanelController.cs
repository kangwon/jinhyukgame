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
    GameObject chageCard;
    public void OnClickCard(int index)
    {

    }

    private void OnEnable()
    {
       weaponList = GameState.Instance.player.GetWeaponList();
        for(int i = 0; i < 11; i++)
        {
            card[i].transform.GetChild(0).GetComponent<Text>().text = $"{weaponList.ElementAt(i).name}\n{weaponList.ElementAt(i).statEffect.attack}";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 11; i++) 
            card[i] = GameObject.Find($"Card{i+1}");
        chageCard = GameObject.Find("ChangeCard");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
