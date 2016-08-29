using System;
using UnityEngine;
using System.Collections;

public class AnimationCoordinator : MonoBehaviour {

    private enum AnimationStates
    {
        ModuleInSystem,
        ModuleOutOfSystem,
        ModuleExploded,
        ModuleShowed
    }

    [SerializeField]
    private Animator _systemAnimator;
    [SerializeField]
    private Animator _moduleAnimator;

    private AnimationStates _currentState;

    void Awake()
    {
        _currentState = AnimationStates.ModuleInSystem;
        if (_systemAnimator == null || _moduleAnimator == null)
        {
            Debug.LogError("YOU FORGOT TO ASSIGN ANIMATOR");
        }
    }

    void OnEnable()
    {
        Debug.Log("AnimatorCoordinator Enabled");
        EventManager.OnTapped += TapAction;
    }

    void OnDisable()
    {
        Debug.Log("AnimatorCoordinator Disabled");
        EventManager.OnTapped -= TapAction;
    }

    private void TapAction()
    {
        switch (_currentState)
        {
            case AnimationStates.ModuleInSystem:
                _systemAnimator.SetTrigger("ModuleOut");
                _currentState = AnimationStates.ModuleOutOfSystem;
                break;
            case AnimationStates.ModuleOutOfSystem:
                _moduleAnimator.SetTrigger("Explode");
                _currentState = AnimationStates.ModuleExploded;
                break;
            case AnimationStates.ModuleExploded:
                _moduleAnimator.SetTrigger("ExplodeReverse");
                _currentState = AnimationStates.ModuleShowed;
                break;
            case AnimationStates.ModuleShowed:
                _systemAnimator.SetTrigger("ModuleIn");
                _currentState = AnimationStates.ModuleInSystem;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Debug.Log(_currentState);
    }
}
