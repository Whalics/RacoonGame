using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Trash : MonoBehaviour
{
    public static List<Trash> AllTrash = new List<Trash>();

    [SerializeField] private TrashController _controller;
    [SerializeField] private float _speed;
    [SerializeField] private float _returnSpeed;
    [SerializeField, Range(0, 1)] private float _drag;
    [SerializeField] private float _dragSpeed;
    [SerializeField] private float _timeToMove;
    [SerializeField] private float _timeToMoveVariation;
    [SerializeField, ReadOnly] private Vector3 _vel;

    private Vector3 _homePos;
    private bool _returnToHome;
    private bool _pushedLastFrame;
    private float _lastTimePushed;
    private float _returnHomeTime;

    public void SetController(TrashController controller) => _controller = controller;
    
    private void OnEnable()
    {
        AllTrash.Add(this);
    }
    
    private void OnDisable()
    {
        AllTrash.Remove(this);
    }

    private void Start()
    {
        _homePos = transform.position;
        _returnToHome = true;
    }

    private void Update()
    {
        if (_pushedLastFrame)
        {
            _pushedLastFrame = false;
            return;
        }
        if (_returnToHome)
        {
            transform.position = Vector3.Lerp(transform.position, _homePos, _returnSpeed * Time.deltaTime);
            return;
        }
        
        transform.position = _controller.ClampBounds(transform.position + _vel * (_speed * Time.deltaTime));
        _vel = Vector3.Lerp(_vel, _vel * (1 - _drag), _dragSpeed * Time.deltaTime);

        if (Time.time - _lastTimePushed > _returnHomeTime)
        {
            _returnToHome = true;
        }
    }

    public void Push(Vector3 dir)
    {
        _vel = dir.normalized;
        transform.position += dir;
        _pushedLastFrame = true;
        _lastTimePushed = Time.time;
        _returnToHome = false;
        _returnHomeTime = _timeToMove + Random.Range(-_timeToMoveVariation, _timeToMoveVariation);
    }
}
