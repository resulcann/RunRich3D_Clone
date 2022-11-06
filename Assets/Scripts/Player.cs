using UnityEngine;
using Dreamteck.Splines;
using System;

public enum WealthState
{
    Poor,
    Average,
    Rich,
    None
}
public class Player : MonoSingleton<Player>
{
    [SerializeField] private float moveSpeed = 5f;
    [HideInInspector] public SplineFollower splineFollower;

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    public WealthState CurrentWealthState { get; private set; }

    public event Action<WealthState /*Old*/, WealthState /*New*/> OnWealthsChanged;
    
    private void Awake()
    {
        splineFollower = GetComponent<SplineFollower>();
        splineFollower.followSpeed = 0;
        ChangeCurrentWealthState(WealthState.None);
    }
    
    public void ChangeCurrentWealthState(WealthState newWealthState)
    {
        var oldWealthState = CurrentWealthState;
        if (oldWealthState == newWealthState) return;
        CurrentWealthState = newWealthState;
        OnWealthsChanged?.Invoke(oldWealthState, CurrentWealthState);
        
    }
}
