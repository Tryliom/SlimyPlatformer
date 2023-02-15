using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashItem : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private GameObject _showOnCollect;

    // For animation
    public float _yPosition;
    private Vector2 _position;
    
    private Animator _animator;
    
    private static readonly int Collected = Animator.StringToHash("Collected");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        
        _position = transform.position;
        _showOnCollect.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(_position.x, _position.y + _yPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerData.UnlockDash();
            _showOnCollect.SetActive(true);
            _animator.SetTrigger(Collected);
        }
    }
    
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
