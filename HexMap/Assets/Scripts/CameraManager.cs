using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public float panSpeed = 2f;
    public float zoomSpeed = 2f;
    public float zoomInMax = 40f;
    public float zoomOutMax = 80f;
    
    private CinemachineInputProvider _inputProvider;
    private CinemachineVirtualCamera _virtualCamera;
    private Transform _cameraTransform;
    
    private void Awake()
    {
        _inputProvider = GetComponent<CinemachineInputProvider>();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cameraTransform = _virtualCamera.VirtualCameraGameObject.transform;
    }

    private void Update()
    {
        float x = _inputProvider.GetAxisValue(0);
        float y = _inputProvider.GetAxisValue(1);
        float z = _inputProvider.GetAxisValue(2);
        if (x != 0 || y != 0)
        {
            PanScreen(x, y);
        }

        if (z != 0)
        {
            ZoomScreen(z);
        }
    }

    private Vector2 GetPanDirection(float x, float y)
    {
        Vector2 direction = Vector2.zero;
        if (y >= Screen.height * 0.95f)
        {
            direction.y += 1;
        }
        else if (y <= Screen.height * 0.05f)
        {
            direction.y -= 1;
        }

        if (x >= Screen.width * 0.95f)
        {
            direction.x += 1;
        }
        else if (x <= Screen.width * 0.05f)
        {
            direction.x -= 1;
        }

        return direction;
    }

    private void PanScreen(float x, float y)
    {
        Vector2 direction = GetPanDirection(x, y);
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, 
            _cameraTransform.position + new Vector3(direction.x, 0, direction.y), Time.deltaTime * panSpeed);
    }
    private void ZoomScreen(float increment)
    {
        float fov = _virtualCamera.m_Lens.FieldOfView;
        float target = Mathf.Clamp(fov + increment, zoomInMax, zoomOutMax);
        _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fov, target, Time.deltaTime * zoomSpeed);
    }
    
}
