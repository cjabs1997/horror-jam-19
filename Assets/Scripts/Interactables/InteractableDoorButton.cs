using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableDoorButton : Interactable
{
    //Lot of variables necessary because I'm moving primitives instead
    //of using an object with an animation.
    [SerializeField] GameObject movingDoor;
    [SerializeField] float openDuration = 4f;

    Animator doorAnimator;

    protected override void Start()
    {
        base.Start();
        doorAnimator = movingDoor.GetComponent<Animator>();
    }

    public override void ActivateInteraction()
    {
        base.ActivateInteraction();
        StartCoroutine("OpenDoor");
    }

    IEnumerator OpenDoor()
    {
       doorAnimator.SetBool("Open", true);
       yield return new WaitForSeconds(openDuration);
       doorAnimator.SetBool("Open", false);
    }
}
