using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class BuffPanelController : MonoBehaviour
{

    GameObject buffName;
    GameObject buffDescription;
    public GameObject buffPanel;
    public GameObject cardSelectPanel;
    // Start is called before the first frame update

    void Start()
    {
        this.buffName = GameObject.Find("BuffName");
        this.buffDescription = GameObject.Find("BuffDescription");
        int i = Random.Range(1, 3); // json 파일 버프 수가 늘어나면 바꾸기
        StatBuff buff =JsonDB.GetBuff($"buff{i}");
        buffName.GetComponent<Text>().text = buff.name;
        buffDescription.GetComponent<Text>().text = buff.description;
              
    }
    
    public void OnClickBuffButton()
    {
        buffPanel.SetActive(false);
        cardSelectPanel.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
