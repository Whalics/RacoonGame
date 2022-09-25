using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _trashPrefab;
    [SerializeField] private LayerMask _trashLayer;
    
    [SerializeField] private List<WeightedTrash> _trash;
    [SerializeField] private float _xDist = 8;
    [SerializeField] private float _yDist = 5;
    [SerializeField] private float _separation = 2.5f;
    [SerializeField] private float _random = 1.5f;

    [SerializeField] private float _layerCount = 8;
    [SerializeField] private float _zMin = 1;
    [SerializeField] private float _zMax = 8;

    [SerializeField] private int _extraRandomObjects = 50;
    [SerializeField] private List<Trash> _individualTrash;

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

        var individualObj = new GameObject("Random").transform;
        individualObj.parent = transform;
        foreach (var trash in _individualTrash)
        {
            if (trash == null) continue;
            float x = Random.Range(p.x - _xDist, p.x + _xDist);
            float y = Random.Range(p.y - _yDist, p.y + _yDist);
            float z = Random.Range(_zMin, _zMax);
            var obj = Instantiate(trash, individualObj);
            obj.transform.position = ClampBounds(new Vector3(x, y, z));
            obj.SetController(this);
        }
    }
    
    private void GenerateLayer(float z)
    {
        var layerObj = new GameObject("Layer: " + z).transform;
        var p = transform.position;
        for (float x = p.x - _xDist; x <= p.x + _xDist; x += _separation)
        {
            for (float y = p.y - _yDist; y <= p.y + _yDist; y += _separation)
            {
                var pos = new Vector3(x, y, z) + _random * (Vector3)Random.insideUnitCircle;
                CreateNewTrash(layerObj, pos);
            }
        }
    }

    private void CreateNewTrash(Transform parent, Vector3 pos)
    {
        parent.parent = transform;
        var prefab = GetRandomTrash();
        if (prefab == null) return;
        var trash = Instantiate(prefab, parent);
        trash.transform.position = ClampBounds(pos);
        trash.SetController(this);
    }

    private Trash GetRandomTrash()
    {
        float weightSum = 0f;
        for (int i = 0; i < _trash.Count; ++i)
        {
            weightSum += _trash[i].Weight;
        }
        int index = 0;
        while (index < _trash.Count)
        {
            if (Random.Range(0, weightSum) < _trash[index].Weight)
            {
                return _trash[index].Trash;
            }
            weightSum -= _trash[index++].Weight;
        }
        Debug.Log("Error", gameObject);
        return null;
    }
}
