using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuffPopupView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject buffPopup;
    public GameObject buffSummary;

    public void OnPointerDown(PointerEventData data)
    {
        buffPopup.SetActive(true);
        buffSummary.SetActive(true);
    }

    public void OnPointerUp(PointerEventData data)
    {
        buffPopup.SetActive(false);
        buffSummary.SetActive(false);
    }
}