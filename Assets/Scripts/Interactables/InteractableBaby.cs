using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBaby : InteractablePickup
{
    [SerializeField] Transform meshTransform; // I think we'll just want to rotate the mesh, not the rigidbody
    [SerializeField] float timeToRotate;

    private Coroutine _rotateRoutine;

    public override void PickUpInteraction()
    {
        base.PickUpInteraction();

        _rigidbody.freezeRotation = true;
        _rotateRoutine = StartCoroutine(RotateToPlayer());
    }

    public override void DropInteraction()
    {
        base.DropInteraction();

        StopCoroutine(_rotateRoutine);
        _rigidbody.freezeRotation = false;
    }

    IEnumerator RotateToPlayer()
    {
        float elapsedTime = 0f;
        Quaternion startingRot = meshTransform.rotation;
        Vector3 lookDirection = controller.transform.position - holdPoint.position;
        Quaternion targetRot = Quaternion.LookRotation(lookDirection);

        while(meshTransform.rotation != targetRot)
        {
            // There might be an argument to go from current rot vs starting but I think this makes more sense?
            meshTransform.rotation = Quaternion.Lerp(startingRot, targetRot, elapsedTime/timeToRotate);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}