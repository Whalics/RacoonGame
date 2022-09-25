using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _trashPrefab;
    [SerializeField] private LayerMask _trashLayer;
    [SerializeField] private List<Sprite> _objects;
    
    [SerializeField] private float _xDist;
    [SerializeField] private float _yDist;
    [SerializeField] private float _separation;
    [SerializeField] private float _random;

    [SerializeField] private float _layerCount;
    [SerializeField] private float _zMin;
    [SerializeField] private float _zMax;

    [SerializeField] private int _extraRandomObjects;

    public LayerMask TrashLayer => _trashLayer;

    private void Start()
    {
        Generate();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var pos = transform.position;
        pos.z = _zMin + (_zMax - _zMin) * 0.5f;
        Gizmos.DrawWireCube(pos, new Vector3(2 * _xDist, 2 * _yDist, _zMax - _zMin));
    }

    public Vector3 ClampBounds(Vector3 pos)
    {
        var p = transform.position;
        pos.x = Mathf.Clamp(pos.x, p.x - _xDist, p.x + _xDist);
        pos.y = Mathf.Clamp(pos.y, p.y - _yDist, p.y + _yDist);
        pos.z = Mathf.Clamp(pos.z, _zMin, _zMax);
        return pos;
    }

    [Button(Mode = ButtonMode.InPlayMode)]
    private void Generate()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _layerCount; i++)
        {
            float z = _zMin + i * (_zMax - _zMin) / (_layerCount - 1);
            GenerateLayer(z);
        }
        
        var randomObj = new GameObject("Random").transform;
        var p = transform.position;
        for (int i = 0; i < _extraRandomObjects; i++)
        {
            float x = Random.Range(p.x - _xDist, p.x + _xDist);
            float y = Random.Range(p.y - _yDist, p.y + _yDist);
            float z = Random.Range(_zMin, _zMax);
            var pos = new Vector3(x, y, z);
            CreateNewTrash(randomObj, pos);
        }
    }
    
    private void GenerateLayer(float z)
    {
        var layerObj = new GameObject("Layer: " + z).transform;
        var p = transform.position;
        for (float x = p.x - _xDist; x < p.x + _xDist; x += _separation)
        {
            for (float y = p.y - _yDist; y < p.y + _yDist; y += _separation)
            {
                var pos = new Vector3(x, y, z) + _random * (Vector3)Random.insideUnitCircle;
                CreateNewTrash(layerObj, pos);
            }
        }
    }

    private void CreateNewTrash(Transform parent, Vector3 pos)
    {
        parent.parent = transform;
        var trash = Instantiate(_trashPrefab, parent);
        trash.sprite = _objects[Random.Range(0, _objects.Count - 1)];
        trash.transform.position = ClampBounds(pos);
        trash.GetComponent<Trash>().SetController(this);
    }
}
