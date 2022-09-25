using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TrashEdible : Trash
{
	public static int EdibleItemsRemaining = -1;
    
    [SerializeField] private bool _randomizePosition;
    [SerializeField] private List<string> _eatSounds;
    [SerializeField] private List<string> _munchSounds;
    private SpriteRenderer _renderer;
    private bool _eat;
    private HandsController _hands;

    private void Awake()
    {
        _hands = FindObjectOfType<HandsController>();
        _renderer = GetComponent<SpriteRenderer>();
        EdibleItemsRemaining++;
    }
    
    protected override void Start()
    {
        if (_randomizePosition)
        {
            var pos = _controller.transform.position;
            pos.x = Random.Range(pos.x - _controller.DistX, pos.x + _controller.DistX);
            pos.y = Random.Range(pos.y - _controller.DistY, pos.y + _controller.DistY);
            pos.z = transform.position.z;
            transform.position = pos;
        }
        base.Start();
    }

    protected override void Update()
    {
        if (_eat) return;
        base.Update();
    }
    
    public override void PlaySound()
    {
        if (_eat) return;
        _eat = true;
        StartCoroutine(AnimateToFront());
        _hands.StartEatItem();
        AllTrash.Remove(this);
        GetComponent<Collider>().enabled = false;
    }

    private IEnumerator AnimateToFront()
    {
        transform.DOMove(new Vector3(0, 0, -2), 1.5f);
        transform.DOScale(transform.localScale * 1.5f, 1.5f);
        
        yield return new WaitForSeconds(1.5f);

        var pos = transform.position;
        Color c = Color.white;
        
        if (_eatSounds.Count > 0)
            AudioManager.PlaySound(_eatSounds[Random.Range(0, _eatSounds.Count)]);
        if (_munchSounds.Count > 0)
            AudioManager.PlaySound(_munchSounds[Random.Range(0, _munchSounds.Count)]);
        
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            var y = pos.y + Mathf.Sin(t * 24) * 0.4f -4 * t;
            transform.position = new Vector3(pos.x, y, pos.z);
            c.a = 1 - t;
            _renderer.color = c;
            yield return null;
        }
        EdibleItemsRemaining--;
        Destroy(gameObject);
    }
}
