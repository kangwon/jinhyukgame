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
        CreateButton($"{weapon.name},{weapon.rank},{weapon.prefix},{weapon.statEffect.attack}");
    }
    public void CreateButton(StatBuff buff)
    {
        CreateButton($"{buff.name}:{buff.description}");
    }
    public void CreateButton(Artifact artifact)
    {
        CreateButton($"{artifact.name}");
    }
    public void CreateButton(Helmet helmet)
    {
        CreateButton($"{helmet.name},{helmet.rank},{helmet.prefix},{helmet.statEffect.ToString()}");
    } 
    public void CreateButton(Armor armor)
    {
        CreateButton($"{armor.name},{armor.rank},{armor.prefix},{armor.statEffect.ToString()}");
    }
    public void CreateButton(Shoes shoes)
    {
        CreateButton($"{shoes.name},{shoes.rank},{shoes.prefix},{shoes.statEffect.ToString()}");
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
}
