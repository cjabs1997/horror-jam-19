using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Transform interactableCheck;
    [SerializeField] float checkRadius;
    [SerializeField] float checkDistance;
    [SerializeField] LayerMask interactableLayer;

    [Tooltip("The point which held objects will try to follow. This should be a Child of the Main Camera Game Object and needs to be dragged in (for now).")]
    [SerializeField] Transform _holdPoint;

    public Interactable interactionTarget { private set; get; }  = null;
    public InteractablePickup heldInteractable { private set; get; } = null; // The specific instance of held objects, allows for more functionality
    public BuiltInCharacterController controller { get; private set; }

    private void Awake()
    {
        controller = this.GetComponent<BuiltInCharacterController>(); // GROSS
    }

    void Start()
    {
        
    }

    void Update()
    {
        ControlTargetHighlighting();

        //input here only for testing.
        //In the future, the player controller script can call this public method
        //if (Input.GetMouseButtonDown(0))
        //    ActivateCurrentInteractionTarget();

    }

    public void ActivateCurrentInteractionTarget()
    {
        if (interactionTarget == null)
            return;

        interactionTarget.ActivateInteraction();

        CheckForPickUp(); // Maybe it makes more sense to include this as part of activating
                          // and it sort of works it's way back? With how things are right now
                          // this works. We can refactor later if we want.
    }

    private void ControlTargetHighlighting()
    {
        Interactable newTarget = FindInteractableObject();

        if (interactionTarget == null)
        {
            //we don't currently have a target
            if (newTarget == null)
            {
                //Debug.Log("Haven't seen a target yet. Do nothing.");
                return;
            }
            else
            {
                //Debug.Log("Found our first target. We should highlight it.");
                interactionTarget = newTarget;
                interactionTarget.HighlightInteractable();
            }
        }
        else
        {
            //we have a current target already
            if (newTarget == null)
            {
                //Debug.Log("We lost our previous target. No current target.");
                interactionTarget.ClearHighlight();
                interactionTarget = null;
            }
            else
            {
                //we are in range of a target
                //is it the one we already have selected?
                if (interactionTarget == newTarget)
                {
                    //Debug.Log("Still looking at the same target. Change nothing.");
                    return;
                }
                else
                {
                    //Debug.Log("Found a better target. Switching.");
                    interactionTarget.ClearHighlight();
                    interactionTarget = newTarget;
                    interactionTarget.HighlightInteractable();
                }
            }
        }
    }

    Interactable FindInteractableObject()
    {
        Collider[] potentialTargets = Physics.OverlapCapsule(interactableCheck.position, 
                                        interactableCheck.position + Camera.main.transform.forward * checkDistance, 
                                        checkRadius, interactableLayer);

        Collider closestTarget = null;
        float distanceToTarget = checkDistance * 10; //should be further than any detected item would ever be
        
        if (potentialTargets.Length > 0) //at least 1 object in range
        {
            foreach (Collider target in potentialTargets) //we only want one to be selected, find closest one
            {
                if (IsInLineOfSight(target))
                {
                    if (Vector3.Distance(interactableCheck.position, target.ClosestPoint(interactableCheck.position)) < distanceToTarget)
                    {
                        //this object is closer than any previously checked object
                        closestTarget = target;
                    }
                }
            }
        }


        if (closestTarget == null)
        {
            //this should only happen if no objects were in range, or they were obstructed
            return null;
        }
        else
        {
            Debug.Log("Found the object.");
            return closestTarget.gameObject.GetComponent<Interactable>();
        }
    }

    bool IsInLineOfSight(Collider target)
    {
        //linecast to target object. If cast hits something other than target, return false
        //It's possible a corner of the object is visible yet this returns false, which is
        //probably fine. Situation likely wouldn't arise too often, and it's not too much to 
        //ask a player to be able to properly see the majority of an object they're interacting with      
        RaycastHit hit;
        Physics.Linecast(interactableCheck.position, target.transform.position, out hit);

        if (hit.collider == target)
            return true;
        else
            return false;
    }

    void OnDrawGizmos()
    {
        //there is no drawCapsule option. These two spheres represent the Physics.OverlapCapsule that will
        //check for interactables in front of the player
        Gizmos.DrawWireSphere(interactableCheck.position, checkRadius);
        Gizmos.DrawWireSphere(interactableCheck.position + Camera.main.transform.forward * checkDistance, checkRadius);
    }
    #region PickUp Stuff

    private void CheckForPickUp()
    {
        // This might be gormless? It lets us determine if it's something we want to pickup
        // and allows us to get a reference to it's more specific class so we get more 
        // control. I'm gonna leave it like this for now.
        // Asks are we holding nothing? AND Is this something we can pick up?
        if (heldInteractable == null && interactionTarget.TryGetComponent<InteractablePickup>(out InteractablePickup pickup))
        {
            Debug.Log("Picked up: " + pickup.gameObject.name);
            pickup.holdPoint = _holdPoint; // Set hold point
            heldInteractable = pickup; // Track the object we are holding
            heldInteractable.ClearHighlight(); // Stop highlighting it (idk if this was needed but we're running with it)
            heldInteractable.controller = controller; // I don't love passing a reference down like this but it works, sue me
        }
    }

    public void DropInteractable()
    {
        if (heldInteractable != null) // Friendly error checking :)
        {
            heldInteractable.DropInteraction();
            heldInteractable = null;
        }
    }

    public void ThrowInteractable(Vector3 throwForce)
    {
        if (heldInteractable != null) // Friendly error checking :)
        {
            heldInteractable.DropInteraction(); // Drop the object first
            heldInteractable.ThrowInteraction(throwForce);
            heldInteractable = null;
        }
    }

    #endregion
}
