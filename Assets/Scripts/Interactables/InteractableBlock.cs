using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBlock : InteractablePickup
{
    private void Update() 
    {
        if (held)
            transform.forward = Camera.main.transform.forward;
    }

    public override void PickUpInteraction()
    {
        base.PickUpInteraction();

        
    }

    public override void DropInteraction()
    {
        base.DropInteraction();

        
    }

    
}
