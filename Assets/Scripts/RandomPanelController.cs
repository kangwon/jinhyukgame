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
        if(RandomCard != null) //TODO : Event를 따로 처리해주는 클래스가 필요
        {
            switch (RandomCard.randomEventType)
            {
                case RandomEventType.Positive:
                    randomType.text = $"긍정";
                    randomDescription.text = $"긍정 이벤트";
                    break;
                case RandomEventType.Neuturality:
                    randomType.text = $"중립";
                    randomDescription.text = $"중립 이벤트";
                    break;
                case RandomEventType.Negative:
                    randomType.text = $"부정";
                    randomDescription.text = $"부정 이벤트";
                    break;
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
