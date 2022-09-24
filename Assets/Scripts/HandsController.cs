using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandsController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private float _speed;
    [SerializeField] private float _influenceRadius;
    [SerializeField] private float _pushForce;
    [SerializeField, ReadOnly] private bool _drag;
    
    [Header("Left")]
    [SerializeField, HighlightIfNull] private Transform _left;
    [SerializeField] private Vector3 _leftRest;
    [SerializeField] private Vector3 _leftDrag;
    
    [Header("Right")]
    [SerializeField, HighlightIfNull] private Transform _right;
    [SerializeField] private Vector3 _rightRest;
    [SerializeField] private Vector3 _rightDrag;

    private List<Trash> _nearbyTrash;
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
        if (drag)
        {
            _nearbyTrash = new List<Trash>();
            var pos = _mousePrev;
            pos.z = 1;  
            foreach (var trash in Trash.AllTrash)
            {
                if (Vector3.Distance(pos, trash.transform.position) < _influenceRadius)
                {
                    _nearbyTrash.Add(trash);
                }
            }
        }
    }

    private void OnValidate()
    {
        if (_cameraController == null) _cameraController = FindObjectOfType<CameraController>();
    }
    
    private void Update()
    {
        if (_cameraController.CanDigInTrash)
        {
            UpdatePosition();
            if (_drag) MoveTrash();
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

    private void MoveTrash()
    {
        foreach (var trash in _nearbyTrash)
        {
            trash.Push(_mouseOffset * _pushForce);
        }
    }
}

internal struct TrashGrab
{
    public Trash Trash;
    public float Dist;
}