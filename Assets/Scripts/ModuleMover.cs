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
    private Transform _targetModule;
    [SerializeField]
    private Transform _slot;
    [SerializeField]
    private Transform[] _targetPoints;
    [SerializeField]
    private float _distanceThreshold;
    [SerializeField]
    private float _movementSpeed;

    private Transform _lastPoint;

    private int _reachedPoints;
    private Vector3 _nextTarget;

    private bool _shouldMove;

	// Use this for initialization
	void Start () {
        if (_targetPoints.Length == 0)
        {
            Debug.LogError("Target waypoints must be attached");
            return;
        }
        if (_targetModule == null || _slot == null)
        {
            Debug.LogError("TargetModule or Slot is not assigned");
            return;
        }
        SetupForJourneyToTheDisplayPosition();
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
        
        ChangeMovementStateOfModule(true);
        
    }

    public void StartGoingToDisplayPosition()
    {
        if(_targetPoints[_targetPoints.Length-1] == _slot)
        {
            _targetPoints = ReverseArrayReplaceLastOne<Transform>(_targetPoints, _lastPoint);
        }
        SetupForJourneyToTheDisplayPosition();
        _currentPosition = ModulePositions.GoingToDisplayPosition;
    }

    public void StartGoingToSystem()
    {        
        _targetPoints = ReverseArrayReplaceLastOne<Transform>(_targetPoints, _slot);
        _reachedPoints = 0;
        _nextTarget = _targetPoints[_reachedPoints].position;

        ChangeMovementStateOfModule(true);
        //todo
    }
    
    private T[] ReverseArrayReplaceLastOne<T>(T[] arr, T lastOne)
    {
        int count = arr.Length;
        T[] reverseArray = new T[count];
        for (int i = count-1; i > 0; i++)
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
            float currentDistance = Vector3.Distance(_nextTarget, _targetModule.position);

            if (currentDistance < _distanceThreshold) // if arrived
            {
                _reachedPoints++;
                if (_reachedPoints >= _targetPoints.Length)
                {
                    //reached final destination
                    _shouldMove = false;
                    if (_currentPosition == ModulePositions.GoingToDisplayPosition) _currentPosition = ModulePositions.OutsideIdle;
                    else if (_currentPosition == ModulePositions.GoingInToSystem) _currentPosition = ModulePositions.InsideIdle;
                    else Debug.LogError("wrong position");
                    return;
                }

                _nextTarget = _targetPoints[_reachedPoints].position;
                Debug.Log("reached a point. current distance >" + currentDistance);
            }
            else
            {
                _targetModule.LookAt(_nextTarget);
                _targetModule.Translate(_targetModule.forward * _movementSpeed * Time.deltaTime, Space.World);
            }

        }
    }
}
