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
    // [SerializeField] private float moveAcceleration; // We can see about adding this back

    private Vector3 _moveVector;
    public Vector3 MoveVector { get { return _moveVector * moveSpeed; } }

    bool boost; // This was to test an interaction, ignore me

    private void Awake()
    {
        _characterController = this.GetComponent<CharacterController>();
        _playerInteraction = this.GetComponent<PlayerInteraction>();

        _transform = this.transform;
        _camTransform = Camera.main.transform;
        boost = false;
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

        if (false && Input.GetKeyDown(KeyCode.Space)) // This was to test an interaction, ignore me
        {
            boost = true;
        }

        CheckInteraction();
    }

    private void FixedUpdate()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLISION VECTOR: " + collision.contactCount);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("COLLISION VECTOR: " + hit.rigidbody.velocity.magnitude);

        Rigidbody body = hit.collider.attachedRigidbody;

        if(body != null && !body.isKinematic)
        {
            Vector3 appliedForce = ((_moveVector * moveSpeed - hit.rigidbody.velocity) * hit.rigidbody.mass);

            //hit.rigidbody.AddForce((hit.point - _transform.position).normalized * 15);
            //hit.rigidbody.AddForce(appliedForce, ForceMode.Force);

            //hit.rigidbody.AddForce(moveVector*moveSpeed, ForceMode.Force);

            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            //hit.rigidbody.AddForce(pushDir * moveSpeed, ForceMode.Force);
            body.velocity += pushDir/2;
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
