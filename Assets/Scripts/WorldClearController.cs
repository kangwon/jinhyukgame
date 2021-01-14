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
        this.gameObject.SetActive(false);
    }

    void OnClickMenuButton()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
