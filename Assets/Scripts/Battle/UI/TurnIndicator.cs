using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TurnIndicator : MonoBehaviour
{
    string text;
    TextMeshProUGUI indicator;
    public int turn {  get; private set; }
    public void Constructor(int mode)
    {
        turn = mode;
        indicator = GetComponentInChildren<TextMeshProUGUI>();
        switch (mode)
        {
            case 0:
                text = "배치";
                break;
            case 1:
            case 2:
                text = "이동";
                break;
            case 3:
                text = "공격";
                break;
            default:
                break;
        }
        indicator.text = text;
    }


}
