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

    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }

    public void OnClickBackButton()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
