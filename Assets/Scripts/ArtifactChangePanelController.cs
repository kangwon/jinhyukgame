using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ArtifactChangePanelController : MonoBehaviour
{
    List<Artifact> artifacts;
    GameObject[] card = new GameObject[4];
    GameObject changeCard;
    GameObject okButton;
    int changeCardIndex = 0;
    bool firstActive = false;
    void changeview()
    {
        if (changeCardIndex < artifacts.Count())
            changeCard.transform.GetChild(0).GetComponent<Text>().text = $"{artifacts.ElementAt(changeCardIndex).name}";
        else
            changeCard.transform.GetChild(0).GetComponent<Text>().text = $"선택된 \n 카드 없음";
    }
    void PrintCard()
    {
        for (int i = 0; i < 4; i++)
        {
            int temp = i;
            if (temp < artifacts.Count())
                card[temp].transform.GetChild(0).GetComponent<Text>().text = $"{artifacts.ElementAt(temp).name}";
            else
                card[temp].transform.GetChild(0).GetComponent<Text>().text = $"카드 없음";
        }
        changeview();
    }

    void OnClickOkButton()
    {
        if (changeCardIndex < artifacts.Count())
            GameState.Instance.player.RemoveAtArtifact(changeCardIndex);
        if (GameState.Instance.player.ArtifactsCount() <= 3)
        {
            this.gameObject.SetActive(false);
        }
        else PrintCard();
    }
    void OnClickCard(int index)
    {
        changeCardIndex = index;
        changeview();
    }

    // Start is called before the first frame update
    void Start()
    {
        okButton = GameObject.Find("ArtifactChangePanel/ArtifactOkButton");
        okButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            OnClickOkButton();
        });
        for (int i = 0; i < 4; i++)
        {
            int temp = i;
            card[temp] = GameObject.Find($"ArtifactChangePanel/ArtifactCard{temp + 1}");
            card[temp].GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClickCard(temp);
            });
        }
        changeCard = GameObject.Find("ArtifactChangePanel/ArtifactChangeCard");
        this.gameObject.transform.localPosition = StageChoice.PanelDisplayPosition;
        this.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        artifacts = GameState.Instance.player?.GetArtifacts();
        if (firstActive) PrintCard();
        firstActive = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
