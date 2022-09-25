using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandsController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private TrashController _trashController;
    [SerializeField] private float _speed;
    [SerializeField] private float _influenceRadius;
    [SerializeField, ReadOnly] private bool _drag;
    
    [Header("Left")]
    [SerializeField, HighlightIfNull] private Transform _left;
    [SerializeField] private Vector3 _leftRest;
    [SerializeField] private Vector3 _leftDrag;
    
    [Header("Right")]
    [SerializeField, HighlightIfNull] private Transform _right;
    [SerializeField] private Vector3 _rightRest;
    [SerializeField] private Vector3 _rightDrag;

    private List<TrashGrab> _nearbyTrash;
    private Vector3 _mousePrev;
    private Vector3 _mouseOffset;


    private SusController suscontroller;

    private void OnEnable()
    {
        suscontroller = GameObject.Find("SusController").GetComponent<SusController>();
        UserInput.Drag += Drag;
    }

    private void OnDisable()
    {
        UserInput.Drag -= Drag;
    }

    private void Drag(bool drag)
    {
        suscontroller.IncreaseSus(0.2f);

        _drag = drag;
    }

    private void OnValidate()
    {
        if (_cameraController == null) _cameraController = FindObjectOfType<CameraController>();
        if (_trashController == null) _trashController = FindObjectOfType<TrashController>();
    }
    
    private void Update()
    {
        if (_cameraController.CanDigInTrash)
        {
            UpdatePosition();
            if (_drag)
            {
                UpdateNearbyTrash();
                MoveTrash();
            }
        }
    }

    private void UpdatePosition()
    {
        var mPos = _cameraController.MainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mPos.z = transform.position.z;
        
        var leftGoal = _drag ? _leftDrag + mPos : _leftRest;
        _left.position = Vector3.Lerp(_left.position, leftGoal, _speed * Time.deltaTime);
        
        var rightGoal = _drag ? _rightDrag + mPos : _rightRest;
        _right.position = Vector3.Lerp(_right.position, rightGoal, _speed * Time.deltaTime);

        _mouseOffset = mPos - _mousePrev;
        _mousePrev = mPos;
    }

    private void UpdateNearbyTrash()
    {
        _nearbyTrash = new List<TrashGrab>();
        var pos = _mousePrev;
        pos.z = GetZDist(pos.x, pos.y);
        foreach (var trash in Trash.AllTrash)
        {
            float dist = Vector3.Distance(pos, trash.transform.position);
            if (dist < _influenceRadius)
            {
                _nearbyTrash.Add(new TrashGrab(trash, dist));
            }
        }
    }

    private float GetZDist(float x, float y)
    {
        var origin = _trashController.ClampBounds(new Vector3(x, y, 0));
        origin.z = -10;
        Physics.Raycast(origin, Vector3.forward, out var hit, 20, _trashController.TrashLayer);
        if (hit.collider)
        {
            return hit.point.z;
        }
        return 0;
    }

    private void MoveTrash()
    {
        foreach (var trash in _nearbyTrash)
        {
            trash.Trash.Push(_mouseOffset * (1 - trash.Dist / _influenceRadius));
        }
    }
}

internal struct TrashGrab
{
    public Trash Trash;
    public float Dist;

    public TrashGrab(Trash trash, float dist)
    {
        Trash = trash;
        Dist = dist;
    }
}