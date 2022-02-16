using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private string mouseXInputName, mouseYInputName;
    [SerializeField] private float mouseSensitivity;
    
    [SerializeField] private Transform playerBody;
    private float xAxisClamp;
    
    private void Awake() {
        SetCursorLocked(true);
        xAxisClamp = 0;
    }
    
    private void SetCursorLocked(bool locked)
    {
        if(locked) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.None;
    }
    
    private void Update() {
        CameraRotation();
    }
    
    private void CameraRotation()
    {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;
        
        xAxisClamp += mouseY;
        
        if(xAxisClamp > 90f)
        {
            xAxisClamp = 90;
            mouseY = 0;
            ClampXAxisRotationToValue(270f);
        }
        else if(xAxisClamp<-90)
        {
            xAxisClamp = -90f;
            mouseY = 0;
            ClampXAxisRotationToValue(90f);
        }
        
        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }
    
    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
    
}
