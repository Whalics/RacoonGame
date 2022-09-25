using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrashGenerator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _trashPrefab;
    [SerializeField] private float _xDist;
    [SerializeField] private int _xAmount;
    [SerializeField] private float _yDist;
    [SerializeField] private int _yAmount;
    [SerializeField] private float _random;
    [SerializeField] private List<Sprite> _objects;

    private void Start()
    {
        Generate();
    }

    [Button(Mode = ButtonMode.InPlayMode)]
    private void Generate()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var p = transform.position;
        for (float x = 0; x <= _xAmount; x ++)
        {
            float xPos = p.x - _xDist + x * 2 * _xDist / _xAmount;
            for (float y = 0; y <= _yAmount; y++)
            {
                float yPos = p.y - _yDist + y * 2 * _yDist / _yAmount;
                var pos = new Vector3(xPos, yPos, p.z) + _random * (Vector3)Random.insideUnitCircle;
                CreateNewTrash(pos);
            }
        }
    }

    private void CreateNewTrash(Vector3 pos)
    {
        var trash = Instantiate(_trashPrefab, transform);
        trash.sprite = _objects[Random.Range(0, _objects.Count - 1)];
        trash.transform.position = pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var p = transform.position;
        Gizmos.DrawLine(new Vector3(p.x - _xDist, p.y - _yDist, p.z), new Vector3(p.x + _xDist, p.y - _yDist, p.z));
        Gizmos.DrawLine(new Vector3(p.x - _xDist, p.y + _yDist, p.z), new Vector3(p.x + _xDist, p.y + _yDist, p.z));
        Gizmos.DrawLine(new Vector3(p.x - _xDist, p.y - _yDist, p.z), new Vector3(p.x - _xDist, p.y + _yDist, p.z));
        Gizmos.DrawLine(new Vector3(p.x + _xDist, p.y - _yDist, p.z), new Vector3(p.x + _xDist, p.y + _yDist, p.z));
    }
}
