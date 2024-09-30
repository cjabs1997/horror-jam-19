using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected Material highlightMaterial;
    protected Material defaultMaterial;

    protected MeshRenderer _meshRenderer;

    protected virtual void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        if (_meshRenderer != null)
            defaultMaterial = _meshRenderer.materials[0];
    }

    public virtual void ActivateInteraction()
    {
        //this method will be overridden by inheriting interactables, as they will need their own behavior
        Debug.Log("This is the base class activation.");
    }

    public void HighlightInteractable()
    {
        if (_meshRenderer == null)
            return;

        Material[] matArray = _meshRenderer.materials;
        matArray[0] = highlightMaterial;
        _meshRenderer.materials = matArray;
    }

    public void ClearHighlight()
    {
        if (_meshRenderer == null)
            return;

        Material[] matArray = _meshRenderer.materials;
        matArray[0] = defaultMaterial;
        _meshRenderer.materials = matArray;
    }
}
