using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class QuickSceneSwitcher : MonoBehaviour // This is gonna be my dirty script for stuff just for quick tests/builds
{
    [SerializeField] CinemachineVirtualCamera _cam;
    [SerializeField] float camSpeed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        /*
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }*/

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Bing");
            if (Input.GetKey(KeyCode.LeftShift))
                SetCamSpeed(-10);
            else
                SetCamSpeed(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                SetCamSpeed(10);
            else
                SetCamSpeed(1);
        }


    }

    private void SetCamSpeed(float s)
    {
        camSpeed = Mathf.Max(camSpeed + s, 0);
        _cam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = camSpeed;
        _cam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = camSpeed;

        Debug.Log("BONG: " + camSpeed);
    }
}
