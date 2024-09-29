using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //CharacterController _characterController;
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
        //_characterController = this.GetComponent<CharacterController>();
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
        moveVector.y=0; // So we don't move up if we're pointing up, etc
        moveVector.Normalize(); // For if we move in a diagonal

        //_characterController.Move(moveVector * Time.deltaTime);

        /* here lies the code killed by cinemachine, RIP
        // Camera stuff, this should be a separate Obj that lerps to this position me thinks
        Debug.Log(Input.GetAxis("Mouse X") + "  " + Input.GetAxis("Mouse Y"));
        xCamAngle += -Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
        yCamAngle += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;

        Vector3 rotation = Vector3.zero;
        rotation.x = xCamAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        rotObj.localRotation = targetRotation;

        rotation = Vector3.zero;
        rotation.y = yCamAngle;
        targetRotation = Quaternion.Euler(rotation);
        pivObj.localRotation = targetRotation;
        */

        if (false && Input.GetKeyDown(KeyCode.Space)) // This was to test an interaction, ignore me
        {
            boost = true;
        }
    }

    private void FixedUpdate()
    {
        //_rigidbody.MovePosition(_transform.position+(moveVector*Time.fixedDeltaTime));
        /*
        Vector3 vDelta = moveAcceleration*moveVector*Time.fixedDeltaTime; // change in our velocity this update
        Vector3 vTarget = vDelta + prevVel; // our target velocity this frame
        vTarget = Vector3.ClampMagnitude(vTarget, moveSpeed); // cap our velocity so we don't go over our max speed
        vTarget -= prevVel; // how much we need to change
        */
        // targetForce = Vector3.ClampMagnitude(targetForce, moveSpeed); // clamp it to our max speed so we don't go over 

        Vector3 dirVector = GetGroundSlope();

        Vector3 vTarget = prevVel + moveAcceleration * dirVector * Time.fixedDeltaTime;
        vTarget = Vector3.ClampMagnitude(vTarget, moveSpeed);
        Vector3 vDelta = vTarget - prevVel;

        if(boost) // This was to test an interaction, ignore me
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
