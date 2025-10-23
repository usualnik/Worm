using System;
using UnityEngine;

public class WormBody : MonoBehaviour
{
    public event Action OnBodyNotGrounded;

    [SerializeField] private WormBodyPart[] _bodyParts;
    [SerializeField] private int _bodyPartGroundedCounter;

    private WormMovement _wormMovement;
    private const float DISTANCE_BETWEEN_BODY_PARTS = 0.01f;

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
        else
        {            
            _bodyPartGroundedCounter--;
            CheckIfAllBodyPartsGrounded();
        }
    }

    private void CheckIfAllBodyPartsGrounded()
    {
        if (_bodyPartGroundedCounter == 0)
        {
            OnBodyNotGrounded?.Invoke();
        }
    }

    private void WormMovement_OnMove(Vector3 movementVector)
    {
        _bodyParts[0].transform.position += movementVector * DISTANCE_BETWEEN_BODY_PARTS;
        _bodyParts[1].transform.position = _prevHeadPos;
        _bodyParts[2].transform.position = _prevBodyPos;

        _prevHeadPos = _bodyParts[0].transform.position;
        _prevBodyPos = _bodyParts[1].transform.position;
        _prevTailPos= _bodyParts[2].transform.position;
    }
      
    public void UpdateBodyPosDuringFall()
    {
        _prevHeadPos = _bodyParts[0].transform.position;
        _prevBodyPos = _bodyParts[1].transform.position;
        _prevTailPos = _bodyParts[2].transform.position;
    }
    public void UndoBodyPos(Vector3[] positions)
    {
        _bodyParts[0].transform.position = positions[1];
        _bodyParts[1].transform.position = positions[2];
        _bodyParts[2].transform.position = positions[3];

        _prevHeadPos = _bodyParts[0].transform.position;
        _prevBodyPos = _bodyParts[1].transform.position;
        _prevTailPos = _bodyParts[2].transform.position;
    }

    public GameObject GetWormHead()
    {
        return _bodyParts[0].gameObject;
    }
    public WormBodyPart[] GetBodyParts()
    {
        return _bodyParts;
    }
    public void FlipHeadSprite(bool shouldFlip)
    {
        var headSpriteRenderer =  _bodyParts[0].GetComponent<SpriteRenderer>();        
        headSpriteRenderer.flipX = shouldFlip;
    }
}
