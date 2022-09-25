using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandsController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private TrashController _trashController;
    [SerializeField] private SusController _susController;
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _influenceRadius = 2;
    [SerializeField, Range(0, 1)] private float _influenceDrag = 0.1f;
    [SerializeField, ReadOnly] private bool _drag;
    [SerializeField, ReadOnly] private float _zTimeUpdate = 1;

    [Header("Left")] 
    [SerializeField, HighlightIfNull] private Transform _left;
    [SerializeField] private Vector3 _leftRest;
    [SerializeField] private Vector3 _leftDrag;
    
    [Header("Right")]
    [SerializeField, HighlightIfNull] private Transform _right;
    [SerializeField] private Vector3 _rightRest;
    [SerializeField] private Vector3 _rightDrag;
    
    [Header("Art")]
    [SerializeField] private List<GameObject> _openArt;
    [SerializeField] private List<GameObject> _closedArt;

    private List<TrashGrab> _nearbyTrash;
    private Vector3 _mousePrev;
    private Vector3 _mouseOffset;
    private float _depthZ;
    private float _timeZChecked;
    
    private void OnEnable()
    {
        UserInput.Drag += Drag;
    }

    private void OnDisable()
    {
        UserInput.Drag -= Drag;
    }

    private void Drag(bool drag)
    {
        if (_cameraController.CanDigInTrash) _susController.IncreaseSus(0.2f);

        _drag = drag;
        var pos = _trashController.ClampBounds(_mousePrev);
        GetZDist(pos.x, pos.y, true);

        bool dragging = drag && _cameraController.CanDigInTrash;
        foreach (var obj in _openArt)
        {
            obj.SetActive(!dragging);
        }
        foreach (var obj in _closedArt)
        {
            obj.SetActive(dragging);
        }
    }

    private void OnValidate()
    {
        if (_cameraController == null) _cameraController = FindObjectOfType<CameraController>();
        if (_trashController == null) _trashController = FindObjectOfType<TrashController>();
        if (_susController == null) _susController = FindObjectOfType<SusController>();
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
        if (Time.time - _timeZChecked > _zTimeUpdate)
        {
            var pos = _trashController.ClampBounds(_mousePrev);
            GetZDist(pos.x, pos.y);
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
        pos.z = _depthZ;
        foreach (var trash in Trash.AllTrash)
        {
            float dist = Vector3.Distance(pos, trash.transform.position);
            if (dist < _influenceRadius)
            {
                _nearbyTrash.Add(new TrashGrab(trash, dist));
            }
        }
    }

    private void GetZDist(float x, float y, bool playSound = false)
    {
        _timeZChecked = Time.time;
        var origin = _trashController.ClampBounds(new Vector3(x, y, 0));
        origin.z = -10;
        Physics.Raycast(origin, Vector3.forward, out var hit, 20, _trashController.TrashLayer);
        if (hit.collider)
        {
            _depthZ = hit.point.z;
            if (playSound) hit.collider.GetComponent<Trash>().PlaySound();
        }
    }

    private void MoveTrash()
    {
        foreach (var trash in _nearbyTrash)
        {
            trash.Trash.Push(_mouseOffset * Mathf.Clamp01(_influenceDrag + 1 - trash.Dist / _influenceRadius));
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