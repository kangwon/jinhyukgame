using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class EquipPanelController : MonoBehaviour
{
    GameObject[] artifactIcon = new GameObject[3];
    GameObject[] equipmentIcon = new GameObject[3];
    bool firstStart = true;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            artifactIcon[i] = GameObject.Find($"EquipPanel/AT{i+1}");
            equipmentIcon[i] = GameObject.Find($"EquipPanel/JB{i+1}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        if (firstStart) firstStart = false;
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (i < GameState.Instance.player.GetArtifacts().Count)
                    artifactIcon[i].transform.GetChild(0).GetComponent<Text>().text = GameState.Instance.player.GetArtifacts()?.ElementAt(i).name;
                else
                    artifactIcon[i].transform.GetChild(0).GetComponent<Text>().text = "";
            }
            equipmentIcon[0].transform.GetChild(0).GetComponent<Text>().text = GameState.Instance.player.GetHelmet()?.name;
            equipmentIcon[1].transform.GetChild(0).GetComponent<Text>().text = GameState.Instance.player.GetArmor()?.name;
            equipmentIcon[2].transform.GetChild(0).GetComponent<Text>().text = GameState.Instance.player.GetShoes()?.name;
        }
    }
}
