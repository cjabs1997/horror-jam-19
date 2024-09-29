using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Transform interactableCheck;
    [SerializeField] float checkRadius;
    [SerializeField] float checkDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(interactableCheck.position, checkRadius);
        Gizmos.DrawWireSphere(interactableCheck.position + transform.forward * checkDistance, checkRadius);
    }
}
