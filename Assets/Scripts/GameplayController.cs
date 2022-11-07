using System;
using MoreMountains.NiceVibrations;
using UnityEngine.UI;

public class GameplayController : MonoSingleton<GameplayController>
{
    public bool IsActive { get; set; }

    public static event Action<bool> OnGameplayFinished;

    private void Awake()
    {
        OnGameplayFinished += HapticVibration;
    }

    public void StartGameplay()
    {
        GameManager.Instance.WealthBar = FindObjectOfType<Slider>();
        IsActive = true;
        Player.Instance.splineFollower.followSpeed = Player.Instance.MoveSpeed;
        Player.Instance.ChangeCurrentWealthState(WealthState.Poor);
        GameManager.Instance.HudVisibility(true);
        GameManager.Instance.SetCameraPos();
        GameManager.Instance.TapToStartButtonShow(false);
        UIManager.Instance.Hide_FinishPanels();
    }

    public void RetryGameplay()
    {
        IsActive = true;
    }

    private void HapticVibration(bool success)
    {
        IsActive = false;
        var hapticType = success ? HapticTypes.Success : HapticTypes.Failure;
        MMVibrationManager.Haptic(hapticType);
    }

    private void StopPlayer()
    {
        Player.Instance.splineFollower.followSpeed = 0f;
    }

    public void FinishGameplay(bool success)
    {
        OnGameplayFinished?.Invoke(success);
        GameManager.Instance.HudVisibility(false);
        
        StopPlayer();
    }
    
    
}
