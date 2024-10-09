using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePickup : Interactable
{
    // If we use the same values for everything there's an argument to hard code OR
    // to use a scriptable object. We can come back to this.
    [Header("Picked Up Traits")]

    [Tooltip("Affects how close the object needs to be to the hold point before it stops getting scaled up. Speed increase is a ratio of this and " +
        "the distance the object is from the hold point. Smaller numbers mean it catches up faster."), Range(0f, 1f)]
    [SerializeField] private float minDistanceScale = 0.025f;

    [Tooltip("Affects how much velocity of the object holding this is applied when the object moves towards the hold point. " +
        "Must be greater than zero. Larger values mean less of the holder's velocity is added.")]
    [SerializeField] private float holderSpeedScale = 3.5f;

    [Tooltip("How far away the object can be from the hold point before being dropped.")]
    [SerializeField] private float breakPoint = 1f;

    [Tooltip("The max speed the object can move towards the hold point. This will override the scaling done from the minDistanceScale and holderSpeedScale. " +
    "Setting to lower values will make the object appear to 'jump' less but possibly slow down the object.")]
    [SerializeField] private float maxSpeed = 7f;



    protected bool _held; // Tracks whether the object is being held
    public Transform holdPoint { get; set; }
    public BuiltInCharacterController controller { get; set; } // Again don't love saving a reference here but it's a jam, sue me
    protected Rigidbody _rigidbody;

    private float defaultDrag;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        defaultDrag = _rigidbody.drag;
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
        _rigidbody.useGravity = false;
        _rigidbody.drag = 0f;

    }

    public void DropInteraction()
    {
        _held = false;
        _rigidbody.useGravity = true;
        _rigidbody.drag = defaultDrag;
        holdPoint = null; // I don't think this is necessary? But might as well
    }

    public void ThrowInteraction(Vector3 throwForce)
    {
        // Currently we are setting a force, not adding a velocity
        // This means that the throw WILL be affected by the mass of the object (takes more force to move a more massive object)
        // If we want it to be universal we can add to the velocity 
        _rigidbody.AddForce(throwForce, ForceMode.Impulse);
    }

    protected void MoveToPoint(Transform point)
    {
        if (holdPoint != null) // Friendly error checking :)
        {
            Vector3 v = point.position - this.transform.position; 
            float distanceToHoldPoint = v.magnitude;

            // If above break point, drop the item
            if (distanceToHoldPoint > breakPoint)
            {
                controller.PlayerInteraction.DropInteractable();
                return;
            }
            
            // Logic basically scales the velocity based on two things:
            //  - the velocity the holder is moving
            //  - how far away the pickup is from its desired location
            v = v * Mathf.Max(1, distanceToHoldPoint / minDistanceScale) + controller.MoveVector / holderSpeedScale;
            // There's an assumption only the player can hold objects, I think that's fine for now

            // Clamp the velocity based on the max speed possible
            v = Vector3.ClampMagnitude(v, maxSpeed);

            _rigidbody.velocity = v;


            // This is the most logical but it jitters and messes with the physics system, we should only do this if we use kinematic rigidbodies
            //Vector3 movePos = Vector3.Lerp(this.transform.position, holdPoint.position, _lerpSpeed * Time.fixedDeltaTime);
            //_rigidbody.MovePosition(movePos);

            //Debug.Log("Moving to: " + holdPoint.position);
        }
    }
}
