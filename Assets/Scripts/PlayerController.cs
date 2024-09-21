using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController _characterController;
    Transform _transform;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotSpeed;

    private float xCamAngle;
    private float yCamAngle;

    private void Awake()
    {
        _characterController = this.GetComponent<CharacterController>();
        _transform = this.transform;
        xCamAngle = 0;
        yCamAngle = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVector = Vector3.zero;
        moveVector += _transform.forward * Input.GetAxisRaw("Vertical");
        moveVector += _transform.right * Input.GetAxisRaw("Horizontal");
        moveVector.Normalize(); // For if we move in a diagonal

        Debug.Log(Input.GetAxis("Mouse X") + "  " + Input.GetAxis("Mouse Y"));

        _characterController.Move(moveVector*moveSpeed*Time.deltaTime);
        


    }
}
