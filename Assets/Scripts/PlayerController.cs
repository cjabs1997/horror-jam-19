using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController _characterController;
    Transform _transform;
    Transform _camTransform;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float hoverSpeed;

    private void Awake()
    {
        _characterController = this.GetComponent<CharacterController>();
        _transform = this.transform;
        _camTransform = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Moving stuff
        Vector3 moveVector = Vector3.zero;
        moveVector += _camTransform.forward * Input.GetAxisRaw("Vertical");
        moveVector += _camTransform.right * Input.GetAxisRaw("Horizontal");
        moveVector.Scale(new Vector3(1,0,1)); // So we don't move up if we're pointing up, etc
        moveVector.Normalize(); // For if we move in a diagonal
        moveVector *= moveSpeed;
        moveVector.y = hoverSpeed * Input.GetAxisRaw("Hover");

        _characterController.Move(moveVector * Time.deltaTime);

        /* here lies the code killed by cinemachine, RIP
        // Camera stuff, this should be a separate Obj that lerps to this position me thinks
        Debug.Log(Input.GetAxis("Mouse X") + "  " + Input.GetAxis("Mouse Y"));
        xCamAngle += -Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
        yCamAngle += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;

        Vector3 rotation = Vector3.zero;
        rotation.x = xCamAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        rotObj.localRotation = targetRotation;

        rotation = Vector3.zero;
        rotation.y = yCamAngle;
        targetRotation = Quaternion.Euler(rotation);
        pivObj.localRotation = targetRotation;
        */
    }
}
