using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransitions : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Poor = Animator.StringToHash("Poor");
    private static readonly int Average = Animator.StringToHash("Average");
    private static readonly int Rich = Animator.StringToHash("Rich");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Player.Instance.OnWealthsChanged += Player_OnWealthStateChanged;
    }
    
    private void Player_OnWealthStateChanged(WealthState oldWealthState, WealthState newWealthState)
    {
        switch (newWealthState)
        {
            case WealthState.Poor:
                _animator.SetBool(Poor,true);
                _animator.SetBool(Average,false);
                _animator.SetBool(Rich,false);
                break;
            
            case WealthState.Average:
                _animator.SetBool(Poor,false);
                _animator.SetBool(Average,true);
                _animator.SetBool(Rich,false);
                break;
            
            case WealthState.Rich:
                _animator.SetBool(Poor,false);
                _animator.SetBool(Average,false);
                _animator.SetBool(Rich,true);
                break;
            
            default:
                _animator.SetBool(Poor,false);
                _animator.SetBool(Average,false);
                _animator.SetBool(Rich,false);
                break;
        }
    }
    
    
}
