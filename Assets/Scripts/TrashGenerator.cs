using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    [SerializeField] private float _x;
    [SerializeField] private float _y;
    [SerializeField] private int _trashAmount;
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

        for (int i = 0; i < _trashAmount; i++)
        {
            var obj = new GameObject("Trash", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
            obj.sprite = _objects[Random.Range(0, _objects.Count - 1)];
            obj.transform.position = GetRandomPosition();
            obj.transform.parent = transform;
        }
    }

    private Vector3 GetRandomPosition()
    {
        var p = transform.position;
        float x = p.x + Random.Range(-_x, _x);
        float y = p.y + Random.Range(-_y, _y);
        return new Vector3(x, y, p.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var p = transform.position;
        Gizmos.DrawLine(new Vector3(p.x - _x, p.y - _y, p.z), new Vector3(p.x + _x, p.y - _y, p.z));
        Gizmos.DrawLine(new Vector3(p.x - _x, p.y + _y, p.z), new Vector3(p.x + _x, p.y + _y, p.z));
        Gizmos.DrawLine(new Vector3(p.x - _x, p.y - _y, p.z), new Vector3(p.x - _x, p.y + _y, p.z));
        Gizmos.DrawLine(new Vector3(p.x + _x, p.y - _y, p.z), new Vector3(p.x + _x, p.y + _y, p.z));
    }
}
