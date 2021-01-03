using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//Leftover
//1. Initialize game related data
public class BackButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickBackButton()
    {
        SceneManager.LoadScene("MenuScene"); //Scene change to menu scene
    }
}
