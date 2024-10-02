using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNote : Interactable
{
    [SerializeField] NoteScriptableObject noteSO;

    //will need a reference to a text field to utilize

    protected override void Start()
    {
        base.Start();
    }

    public override void ActivateInteraction()
    {
        base.ActivateInteraction();
        //enable ui elements that contain textmeshpro fields
        //populate those fields with content of noteSO
    }
}
