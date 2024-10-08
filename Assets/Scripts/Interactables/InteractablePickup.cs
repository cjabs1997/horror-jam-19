using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePickup : Interactable
{
    [SerializeField] protected float _lerpSpeed;
    private Vector3 velocity = Vector3.zero;

    protected bool _held;
    public Transform holdPoint { get; set; }
    public BuiltInCharacterController controller; // GROSS but let me cook
    protected Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
        _held = false;
    }

    void FixedUpdate() // I think we want to move this with physics? It makes the most sense.
    {
        if (_held)
        {
            MoveToPoint(holdPoint);
        }
    }

    public override void ActivateInteraction()
    {
        base.ActivateInteraction();

        _held = true;
        _rigidbody.useGravity = false; // Would we rather make it kinematic? Maybe? We'll see

    }

    public void DropInteraction()
    {
        _held = false;
        _rigidbody.useGravity = true;
        holdPoint = null; // I don't think this is necessary? But might as well
    }

    protected void MoveToPoint(Transform point)
    {
        if (holdPoint != null) // Friendly error checking :)
        {
            Vector3 v = point.position - this.transform.position;
            float a = 0.025f; 
            float b = v.magnitude;
            float breakPoint = 2f;
            float maxSpeed = 5f;

            if(b > breakPoint)
            {
                // Above break point, drop the item
                controller.PlayerInteraction.DropInteractable();
                return;
            }
            
            // Logic basically scales the velocity based on two things:
            //  - the direction the player is moving
            //  - how far away the pickup is from its desired location
            v = v * Mathf.Max(1, b/a) + controller.MoveVector/3.5f; // These values need tweaking, can also consider adding a break point
                                                                    // Maybe no friction as well?

            v = Vector3.ClampMagnitude(v, maxSpeed);

            _rigidbody.velocity = v;

            // This is the most logical but it jitters and messes with the physics system, we should only do this if we use kinematic rigidbodies
            //Vector3 movePos = Vector3.Lerp(this.transform.position, holdPoint.position, _lerpSpeed * Time.fixedDeltaTime);
            //_rigidbody.MovePosition(movePos);

            //Debug.Log("Moving to: " + holdPoint.position);
        }
    }
}
