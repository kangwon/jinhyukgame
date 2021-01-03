using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickGameStartButton()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void OnClickRecordButton()
    {
        SceneManager.LoadScene("RecordScene");
    }
    public void OnClickOptionButton()
    {
        SceneManager.LoadScene("OptionScene");
    }

    public void OnClickCreditButton()
    {
        SceneManager.LoadScene("CreditScene");
    }
}