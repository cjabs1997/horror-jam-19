using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableNote : Interactable
{
    [SerializeField] float deactivationRange = 4f; //if player is further than this, active note will close
    [SerializeField] NoteScriptableObject noteSO;
    [SerializeField] Canvas noteDisplayCanvas;
    [SerializeField] TextMeshProUGUI scriptureField;
    [SerializeField] TextMeshProUGUI loreField;


    bool isActive = false;

    protected override void Start()
    {
        base.Start();
    }

    void Update() 
    {
        if (!isActive)
            return;
        
        if (Vector3.Distance(transform.position, Camera.main.transform.position) > deactivationRange)
            ActivateInteraction();
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
        }
        else if (scriptureField.fontSize < loreField.fontSize)
        {
            loreField.enableAutoSizing = false;
            loreField.fontSize = scriptureField.fontSize;
        }
        else
        {
            scriptureField.enableAutoSizing = false;
            scriptureField.fontSize = loreField.fontSize;
        }

    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position, deactivationRange);
    }
}
