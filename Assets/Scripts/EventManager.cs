using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{

    public delegate void TapAction();
    public static event TapAction OnTapped;


    private static float _lastTimeTapped;
    private static readonly float _tapBlockInterval = 1.25f;
    public static void TriggerTapAction()
    {
        if (_lastTimeTapped + _tapBlockInterval < Time.time)
        {
            Debug.Log("TAP ACTION INVOKED!");
            //OnTapped?.Invoke(); //doesnt work in unity...
            if (OnTapped != null) OnTapped();
            _lastTimeTapped = Time.time;
        }
        else
        {
            Debug.Log("slow down bro, chill");
        }
    }
}
