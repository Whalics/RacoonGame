using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandsController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private float _speed;
    [SerializeField, ReadOnly] private bool _drag;
    
    [Header("Left")]
    [SerializeField, HighlightIfNull] private Transform _left;
    [SerializeField] private Vector3 _leftRest;
    [SerializeField] private Vector3 _leftDrag;
    
    [Header("Right")]
    [SerializeField, HighlightIfNull] private Transform _right;
    [SerializeField] private Vector3 _rightRest;
    [SerializeField] private Vector3 _rightDrag;
    
    [Button(Spacing = 10)]
    private void SetRestPos()
    {
        _leftRest = _left.position;
        _rightRest = _right.position;
    }

    [Button]
    private void SetDragPos()
    {
        _leftDrag = _left.position;
        _rightDrag = _right.position;
    }

    private void OnEnable()
    {
        UserInput.Drag += Drag;
    }
    private void OnDisable()
    {
        UserInput.Drag -= Drag;
    }
    private void Drag(bool drag) => _drag = drag;

    private void OnValidate()
    {
        if (_cameraController == null) _cameraController = FindObjectOfType<CameraController>();
    }
    
    private void Update()
    {
        if (_cameraController.CanDigInTrash)
        {
            UpdatePosition();
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
    }
}
