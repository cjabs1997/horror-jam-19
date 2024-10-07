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

        StartCoroutine("SetTextFields");

    }

    IEnumerator SetTextFields()
    {
        scriptureField.text = noteSO.GetScriptureText();
        loreField.text = noteSO.GetLoreText();

        scriptureField.enableAutoSizing = true;
        loreField.enableAutoSizing = true; //we set these true, but they won't update fontSize until canvas is enabled

        noteDisplayCanvas.enabled = isActive;
        yield return new WaitForSeconds(0.01f);

        if (scriptureField.fontSize == loreField.fontSize)
        {
            //don't need to change anything
            Debug.Log(scriptureField.fontSize + " is already equal to " + loreField.fontSize);
        }
        else if (scriptureField.fontSize < loreField.fontSize)
        {
            Debug.Log("Top text: " + scriptureField.fontSize + " is smaller than Bottom text: " + loreField.fontSize);
            loreField.enableAutoSizing = false;
            loreField.fontSize = scriptureField.fontSize;
            Debug.Log("Did we set " + loreField.fontSize + " = " + scriptureField.fontSize + "?");
        }
        else
        {
            Debug.Log("Top text: " + scriptureField.fontSize + " is bigger than Bottom text: " + loreField.fontSize);
            scriptureField.enableAutoSizing = false;
            scriptureField.fontSize = loreField.fontSize;
            Debug.Log("Did we set " + scriptureField.fontSize + " = " + loreField.fontSize + "?");
        }

    }
}
