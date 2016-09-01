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

    [SerializeField]
    private ModuleRotater _moduleRotater;

    private AnimationStates _currentState;

    void Awake()
    {
        _currentState = AnimationStates.ModuleInSystem;
        if (_systemAnimator == null || _moduleAnimator == null)
        {
            Debug.LogError("YOU FORGOT TO ASSIGN ANIMATOR");
        }
        if(_moduleRotater == null)
        {
            Debug.LogError("YOU FORGOT TO ASSIGN A MODULE ROTATER TO THIS SCRIPT");
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
                EventManager.TriggerExplode();
                //StartCoroutine(DelayedExplode(1f));
                _moduleAnimator.SetTrigger("Explode");
                _currentState = AnimationStates.ModuleExploded;
                break;
            case AnimationStates.ModuleExploded:
                _moduleAnimator.SetTrigger("ExplodeReverse");
                EventManager.TriggerModuleIdle();
                _currentState = AnimationStates.ModuleShowed;
                break;
            case AnimationStates.ModuleShowed:
                EventManager.TriggerModuleStop();
                _moduleRotater.StartRotatingTo(Quaternion.identity, 1f); // todo give animation length as reference
                _systemAnimator.SetTrigger("ModuleIn");
                _currentState = AnimationStates.ModuleInSystem;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        //Debug.Log(_currentState);
    }

    private IEnumerator DelayedExplode(float t)
    {
        float timePassed = 0;
        while(t > timePassed)
        {
            timePassed += Time.deltaTime;
            yield return new WaitForSeconds(0.33f);
        }
        Debug.LogWarning("mmm");
        _moduleAnimator.SetTrigger("Explode");
        _currentState = AnimationStates.ModuleExploded;
    }
}
