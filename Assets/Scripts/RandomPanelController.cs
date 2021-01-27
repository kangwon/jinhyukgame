using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class RandomPanelController : MonoBehaviour
{
    StageChoice stageChoice;
    Text randomDescription;
    Text randomType;
    public RandomCard RandomCard;
    List<GameObject> buttons = new List<GameObject>();
    GameObject selectPanel;
    GameObject buttonPrefab;
    RandomEvent randomEvent;
    public void OnClickRandomButton()
    {
        stageChoice.MoveToNextStage();
    }
    public void CreateButton(string name)
    {
        GameObject button = Instantiate(buttonPrefab, selectPanel.transform.GetChild(0).transform);
        button.transform.GetChild(0).GetComponent<Text>().text = $"{name}";
        buttons.Add(button);
    }  
    public void CreateButton(Weapon weapon) 
    {
        CreateButton($"{weapon.name},{weapon.rank},{weapon.prefix},{weapon.statEffect.attack}");
    }
    public void CreateButton(StatBuff buff)
    {
        GameObject button = Instantiate(buttonPrefab, selectPanel.transform.GetChild(0).transform);
        button.transform.GetChild(0).GetComponent<Text>().text = $"{buff.name}:{buff.description}";
        button.GetComponent<Button>().onClick.AddListener(()=> 
        { 
            GameState.Instance.player.AddBuff(buff);
            OnClickRandomButton(); 
        });
        buttons.Add(button);
    }
    public void CreateButton(Artifact artifact)
    {
        CreateButton($"{artifact.name}");
    }
    public void CreateButton(Helmet helmet,bool isRank)
    {
        GameObject button = Instantiate(buttonPrefab, selectPanel.transform.GetChild(0).transform);
        button.transform.GetChild(0).GetComponent<Text>().text = $"{helmet.name},{helmet.rank},{helmet.prefix},{helmet.statEffect.ToString()}";
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            OnClickButtonHelmet(helmet, isRank) ;
        });
        buttons.Add(button);
    } 
    public void CreateButton(Armor armor)
    {
        CreateButton($"{armor.name},{armor.rank},{armor.prefix},{armor.statEffect.ToString()}");
    }
    public void CreateButton(Shoes shoes)
    {
        CreateButton($"{shoes.name},{shoes.rank},{shoes.prefix},{shoes.statEffect.ToString()}");
    }
    void OnClickButtonHelmet(Helmet helmet,bool isRank)
    {
        if (helmet.id != "helmet")
        {
            var stringTemp = helmet.id;
            var intRank = Int32.Parse(stringTemp.Substring(8, 1));
            var intPrefix = Int32.Parse(stringTemp.Substring(9, 1));
            if ((isRank && (intRank < 4)) || (!isRank && (intPrefix < 4)))
            {
                if (isRank) intRank++;
                else intPrefix++;
                stringTemp = stringTemp.Substring(0, 8) + $"{intRank}" + $"{intPrefix}";
                GameState.Instance.player.SetEquipment(JsonDB.GetEquipment(stringTemp));
                OnClickRandomButton();
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        randomDescription = GameObject.Find("RandomPanel/RandomDescription").GetComponent<Text>();
        randomType = GameObject.Find("RandomPanel/RandomType").GetComponent<Text>();
        stageChoice = GameObject.Find("Canvas").GetComponent<StageChoice>();
        selectPanel = GameObject.Find("Canvas/RandomPanel/SelectPanel").gameObject;
        buttonPrefab = Resources.Load<GameObject>("SelectButtonPrefab");
        randomEvent = new RandomEvent();
    }

    private void OnEnable()
    {
        if(RandomCard != null) 
        {           
            switch (RandomCard.randomEventType)
            {
                case RandomEventType.Positive:
                    randomEvent.PositiveEvent(randomType, randomDescription, RandomCard); 
                    break;
                case RandomEventType.Neuturality:
                    randomEvent.NeuturalityEvent(randomType, randomDescription, RandomCard);
                    break;
                case RandomEventType.Negative:
                    randomEvent.NegativeEvent(randomType, randomDescription, RandomCard);
                    break;
            }
        }

    }
    private void OnDisable()
    {
        while (buttons.Count != 0)
        {
            Destroy(buttons.ElementAt(0));
            buttons.RemoveAt(0);
        }
    }
    // Update is called once per frame
}
