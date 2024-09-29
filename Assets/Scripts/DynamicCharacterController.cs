using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCharacterController : MonoBehaviour
{
    Rigidbody _rigidbody;
    Transform _transform;
    Transform _camTransform;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveAcceleration;

    Vector3 moveVector;
    Vector3 prevVel;
    bool boost; // This was to test an interaction, ignore me

    RaycastHit hit;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _transform = this.transform;
        _camTransform = Camera.main.transform;
        prevVel = Vector3.zero;
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


        if (false && Input.GetKeyDown(KeyCode.Space)) // This was to test an interaction, ignore me
        {
            boost = true;
        }
    }

    private void FixedUpdate()
    {
        Vector3 dirVector = GetGroundSlope();

        Vector3 vTarget = prevVel + moveAcceleration * dirVector * Time.fixedDeltaTime;
        vTarget = Vector3.ClampMagnitude(vTarget, moveSpeed);
        Vector3 vDelta = vTarget - prevVel;

        if (boost) // This was to test an interaction, ignore me
        {
            boost = false;
            vDelta += moveVector * 100f;
        }
        _rigidbody.velocity += vDelta;
        prevVel = _rigidbody.velocity;

        //Debug.Log(prevVel + "  " + _rigidbody.velocity);
        //Debug.Log(vDelta);
    }

    private Vector3 GetGroundSlope()
    {
        Vector3 dirVector = moveVector; // Let's me play with stuff, og was just moveVector

        if (Physics.Raycast(_transform.position, Vector3.down, out hit, 2f)) // If we're on a surface, get the normal to the plane
        {
            dirVector = Vector3.ProjectOnPlane(moveVector, hit.normal);
            Debug.DrawLine(hit.point, hit.point + dirVector, Color.red); // Draw the vector of the slope
            // Can add slope checking logic here if we need
        }

        return dirVector;
    }
}
