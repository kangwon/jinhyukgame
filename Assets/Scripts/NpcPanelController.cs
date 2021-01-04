using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcPanelController : MonoBehaviour
{
    public List<string> NpcItemIds;

    public StageChoice stageChoice;

    void Start()
    {
        NpcItemIds = new List<string>()
        {
            "sword1", "sword2", "sword3"
        };

        Player player = GameState.Instance.player;
        for (int i = 0; i < NpcItemIds.Count; i++)
        {
            Button button = GameObject.Find($"NpcItemButton{i + 1}").GetComponent<Button>();
            Equipment item = JsonDB.GetEquipment(NpcItemIds[i]);
            
            button.transform.Find("ItemName").GetComponent<Text>().text = item.name;
            button.transform.Find("ItemPrice").GetComponent<Text>().text = $"{item.price}G";
            button.onClick.AddListener(() => {
                player.BuyItem(item);
                stageChoice.MoveToNextStage();
            });
        }
    }

    void Update()
    {
        
    }
}
