using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class BuffPanelController : MonoBehaviour
{
    GameObject buffName;
    GameObject buffDescription;
    GameObject buffButton;
    // Start is called before the first frame update

    void Start()
    {
        this.buffName = GameObject.Find("BuffName");
        this.buffDescription = GameObject.Find("BuffDescription");
        this.buffButton = GameObject.Find("Button");
        int i = Random.Range(1, 3);
        StatBuff buff =JsonDB.GetBuff($"buff{i}");
        buffName.GetComponent<Text>().text = buff.name;
        buffDescription.GetComponent<Text>().text = buff.description;
              
    }

    public void OnclickBuffButton()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
