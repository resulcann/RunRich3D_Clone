using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Button tapToStartBtn;
    [SerializeField] private Slider wealthBar;
    [SerializeField] private TextMeshProUGUI totalMoneyText;
    [HideInInspector] public Canvas playerCanvas;
    public int totalMoney;
    public GameObject popUpMoney;
    

    public Slider WealthBar
    {
        get => wealthBar;
        set => wealthBar = value;
    }
    public Color32 WealthBarColor
    {
        get => wealthBar.fillRect.GetComponent<Image>().color;
        set => wealthBar.fillRect.GetComponent<Image>().color = value;
    }
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        HudVisibility(false);
        TapToStartButtonShow(true);
    }

    public void TapToStartButtonShow(bool value)
    {
        tapToStartBtn.gameObject.SetActive(value);
    }

    public void HudVisibility(bool value)
    {
        totalMoneyText.transform.parent.gameObject.SetActive(value);
        playerCanvas.enabled = value;
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
    }

    private void MoneyPopUp(string text,int value)
    {
        var popUp = Instantiate(popUpMoney, Vector3.zero, Quaternion.identity, playerCanvas.transform);
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