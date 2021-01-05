using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChestPanelController : MonoBehaviour
{
    public GameObject chestPanel;
    public GameObject chestType;
    public GameObject chestDescription;
    public StageChoice stageChoice;
    public void OnClickTreasureButton()
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
