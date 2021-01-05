using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeginnerWeapon : MonoBehaviour
{
    Player player = GameState.Instance.player;
    // Start is called before the first frame update
    void Start()
    {
        player.ResetWeaponList();
        for (int i = 0; i < 5; i++)
        {
            player.SetEquipment(JsonDB.GetEquipment($"weapon_{i}00"));
            player.SetEquipment(JsonDB.GetEquipment($"weapon_{i}00"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
