using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableDoorButton : Interactable
{
    //Lot of variables necessary because I'm moving primitives instead
    //of using an object with an animation.
    [SerializeField] Transform movingDoor;
    [SerializeField] Vector3 doorOpenOffset;
    [SerializeField] float moveSpeed;
    [SerializeField] float openDuration = 4f;

    Vector3 doorClosedPosition;
    Vector3 doorOpenPosition;

    Vector3 currentTargetPosition;
    bool isMoving = false;

    protected override void Start()
    {
        base.Start();
        doorClosedPosition = movingDoor.position;
        doorOpenPosition = doorClosedPosition + doorOpenOffset;
    }

    protected void Update()
    {
        if (isMoving)
        {
            var step = moveSpeed * Time.deltaTime;
            movingDoor.position = Vector3.MoveTowards(movingDoor.position, currentTargetPosition, step);

            if (movingDoor.position == currentTargetPosition)
                isMoving = false;
        }
    }

    public override void ActivateInteraction()
    {
        base.ActivateInteraction();
        StartCoroutine("OpenDoor");
    }

    IEnumerator OpenDoor()
    {
       SetTargetPosition(doorOpenPosition);
       yield return new WaitForSeconds(openDuration);
       SetTargetPosition(doorClosedPosition);
    }

    void SetTargetPosition(Vector3 targetPosition)
    {
        currentTargetPosition = targetPosition;
        isMoving = true;
    }
}
