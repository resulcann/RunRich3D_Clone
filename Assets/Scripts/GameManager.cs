using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Button tapToStartBtn;
    [SerializeField] private Slider wealthBar;
    [SerializeField] private TextMeshProUGUI totalMoneyText;
    public int totalMoney;

    public Color32 WealthBarColor
    {
        get
        {
            return wealthBar.fillRect.GetComponent<Image>().color;
        }
        set
        {
            wealthBar.fillRect.GetComponent<Image>().color = value;
        }
    }

    public static event Action OnMoneyCollected;
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    
    public void HideTapToStartButton()
    {
        tapToStartBtn.gameObject.SetActive(false);
    }

    public void UpdateMoney(int value)
    {
        totalMoney += value;
        wealthBar.value += value;
        totalMoney = Mathf.Clamp(totalMoney, 0, int.MaxValue);
        totalMoneyText.text = totalMoney.ToString();
        
        if( wealthBar.value >= wealthBar.minValue && wealthBar.value <= wealthBar.maxValue/4) Player.Instance.ChangeCurrentWealthState(WealthState.Poor);
        else if(wealthBar.value > wealthBar.maxValue/4 && wealthBar.value <= wealthBar.maxValue/2) Player.Instance.ChangeCurrentWealthState(WealthState.Average);
        else if(wealthBar.value > wealthBar.maxValue/2 && wealthBar.value <= wealthBar.maxValue) Player.Instance.ChangeCurrentWealthState(WealthState.Rich);
        
        OnMoneyCollected?.Invoke();
    }
}