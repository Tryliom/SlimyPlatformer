using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDash : MonoBehaviour
{
    private Animator _animator;
    
    private static readonly int Contact = Animator.StringToHash("Contact");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetTrigger(Contact);
        }
    }
    
    public string GetUniqueId()
    {
        return gameObject.GetInstanceID().ToString();
    }
}
