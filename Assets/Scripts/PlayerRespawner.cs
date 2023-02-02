using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    private Animator _animator;
    
    private static readonly int Respawn1 = Animator.StringToHash("Respawn");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Respawn()
    {
        _animator.SetBool(Respawn1, true);
    }
}
