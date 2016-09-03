using UnityEngine;
using System.Collections;

public class ModuleMover : MonoBehaviour {

    private enum ModulePositions
    {
        InsideIdle,
        GoingToDisplayPosition,
        OutsideIdle,
        GoingInToSystem
    }

    private ModulePositions _currentPosition;

    [SerializeField]
    private Transform _moduleToBeMoved;
    [SerializeField]
    private Transform _slot;
    [SerializeField]
    private Transform[] _targetPoints;
    [SerializeField]
    private float _distanceThreshold;
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private bool _shouldLookAtTargetPoint = true;


    [SerializeField] private float _resetRotationSpeed = 100f;

    private Transform _lastPoint;

    private int _reachedPoints;
    private Vector3 _nextTarget;

    private bool _shouldMove;
    [SerializeField]
    private bool goInReverse;

	// Use this for initialization
	void Start () {
        if (_targetPoints.Length == 0)
        {
            Debug.LogError("Target waypoints must be attached");
            return;
        }
        if (_moduleToBeMoved == null || _slot == null)
        {
            Debug.LogError("TargetModule or Slot is not assigned");
            return;
        }
        SetupForJourneyToTheDisplayPosition();
        ChangeMovementStateOfModule(false);
        _lastPoint = _targetPoints[_targetPoints.Length - 1];
        _currentPosition = ModulePositions.InsideIdle;
    }

    public void ChangeMovementStateOfModule(bool b)
    {
        _shouldMove = b;
    }

    private void SetupForJourneyToTheDisplayPosition()
    {
        _reachedPoints = 0;
        _nextTarget = _targetPoints[_reachedPoints].position;
    }

    public void StartGoingToDisplayPosition()
    {
        if(_targetPoints[_targetPoints.Length-1] == _slot)
        {
            _targetPoints = ReverseArrayReplaceLastOne<Transform>(_targetPoints, _lastPoint);
        }
        goInReverse = false;
        SetupForJourneyToTheDisplayPosition();
        ChangeMovementStateOfModule(true);
        _currentPosition = ModulePositions.GoingToDisplayPosition;
        
    }

    public void StartGoingToSystem()
    {        
        _targetPoints = ReverseArrayReplaceLastOne<Transform>(_targetPoints, _slot);
        _reachedPoints = 0;
        _nextTarget = _targetPoints[_reachedPoints].position;

        goInReverse = true;
        ChangeMovementStateOfModule(true);
        _currentPosition = ModulePositions.GoingInToSystem;
        //todo
    }
    
    private T[] ReverseArrayReplaceLastOne<T>(T[] arr, T lastOne)
    {
        int count = arr.Length;
        T[] reverseArray = new T[count];
        for (int i = count-1; i > 0; i--)
        {
            reverseArray[count - i] = arr[i];
        }

        reverseArray[count - 1] = lastOne;

        return reverseArray;
    }

    void Update()
    {
        switch (_currentPosition)
        {
            case ModulePositions.InsideIdle:
                break;
            case ModulePositions.GoingToDisplayPosition:
                Move();
                break;
            case ModulePositions.OutsideIdle:
                break;
            case ModulePositions.GoingInToSystem:
                Move();
                break;
            default:
                break;
        }
        
    }

    private void Move()
    {
        if (_shouldMove)
        {
            float currentDistance = Vector3.Distance(_nextTarget, _moduleToBeMoved.position);

            if (currentDistance < _distanceThreshold) // if arrived
            {
                _reachedPoints++;
                if (_reachedPoints >= _targetPoints.Length)
                {
                    //reached final destination
                    _shouldMove = false;
                    if (_currentPosition == ModulePositions.GoingToDisplayPosition)
                    {

                        //StartCoroutine(RotateJustLikeLastTarget());
                        _currentPosition = ModulePositions.OutsideIdle;
                        EventManager.TriggerModuleArrival();
                    }
                    else if (_currentPosition == ModulePositions.GoingInToSystem) _currentPosition = ModulePositions.InsideIdle;
                    else Debug.LogError("wrong position");
                    return;
                }

                _nextTarget = _targetPoints[_reachedPoints].position;
                Debug.Log("reached a point. current distance >" + currentDistance);
            }
            else
            {
                if (_shouldLookAtTargetPoint)
                {
                    
                    if (goInReverse)
                    {
                        _moduleToBeMoved.LookAt(2 * _moduleToBeMoved.position - _nextTarget);
                    }
                    else _moduleToBeMoved.LookAt(_nextTarget);

                }
                _moduleToBeMoved.Translate(_moduleToBeMoved.forward * _movementSpeed * Time.deltaTime * (goInReverse?-1:1), Space.World);
            }

        }
    }


    private IEnumerator RotateJustLikeLastTarget()
    {
        while (_moduleToBeMoved.rotation != _lastPoint.rotation)
        {
            _moduleToBeMoved.rotation = Quaternion.RotateTowards(_moduleToBeMoved.rotation, _lastPoint.rotation, _resetRotationSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        _currentPosition = ModulePositions.OutsideIdle;
        EventManager.TriggerModuleArrival();
    }

}
