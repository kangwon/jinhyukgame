using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    void Start()
    {
        var menuButton = GameObject.Find("GameOverPanel/MenuButton").GetComponent<Button>();
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
