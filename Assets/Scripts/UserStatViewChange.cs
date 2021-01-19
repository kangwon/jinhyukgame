using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStatViewChange : MonoBehaviour
{
    int userStatnum = 0;

    public GameObject equipPanel, buffandstatPanel, bfPopup;

    void ScreenView(bool torf)
    {
        equipPanel.SetActive(torf);
        buffandstatPanel.SetActive(!torf);
    }

    void UserStatActive(int usn)
    {
        if (usn == 0)
        {
            userStatnum = 1;
            ScreenView(true);
        }
        else
        {
            userStatnum = 0;
            ScreenView(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        equipPanel = GameObject.Find("EquipPanel");
        equipPanel.SetActive(false);
        buffandstatPanel = GameObject.Find("BuffandStatPanel");
        bfPopup = GameObject.Find("BuffandStatPanel/BF popup");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickUserScreen()
    {
        UserStatActive(userStatnum);
    }
}
