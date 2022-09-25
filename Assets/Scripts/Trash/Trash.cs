using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Trash : MonoBehaviour
{
    public static List<Trash> AllTrash = new List<Trash>();

    [SerializeField] private TrashController _controller;
    [SerializeField] private float _speed = 4;
    [SerializeField] private float _returnSpeed = 0.02f;
    [SerializeField, Range(0, 1)] private float _drag = 0.8f;
    [SerializeField] private float _dragSpeed = 10;
    [SerializeField] private float _timeToMove = 4;
    [SerializeField] private float _timeToMoveVariation = 4;
    [SerializeField, ReadOnly] private Vector3 _vel;
    [SerializeField] private List<string> _clickSounds;

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
        
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, 360f)));
    }

    protected virtual void Update()
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

    public virtual void Push(Vector3 dir)
    {
        var vel = dir.normalized;
        transform.position = _controller.ClampBounds(transform.position + dir);
        _vel = vel;
        _pushedLastFrame = true;
        _lastTimePushed = Time.time;
        _returnToHome = false;
        _returnHomeTime = _timeToMove + Random.Range(-_timeToMoveVariation, _timeToMoveVariation);
    }

    public void PlaySound()
    {
        if (_clickSounds.Count == 0) return;
        AudioManager.PlaySound(_clickSounds[Random.Range(0, _clickSounds.Count)]);
    }
}
