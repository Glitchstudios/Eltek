using UnityEngine;
using System.Collections;

public static class EventManager
{

    public delegate void TapAction();
    public static event TapAction OnTapped;

    public delegate void Explode();
    public static event Explode OnExplode;

    public delegate void ModuleGoIdle();
    public static event ModuleGoIdle OnModuleIdle;

    public delegate void ModuleStopIdle();
    public static event ModuleStopIdle OnModuleStop;


    private static float _lastTimeTapped;
    private static readonly float _tapBlockInterval = 1.25f;


    public static void TriggerTapAction()
    {
        if (_lastTimeTapped + _tapBlockInterval < Time.time)
        {
            //Debug.Log("TAP ACTION INVOKED!");
            //OnTapped?.Invoke(); //doesnt work in unity...
            if (OnTapped != null) OnTapped();
            _lastTimeTapped = Time.time;
        }
        else
        {
            Debug.Log("slow down bro, chill");
        }
    }

    public static void TriggerExplode()
    {
        Debug.Log("Explode Triggered");
        if (OnExplode != null) OnExplode();
    }

    public static void TriggerModuleIdle()
    {
        Debug.Log("Idle triggered");
        if (OnModuleIdle != null) OnModuleIdle();
    }

    public static void TriggerModuleStop()
    {
        Debug.Log("ModuleStop triggered");
        if (OnModuleStop != null) OnModuleStop();
    }
}
