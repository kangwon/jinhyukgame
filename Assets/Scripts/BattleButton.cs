using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleButton : MonoBehaviour
{
    public CombatController cc;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickButton() {
        cc.isBattle = !cc.isBattle;
        Debug.Log("Button Clicked!");
    }
}
