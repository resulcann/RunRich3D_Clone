using System;
using MoreMountains.NiceVibrations;

public class GameplayController : MonoSingleton<GameplayController>
{
    public bool IsActive { get; set; }

    public Action<bool> OnGameplayFinished;

    public void StartGameplay()
    {
        IsActive = true;
        Player.Instance.splineFollower.followSpeed = Player.Instance.MoveSpeed;
        Player.Instance.ChangeCurrentWealthState(WealthState.Poor);
    }

    public void RetryGameplay()
    {
        IsActive = true;
    }

    public void FinishGameplay(bool success)
    {
        IsActive = false;

        var hapticType = success ? HapticTypes.Success : HapticTypes.Failure;

        MMVibrationManager.Haptic(hapticType);
        OnGameplayFinished?.Invoke(success);
    }
}
