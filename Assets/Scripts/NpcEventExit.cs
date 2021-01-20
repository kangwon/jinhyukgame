using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcEventExit : MonoBehaviour
{
    public StageChoice stageChoice;
    public void OnClickExit()
    {
        stageChoice.MoveToNextStage();
    }
}
