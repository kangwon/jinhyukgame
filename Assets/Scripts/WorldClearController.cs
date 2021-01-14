using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldClearController : MonoBehaviour
{
    void Start()
    {
        var menuButton = GameObject.Find("Canvas").transform
            .Find("WorldClearPanel/MenuButton").gameObject.GetComponent<Button>();
        menuButton.onClick.AddListener(() => {
            this.OnClickMenuButton();
        });
    }

    void OnClickMenuButton()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
