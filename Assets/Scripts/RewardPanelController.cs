using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RewardPanelController : MonoBehaviour
{
    public StageChoice stageChoice;
    public void OnClickRewardAbandon()
    {
        stageChoice.MoveToNextStage();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
