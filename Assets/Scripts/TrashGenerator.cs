using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
    [SerializeField] private float _x;
    [SerializeField] private float _y;
    [SerializeField] private List<Sprite> _objects;

    private Vector3 GetRandomPosition()
    {
        var p = transform.position;
        var x = p.x + Random.Range(-_x, _x);
        var y = p.y + Random.Range(-_y, _y);
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
