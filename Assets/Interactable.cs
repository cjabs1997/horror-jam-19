using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected Material highlightMaterial;
    protected Material defaultMaterial;

    protected MeshRenderer _meshRenderer;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        if (_meshRenderer != null)
            defaultMaterial = _meshRenderer.material;
    }

    public virtual void ActivateInteraction()
    {
        //this method will be overridden by inheriting interactables, as they will need their own behavior
    }

    public void HighlightInteractable()
    {
        if (_meshRenderer == null)
            return;

        _meshRenderer.material = highlightMaterial;
    }

    public void ClearHighlight()
    {
        if (_meshRenderer == null)
            return;

        _meshRenderer.material = defaultMaterial;
    }
}
