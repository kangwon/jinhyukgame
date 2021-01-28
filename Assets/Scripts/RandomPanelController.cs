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
    public void CreateButton(Weapon weapon,bool isRank,int index) 
    {
        GameObject button = Instantiate(buttonPrefab, selectPanel.transform.GetChild(0).transform);
        button.transform.GetChild(0).GetComponent<Text>().text = $"{weapon.name},{weapon.rank},{weapon.prefix},{weapon.statEffect.attack}";
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            OnClickButtonWeapon(weapon, isRank,index);
        });
        buttons.Add(button);
    }
    public void CreateButton(Equipment equip,bool isRank)
    {
        GameObject button = Instantiate(buttonPrefab, selectPanel.transform.GetChild(0).transform);
        button.transform.GetChild(0).GetComponent<Text>().text = $"{equip.name},{equip.rank},{equip.prefix},{equip.statEffect.ToString()}";
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            OnClickButtonEquipment(equip, isRank) ;
        });
        buttons.Add(button);
    } 
    public void CreateButton(Artifact artifact,int index)
    {
        GameObject button = Instantiate(buttonPrefab, selectPanel.transform.GetChild(0).transform);
        button.transform.GetChild(0).GetComponent<Text>().text = $"{artifact.name}";
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameState.Instance.player.ChangeAtArtifact(index, RandomCard.artifact);
            OnClickRandomButton();
        });
        buttons.Add(button);
    }
    void OnClickButtonWeapon(Weapon weapon,bool isRank, int index)
    {
        if (weapon.id != "bare_fist")
        {
            var stringTemp = weapon.id;
            var intRank = (int)weapon.rank;
            var intPrefix = (int)weapon.prefix;
            if ((isRank && (intRank < 4)) || (!isRank && (intPrefix < 4)))
            {
                if (isRank) intRank++;
                else intPrefix++;
                stringTemp = stringTemp.Substring(0, 8) + $"{intRank}" + $"{intPrefix}";
                var weaponList = GameState.Instance.player.GetWeaponList();
                weaponList.RemoveAt(index);
                weaponList.Add(JsonDB.GetWeapon(stringTemp));
               var sortList  = weaponList.OrderBy(x => x.id).ToList();
                GameState.Instance.player.SetWeaponList(sortList);
                OnClickRandomButton();
            }
        }
    }
    void OnClickButtonEquipment(Equipment equip,bool isRank)
    {
        if ((equip.id != "helmet") && (equip.id != "armor")&& (equip.id != "shoes"))
        {
            var stringTemp = equip.id;
            var intRank = (int)equip.rank;
            var intPrefix = (int)equip.prefix;
            if ((isRank && (intRank < 4)) || (!isRank && (intPrefix < 4)))
            {
                if (isRank) intRank++;
                else intPrefix++;
                if(equip.type =="helmet") stringTemp = stringTemp.Substring(0, 8);
                else if (equip.type == "armor" || equip.type=="shoes") stringTemp = stringTemp.Substring(0, 7);
                stringTemp += $"{intRank}" + $"{intPrefix}";
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
