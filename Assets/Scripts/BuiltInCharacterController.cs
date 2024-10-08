using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltInCharacterController : MonoBehaviour
{
    CharacterController _characterController;
    Transform _transform;
    Transform _camTransform;

    PlayerInteraction _playerInteraction;
    public PlayerInteraction PlayerInteraction { get { return _playerInteraction; } }

    [SerializeField] private float moveSpeed;

    private Vector3 _moveVector;
    public Vector3 MoveVector { get { return _moveVector * moveSpeed; } }

    private void Awake()
    {
        _characterController = this.GetComponent<CharacterController>();
        _playerInteraction = this.GetComponent<PlayerInteraction>();

        _transform = this.transform;
        _camTransform = Camera.main.transform;
    }

    void Start()
    {

    }

    void Update()
    {
        // Moving stuff
        _moveVector = Vector3.zero;
        _moveVector += _camTransform.forward * Input.GetAxisRaw("Vertical");
        _moveVector += _camTransform.right * Input.GetAxisRaw("Horizontal");
        _moveVector.y = 0; // So we don't move up if we're pointing up, etc
        _moveVector.Normalize(); // For if we move in a diagonal

        bool t = _characterController.SimpleMove(_moveVector * moveSpeed);

        CheckInteraction();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("COLLISION VECTOR: " + hit.rigidbody.velocity.magnitude);

        Rigidbody body = hit.collider.attachedRigidbody;

        if(body != null && !body.isKinematic)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z); // Gets direction we should push object
            body.velocity += pushDir/2; // Scaled it down a little to make it feel better, we can adjust this
        }
        else
        {
            // Shit is static yo
        }
    }

    private void CheckInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(_playerInteraction.heldInteractable == null) // We're not holding something
            {
                _playerInteraction.ActivateCurrentInteractionTarget();
            }
            else // We're holding something
            {
                _playerInteraction.DropInteractable();
            }
        }
    }
}
