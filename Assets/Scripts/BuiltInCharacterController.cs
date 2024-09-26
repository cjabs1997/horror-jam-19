using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltInCharacterController : MonoBehaviour
{
    CharacterController _characterController;
    Transform _transform;
    Transform _camTransform;

    [SerializeField] private float moveSpeed;
    // [SerializeField] private float moveAcceleration; // We can see about adding this back

    Vector3 moveVector;
    bool boost; // This was to test an interaction, ignore me

    private void Awake()
    {
        _characterController = this.GetComponent<CharacterController>();
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
        moveVector = Vector3.zero;
        moveVector += _camTransform.forward * Input.GetAxisRaw("Vertical");
        moveVector += _camTransform.right * Input.GetAxisRaw("Horizontal");
        moveVector.y = 0; // So we don't move up if we're pointing up, etc
        moveVector.Normalize(); // For if we move in a diagonal

        bool t = _characterController.SimpleMove(moveVector * moveSpeed);

        if (false && Input.GetKeyDown(KeyCode.Space)) // This was to test an interaction, ignore me
        {
            boost = true;
        }

        // Likely check if no input and can add some pseudo drag
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

        if(hit.rigidbody)
        {
            Debug.Log("RIGID");
            hit.rigidbody.AddForce((hit.point - _transform.position).normalized * 5);
        }
        else
        {
            Debug.Log("STATIC");
        }
    }
}
