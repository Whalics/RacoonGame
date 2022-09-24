using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField, ReadOnly] private Vector3 _vel;
    
    private void Update()
    {
        transform.position += _vel * (_speed * Time.deltaTime);
    }
}
