using System;
using UnityEngine;
using System.Collections;

public class AnimationCoordinator : MonoBehaviour
{

    private enum AnimationStates
    {
        ModuleInSystem,
        ModuleOutOfSystem,
        ModuleExploded,
        ModuleShowed
    }

    [SerializeField]
    private ModuleMover _moduleMover;
    [SerializeField]
    private Animator _moduleAnimator;
    [SerializeField]
    private Animator _uiAnimator;
    [SerializeField]
    private AudioManager _audioManager;
    [SerializeField]
    private Transform _lightToBeToggled;
    [SerializeField]
    private ModuleRotater _moduleRotater;
    
    

    private AnimationStates _currentState;

    void Awake()
    {
        _currentState = AnimationStates.ModuleInSystem;
        if (_moduleMover == null || _moduleAnimator == null || _moduleRotater == null || _lightToBeToggled == null)
        {
            Debug.LogError("YOU FORGOT TO ASSIGN SOMETHING");
        }
        _lightToBeToggled.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        //Debug.Log("AnimatorCoordinator Enabled");
        EventManager.OnTapped += TapAction;
        EventManager.OnModuleArrive += WhenModuleArrives;
        EventManager.OnPositionReseted += OnPositionReseted;
    }

    void OnDisable()
    {
        //Debug.Log("AnimatorCoordinator Disabled");
        EventManager.OnTapped -= TapAction;
        EventManager.OnModuleArrive -= WhenModuleArrives;
        EventManager.OnPositionReseted -= OnPositionReseted;
    }


    private void TapAction()
    {
        switch (_currentState)
        {
            case AnimationStates.ModuleInSystem:
                _moduleMover.StartGoingToDisplayPosition();
                _lightToBeToggled.gameObject.SetActive(true);
                break;
            case AnimationStates.ModuleOutOfSystem:

                _moduleRotater.DetermineResetToRotation(false);
                EventManager.TriggerExplode();
                break;
            case AnimationStates.ModuleExploded:
                
                _moduleAnimator.SetTrigger("ExplodeReverse");
                _audioManager.TwirlAudio();
                EventManager.TriggerModuleIdle();
                _currentState = AnimationStates.ModuleShowed;
                break;
            case AnimationStates.ModuleShowed:
                _uiAnimator.SetTrigger("closeUI");
                _moduleRotater.DetermineResetToRotation(true);
                _lightToBeToggled.gameObject.SetActive(false);
                EventManager.TriggerModuleStop();

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        //Debug.Log(_currentState);
    }

    private void WhenModuleArrives()
    {
        _uiAnimator.SetTrigger("openUI");
        _currentState = AnimationStates.ModuleOutOfSystem;
        EventManager.TriggerModuleIdle();
    }

    private void OnPositionReseted()
    {
        if (_currentState == AnimationStates.ModuleOutOfSystem)
        {
            
            _moduleAnimator.SetTrigger("Explode");
            _audioManager.TwirlAudio();
            _currentState = AnimationStates.ModuleExploded;
        }
        else if (_currentState == AnimationStates.ModuleShowed)
        {
            
            _moduleMover.StartGoingToSystem();
            _currentState = AnimationStates.ModuleInSystem;
        }

    }

}
