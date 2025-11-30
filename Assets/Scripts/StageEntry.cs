using UnityEngine;

public class StageEntry : MonoBehaviour
{
    public string model;
    public string controller;

    public void EnterBattle()
    {
        if (!GameManager.Instance)
        {
            Debug.Log("Missing GameManager");
            return;
        }
        
        GameManager.Instance.EnterBattle(model, controller);
    }
}
