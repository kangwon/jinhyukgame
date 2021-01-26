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
    public RandomCard RandomCard;
    List<GameObject> buttons = new List<GameObject>();
    GameObject selectPanel;
    GameObject buttonPrefab;
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
        GameObject button = Instantiate(buttonPrefab, selectPanel.transform.GetChild(0).transform);
        button.transform.GetChild(0).GetComponent<Text>().text = $"{weapon.name},{weapon.rank},{weapon.prefix},{weapon.statEffect.attack}";
        buttons.Add(button);
    }
    public void CreateButton(StatBuff buff)
    {
        GameObject button = Instantiate(buttonPrefab, selectPanel.transform.GetChild(0).transform);
        button.transform.GetChild(0).GetComponent<Text>().text = $"{buff.name}:{buff.description}";
        buttons.Add(button);
    }
    public void CreateButton(Artifact artifact)
    {
        GameObject button = Instantiate(buttonPrefab, selectPanel.transform.GetChild(0).transform);
        button.transform.GetChild(0).GetComponent<Text>().text = $"{artifact.name}";
        buttons.Add(button);
    }
    public void CreateButton(Helmet helmet)
    {
        GameObject button = Instantiate(buttonPrefab, selectPanel.transform.GetChild(0).transform);
        button.transform.GetChild(0).GetComponent<Text>().text = $"{helmet.name},{helmet.rank},{helmet.prefix},{helmet.statEffect.ToString()}";
        buttons.Add(button);
    } 
    public void CreateButton(Armor armor)
    {
        GameObject button = Instantiate(buttonPrefab, selectPanel.transform.GetChild(0).transform);
        button.transform.GetChild(0).GetComponent<Text>().text = $"{armor.name},{armor.rank},{armor.prefix},{armor.statEffect.ToString()}";
        buttons.Add(button);
    }
    public void CreateButton(Shoes shoes)
    {
        GameObject button = Instantiate(buttonPrefab, selectPanel.transform.GetChild(0).transform);
        button.transform.GetChild(0).GetComponent<Text>().text = $"{shoes.name},{shoes.rank},{shoes.prefix},{shoes.statEffect.ToString()}";
        buttons.Add(button);
    }
    // Start is called before the first frame update
    void Start()
    {
        randomDescription = GameObject.Find("RandomPanel/RandomDescription").GetComponent<Text>();
        randomType = GameObject.Find("RandomPanel/RandomType").GetComponent<Text>();
        stageChoice = GameObject.Find("Canvas").GetComponent<StageChoice>();
        selectPanel = GameObject.Find("Canvas/RandomPanel/SelectPanel").gameObject;
        buttonPrefab = Resources.Load<GameObject>("SelectButtonPrefab");
    }

    private void OnEnable()
    {
        if(RandomCard != null) 
        {           
            switch (RandomCard.randomEventType)
            {
                case RandomEventType.Positive:
                    RandomEvent.Instance.PositiveEvent(randomType, randomDescription, RandomCard); 
                    break;
                case RandomEventType.Neuturality:
                    RandomEvent.Instance.NeuturalityEvent(randomType, randomDescription, RandomCard);
                    break;
                case RandomEventType.Negative:
                    RandomEvent.Instance.NegativeEvent(randomType, randomDescription, RandomCard);
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
    void Update()
    {
        //var weapons = GameState.Instance.player.GetWeaponList();
        // var helmet = GameState.Instance.player.GetHelmet();
        //var armor = GameState.Instance.player.GetArmor();
        // var shoes = GameState.Instance.player.GetShoes();
        // var artifacts = GameState.Instance.player.GetArtifacts();       
    }
}
