using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashEdible : Trash
{
    private bool _eat;
    
    protected override void Update()
    {
        if (_eat) return;
        base.Update();
    }
    
    public override void Push(Vector3 dir)
    {
        _eat = true;
        StartCoroutine(AnimateToFront());
        AllTrash.Remove(this);
        GetComponent<Collider>().enabled = false;
    }

    private IEnumerator AnimateToFront()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
