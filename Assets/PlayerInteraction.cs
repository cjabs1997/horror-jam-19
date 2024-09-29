using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Transform interactableCheck;
    [SerializeField] float checkRadius;
    [SerializeField] float checkDistance;
    [SerializeField] LayerMask interactableLayer;

    Interactable interactionTarget = null;

    void Start()
    {
        
    }

    void Update()
    {
        ControlTargetHighlighting();
    }

    private void ControlTargetHighlighting()
    {
        Interactable newTarget = FindInteractableObject();

        if (interactionTarget == null)
        {
            //we don't currently have a target
            if (newTarget == null)
            {
                //we haven't found an interactable to target yet. do nothing
                return;
            }
            else
            {
                //found our first target
                interactionTarget = newTarget;
                interactionTarget.HighlightInteractable();
            }
        }
        else
        {
            //we have a current target already
            if (newTarget == null)
            {
                //we are no longer in range of previous target
                interactionTarget.ClearHighlight();
            }
            else
            {
                //we are in range of a target
                //is it the one we already have selected?
                if (interactionTarget == newTarget)
                    return;
                else
                {
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
}
