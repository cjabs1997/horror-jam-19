using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableNote : Interactable
{
    [SerializeField] NoteScriptableObject noteSO;
    [SerializeField] Canvas noteDisplayCanvas;
    [SerializeField] TextMeshProUGUI scriptureField;
    [SerializeField] TextMeshProUGUI loreField;

    //Temp bool to test opening and then closing
    //will need to workshop ideas for closing. Maybe player just activates again,
    //or when they walk out of a range, it closes. idk.
    bool isActive = false;

    protected override void Start()
    {
        base.Start();
    }

    public override void ActivateInteraction()
    {
        base.ActivateInteraction();
        //enable ui elements that contain textmeshpro fields
        //populate those fields with content of noteSO
        isActive = !isActive;

        scriptureField.text = noteSO.GetScriptureText();
        loreField.text = noteSO.GetLoreText();

        noteDisplayCanvas.enabled = isActive;
    }
}
