using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class RandomPanelController : MonoBehaviour
{
    StageChoice stageChoice;
    Text randomDescription;
    Text randomType;
    GameObject[] selectButton = new GameObject[16];
    public RandomCard RandomCard;
    public void OnClickRandomButton()
    {
        stageChoice.MoveToNextStage();
    }
    // Start is called before the first frame update
    void Start()
    {
        randomDescription = GameObject.Find("RandomPanel/RandomDescription").GetComponent<Text>();
        randomType = GameObject.Find("RandomPanel/RandomType").GetComponent<Text>();
        stageChoice = GameObject.Find("Canvas").GetComponent<StageChoice>();
        for(int i = 0; i < 16; i++)
        {
            selectButton[i] = GameObject.Find($"RandomPanel/SelectPanel/Contents/SelectButton{i + 1}");
        }
    }

    private void OnEnable()
    {
        if(RandomCard != null) 
        {           
            switch (RandomCard.randomEventType)
            {
                case RandomEventType.Positive:
                    RandomEvent.PositiveEvent(randomType, randomDescription, RandomCard); 
                    break;
                case RandomEventType.Neuturality:
                    RandomEvent.NeuturalityEvent(randomType, randomDescription, RandomCard);
                    break;
                case RandomEventType.Negative:
                    RandomEvent.NegativeEvent(randomType, randomDescription, RandomCard);
                    break;
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        var weapons = GameState.Instance.player.GetWeaponList();
        var helmet = GameState.Instance.player.GetHelmet();
        var armor = GameState.Instance.player.GetArmor();
        var shoes = GameState.Instance.player.GetShoes();
        var artifacts = GameState.Instance.player.GetArtifacts();
        for (int i = 0; i < 10; i++) //무기
        {
            selectButton[i].transform.GetChild(0).GetComponent<Text>().text = $"{weapons.ElementAt(i).name},{weapons.ElementAt(i).prefix},{weapons.ElementAt(i).rank}"; 
        }
        //장비
        selectButton[10].transform.GetChild(0).GetComponent<Text>().text = $"{helmet.name},{helmet.prefix},{helmet.rank}";
        selectButton[11].transform.GetChild(0).GetComponent<Text>().text = $"{armor.name},{armor.prefix},{armor.rank}";
        selectButton[12].transform.GetChild(0).GetComponent<Text>().text = $"{shoes.name},{shoes.prefix},{shoes.rank}";
        for(int i = 0; i < 3; i++) //아티펙트
        {
            if(i< artifacts.Count)
                selectButton[i+13].transform.GetChild(0).GetComponent<Text>().text = $"{artifacts.ElementAt(i).name}";
            else
            {
                selectButton[i + 13].SetActive(false);
            }
        }
    }
}
