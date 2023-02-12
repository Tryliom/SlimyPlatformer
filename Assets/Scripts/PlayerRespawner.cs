using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    private Animator _animator;
    
    private Vector2 _lastCheckpointPosition;
    
    private static readonly int Respawn1 = Animator.StringToHash("Respawn");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        
        // Set the last checkpoint position to the player's starting position
        _lastCheckpointPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint") && !other.GetComponent<CheckpointController>().IsActivated())
        {
            _lastCheckpointPosition = other.transform.position;
            
            other.GetComponent<CheckpointController>().Activate();
        }
    }
    
    public void Respawn()
    {
        transform.position = _lastCheckpointPosition;
        GetComponent<PlayerController>().isDead = false;
        _animator.SetTrigger(Respawn1);
    }
}
