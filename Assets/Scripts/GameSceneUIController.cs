using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneUIController : MonoBehaviour
{
    public Player player = new Player
    (
        new Stat()
        {
            maxHp = 20,
            attack = 5,
            defense = 3,
            speed = 2,
            startSpeedGauge = 1,
        }
    );

    List<string> NpcItemIds = new List<string>()
    {
        "sword1", "sword2", "sword3"
    };

    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < NpcItemIds.Count; i++)
        {
            Button button = GameObject.Find($"NpcItemButton{i + 1}").GetComponent<Button>();
            Equipment item = JsonDB.GetEquipment(NpcItemIds[i]);
            
            button.transform.Find("ItemName").GetComponent<Text>().text = item.name;
            button.transform.Find("ItemPrice").GetComponent<Text>().text = $"{item.price}G";
            button.onClick.AddListener(() => player.BuyItem(item));
        }
        Button buffButton = GameObject.Find("BuffButton").GetComponent<Button>(); 
        StatBuff buff = JsonDB.GetBuff("buff01");
        buffButton.onClick.AddListener(()=>player.AddBuff(buff));
    }

    // Update is called once per frame
    void Update()
    {
        if (!(player is null))
        {
            Stat playerStat = player.GetStat();
            GameObject.Find("Atk").GetComponent<Text>().text = $"공: {playerStat.attack}";
            GameObject.Find("Def").GetComponent<Text>().text = $"방: {playerStat.defense}";
            GameObject.Find("Spd").GetComponent<Text>().text = $"속: {playerStat.speed}";
            GameObject.Find("Hp").GetComponentInChildren<Text>().text = $"({player.hp} / {playerStat.maxHp})";
            GameObject.Find("Money").GetComponentInChildren<Text>().text = $"{player.money}G";
        }
    }

    public void OnClickBackButton()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
