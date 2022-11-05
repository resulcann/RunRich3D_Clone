using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Button tapToStartBtn;
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    
    public void HideTapToStartButton()
    {
        tapToStartBtn.gameObject.SetActive(false);
    }
}