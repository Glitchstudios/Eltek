using UnityEngine;
using System.Collections;

public class ModuleRotater : MonoBehaviour
{

    [SerializeField]
    private float _rotationSpeed = .5f;
    [SerializeField]
    private bool _turnRight = true;
    [SerializeField]
    private float _rotationAxisX = 0f;
    [SerializeField]
    private float _rotationAxisY = 1f;
    [SerializeField]
    private float _rotationAxisZ = 0f;

    [SerializeField]
    private float _resetRotationSpeed = 100f;

    private Vector3 _rotationVector;

    private bool _shouldRotate;
    private bool _shouldCounterRotate;

    private Transform _cachedTransform;

    void Awake()
    {
        _cachedTransform = transform;
    }

    void OnEnable()
    {
        _rotationVector = new Vector3(_rotationAxisX, _rotationAxisY, _rotationAxisZ);
        Debug.Log("subbed");
        EventManager.OnModuleIdle += StartRotating;
        //EventManager.OnExplode += DoExplodeRotation;
        EventManager.OnModuleStop += StopRotating;
    }

    void OnDisable()
    {
        Debug.Log("unsubbed");
        EventManager.OnModuleIdle -= StartRotating;
        //EventManager.OnExplode -= DoExplodeRotation;
        EventManager.OnModuleStop -= StopRotating;

        _shouldRotate = false;
    }

    private void StartRotating()
    {
        if (_shouldRotate)
        {
            Debug.Log("It is already rotating");
            return;
        }
        Debug.Log("Start Rotation coroutine");
        _shouldRotate = true;
        StartCoroutine(RotateThis());
    }

    private void StopRotating()
    {
        _shouldRotate = false;
        _shouldCounterRotate = false;
    }

    private IEnumerator RotateThis()
    {
        //
        //Debug.Log("Rotating started");
        //while(_shouldRotate)
        //{
        //    if(_shouldCounterRotate)
        //    {
        //        Debug.Log("counter rotating");
        //        
        //    }
        //    else
        //    {
        //        _rotationVector = new Vector3(_rotationAxisX, _rotationAxisY, _rotationAxisZ);
        //        //todo DO ROTATE LOGIC
        //        _cachedTransform.Rotate(_rotationVector, _rotationSpeed * (_turnRight?1f:-1f));
        //    }
        //
        //    yield return new WaitForEndOfFrame();
        //}
        //
        //Debug.Log("Rotating stopped");
        yield return new WaitForEndOfFrame();
    }

    private void DoExplodeRotation()
    {
        _shouldCounterRotate = true;
        Debug.Log("Doing Explode counter rotation");
        StartRotatingTo(Quaternion.identity,1f);
        
    }

    public void StopCounterRotation()
    {
        _shouldCounterRotate = false;
    }


    public void StartRotatingTo(Quaternion d, float timeItTakes)
    {
        StartCoroutine(RotateTo(d));
    }

    private IEnumerator RotateTo(Quaternion d)
    {        
        while (Mathf.Abs(Quaternion.Angle(_cachedTransform.rotation, d)) > 0.002f)
        {
            _cachedTransform.rotation = Quaternion.RotateTowards(_cachedTransform.rotation, d, _resetRotationSpeed * Time.deltaTime);
            
            yield return new WaitForEndOfFrame();
        }
        _shouldCounterRotate = false;
        Debug.Log("Reached zero");
    }
}
