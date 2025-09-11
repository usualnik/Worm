using System;
using UnityEngine;

public class WormBodyPart : MonoBehaviour
{
    [SerializeField] private bool _isGrounded;

    private WormBody wormBody;

    private void Start()
    {
        wormBody = GetComponentInParent<WormBody>();
    }


    private void OnTriggerEnter2D(Collider2D collider2d)
    {
        if (collider2d.CompareTag("Ground"))
        {
            _isGrounded = true;
            wormBody.BodyPartGrounded(_isGrounded);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
           
            _isGrounded = false;
            wormBody.BodyPartGrounded(_isGrounded);

        }
    }

    public bool GetGrounded()
    {
        return _isGrounded;
    }
}
