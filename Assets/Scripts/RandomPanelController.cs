using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomPanelController : MonoBehaviour
{
    StageChoice stageChoice;
    Text randomDescription;
    Text randomType;
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
        
    }
}
