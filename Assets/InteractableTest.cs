using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTest : Interactable
{
    Rigidbody _rb;

    protected override void Start() 
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
    }

    public override void ActivateInteraction()
    {
        base.ActivateInteraction();
        Debug.Log("This is the derived class activation");
        //for this test object, I'll just throw the object in the air when activated
        _rb.AddForce(Vector3.up * 200);
    }
}
