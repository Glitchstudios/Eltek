using UnityEngine;
using System.Collections;

public class ModuleRotater : MonoBehaviour
{
    [SerializeField]
    private ModuleMover _mm;
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

    [SerializeField] private float _resetRotationSpeed = 100f;
    [SerializeField] private float _anglePrecision = 1.5f;

    private Vector3 _rotationVector;

    private bool _shouldRotate;

    private Transform _cachedTransform;
    private Quaternion _initialRotation;
    private Quaternion _resetToRotation;

    void Awake()
    {
        _cachedTransform = transform;
        _initialRotation = _cachedTransform.rotation;
    }

    void OnEnable()
    {
        _rotationVector = new Vector3(_rotationAxisX, _rotationAxisY, _rotationAxisZ);
        //Debug.Log("subbed");
        EventManager.OnModuleIdle += StartRotating;
        EventManager.OnExplode += DoExplodeRotation;
        EventManager.OnModuleStop += ResetRotation;
    }

    void OnDisable()
    {
        //Debug.Log("unsubbed");
        EventManager.OnModuleIdle -= StartRotating;
        EventManager.OnExplode -= DoExplodeRotation;
        EventManager.OnModuleStop -= ResetRotation;

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
    }

    private void ResetRotation()
    {
        _shouldRotate = false;
        StartRotatingTo(_resetToRotation);

    }

    public void DetermineResetToRotation(bool hardReset)
    {
        if (hardReset)
        {
            _resetToRotation = _initialRotation;
        }
        else
        {
            _resetToRotation = _mm.LastPoint.rotation;
        }
    }

    private IEnumerator RotateThis()
    {

        Debug.Log("Rotating started");
        while (_shouldRotate)
        {
            _rotationVector = new Vector3(_rotationAxisX, _rotationAxisY, _rotationAxisZ);
            //todo DO ROTATE LOGIC
            _cachedTransform.Rotate(_rotationVector, _rotationSpeed * (_turnRight ? 1f : -1f));

            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Rotating stopped");
        yield return new WaitForEndOfFrame();
    }


    private void DoExplodeRotation()
    {
        _shouldRotate = false;
        Debug.Log("Doing Explode counter rotation");
        StartRotatingTo(_resetToRotation);
    }

    public void StartRotatingTo(Quaternion d, float timeItTakes = 1f)
    {
        StartCoroutine(RotateTo(d));
    }

    private IEnumerator RotateTo(Quaternion d)
    {
        float angle = Quaternion.Angle(_cachedTransform.rotation, d);
        while ( angle > _anglePrecision)
        {
            //Debug.Log(angle);
            //_cachedTransform.rotation = Quaternion.RotateTowards(_cachedTransform.rotation, d, _resetRotationSpeed * Time.deltaTime);
            _cachedTransform.Rotate(_rotationVector, _resetRotationSpeed * (_turnRight ? 1f : -1f) * Time.deltaTime);
            angle = Quaternion.Angle(_cachedTransform.rotation, d);
            yield return new WaitForEndOfFrame();
        }
        _shouldRotate = false;
        EventManager.TriggerPositionReseted();
        Debug.Log("Reached zero");
    }
}