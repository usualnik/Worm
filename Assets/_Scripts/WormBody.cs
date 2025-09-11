using System;
using UnityEngine;

public class WormBody : MonoBehaviour
{
    public event Action OnBodyNotGrounded;

    [SerializeField] private WormBodyPart[] _bodyParts;
    [SerializeField] private int _bodyPartGroundedCounter;

    private WormMovement _wormMovement;
    private const float DISTANCE_BETWEEN_BODY_PARTS = 0.1f;

    private Vector3 _prevHeadPos;
    private Vector3 _prevBodyPos;
    private Vector3 _prevTailPos;


    private void Start()
    {
        _wormMovement = GetComponentInParent<WormMovement>();

        //Events
        _wormMovement.OnMove += WormMovement_OnMove;
    

        //Initialization
        _prevHeadPos = _bodyParts[0].transform.position;
        _prevBodyPos = _bodyParts[1].transform.position;
        _prevTailPos = _bodyParts[2].transform.position;



    }


    private void OnDestroy()
    {
        _wormMovement.OnMove -= WormMovement_OnMove;

       
    }

    public void BodyPartGrounded(bool isGrounded)
    {
        if (isGrounded)
        {
            _bodyPartGroundedCounter++;
            CheckIfAllBodyPartsGrounded();
        }
        else{
            
            _bodyPartGroundedCounter--;
            CheckIfAllBodyPartsGrounded();

        }
    }

    private void CheckIfAllBodyPartsGrounded()
    {
        if (_bodyPartGroundedCounter == _bodyParts.Length)
        {
            OnBodyNotGrounded?.Invoke();
        }
    }

    private void WormMovement_OnMove(Vector3 obj)
    {
        _bodyParts[0].transform.position += obj * DISTANCE_BETWEEN_BODY_PARTS;
        _bodyParts[1].transform.position = _prevHeadPos;
        _bodyParts[2].transform.position = _prevBodyPos;

        _prevHeadPos = _bodyParts[0].transform.position;
        _prevBodyPos = _bodyParts[1].transform.position;
        _prevTailPos= _bodyParts[2].transform.position;
    }

    public GameObject GetWormHead()
    {
        return _bodyParts[0].gameObject;
    }
    public WormBodyPart[] GetBodyParts()
    {
        return _bodyParts;
    }
}
