using System;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Button tapToStartBtn;
    [SerializeField] private Slider wealthBar;
    [SerializeField] private TextMeshProUGUI totalMoneyText;
    public int totalMoney;
    public GameObject popUpMoney;
    private Canvas _playerCanvas;

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
        _playerCanvas = wealthBar.GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        HideHuds();
    }

    public void HideTapToStartButton()
    {
        tapToStartBtn.gameObject.SetActive(false);
    }

    public void HideHuds()
    {
        totalMoneyText.transform.parent.gameObject.SetActive(false);
        _playerCanvas.enabled = false;
    }
    public void ShowHuds()
    {
        totalMoneyText.transform.parent.gameObject.SetActive(true);
        _playerCanvas.enabled = true;
    }

    public void UpdateMoney(int value)
    {
        totalMoney += value;
        wealthBar.value += value;
        totalMoney = Mathf.Clamp(totalMoney, 0, int.MaxValue);
        totalMoneyText.text = totalMoney.ToString();
        
        MoneyPopUp(value.ToString(), value);

        if( wealthBar.value >= wealthBar.minValue && wealthBar.value <= wealthBar.maxValue/4) Player.Instance.ChangeCurrentWealthState(WealthState.Poor);
        else if(wealthBar.value > wealthBar.maxValue/4 && wealthBar.value <= wealthBar.maxValue/2) Player.Instance.ChangeCurrentWealthState(WealthState.Average);
        else if(wealthBar.value > wealthBar.maxValue/2 && wealthBar.value <= wealthBar.maxValue) Player.Instance.ChangeCurrentWealthState(WealthState.Rich);
        
        OnMoneyCollected?.Invoke();
    }

    public void MoneyPopUp(string text,int value)
    {
        var popUp = Instantiate(popUpMoney, Vector3.zero, Quaternion.identity, _playerCanvas.transform);
        if (value > 0)
        {
            popUp.GetComponentInChildren<TextMesh>().color = new Color32(49, 255, 45, 255);
            popUp.GetComponentInChildren<TextMesh>().text = "+" + text + "$";
        }
        else
        {
            popUp.GetComponentInChildren<TextMesh>().color = new Color32(255,49,45,255);
            popUp.GetComponentInChildren<TextMesh>().text = "" + text + "$";
        }
        
        popUp.transform.localPosition = new Vector3(0.75f,-0.5f,0f);
        popUp.transform.localRotation = Quaternion.Euler(0f,0f,0f);
        
        Destroy(popUp,1f);
    }

    public void SetCameraPos()
    {
        if (Camera.main != null) Camera.main.transform.DOLocalMove(new Vector3(0f, 3.5f, -5f), 1f);
    }
}