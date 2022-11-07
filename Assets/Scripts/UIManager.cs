using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject finishSuccessPanel, finishFailPanel;
    private void Awake()
    {
        GameplayController.OnGameplayFinished += Show_FinishPanel;
    }

    private void Show_FinishPanel(bool success)
    {
        if (success)
        {
            finishSuccessPanel.SetActive(true);
            finishFailPanel.SetActive(false);
        }
        else
        {
            finishSuccessPanel.SetActive(false);
            finishFailPanel.SetActive(true);
        }
    }

    public void Hide_FinishPanels()
    {
        finishSuccessPanel.SetActive(false);
        finishFailPanel.SetActive(false);
    }
}
