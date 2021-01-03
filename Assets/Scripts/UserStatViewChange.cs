using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStatViewChange : MonoBehaviour
{
    int userStatnum = 0;

    public GameObject ATKview, ATKicon, DEFview, DEFicon, SPDview, SPDicon, BFview, BFicon;
    public GameObject jb1, jb2, jb3, jb4, at1, at2, at3;

    void ScreenoneView(bool torf)
    {
        ATKview.SetActive(torf);
        ATKicon.SetActive(torf);
        DEFview.SetActive(torf);
        DEFicon.SetActive(torf);
        SPDview.SetActive(torf);
        SPDicon.SetActive(torf);
        BFview.SetActive(torf);
        BFicon.SetActive(torf);
    }

    void ScreentwoView(bool torf)
    {
        jb1.SetActive(torf);
        jb2.SetActive(torf);
        jb3.SetActive(torf);
        jb4.SetActive(torf);
        at1.SetActive(torf);
        at2.SetActive(torf);
        at3.SetActive(torf);
    }


    void UserStatActive(int usn)
    {
        if (usn == 0)
        {
            userStatnum = 1;
            ScreenoneView(false);
            ScreentwoView(true);
        }
        else
        {
            userStatnum = 0;
            ScreenoneView(true);
            ScreentwoView(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
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
