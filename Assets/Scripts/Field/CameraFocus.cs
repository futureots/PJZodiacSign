using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    // 카메라 위치
    Transform camTransform => transform;
    // 중심 대상위치
    public Transform targetTransform;
    /// <summary>
    /// 카메라 회전 속도
    /// </summary>
    public float rotateSpeed;
    public float zoomSpeed;
    private void Start()
    {

    }
    [ContextMenu("OneUpdate")]
    private void Update()
    {
        camTransform.LookAt(targetTransform);
        // 마우스 우클릭으로 카메라 회전
        if (Input.GetMouseButton(1))
        {
            SetCameraHorizontal();
            SetCameraVertical();
            SetCameraZoom();
        }
        
    }

    void SetCameraHorizontal()
    {
        var mouseX = Input.GetAxis("Mouse X");
        //Debug.Log(mouseX);
        var rot = targetTransform.rotation.eulerAngles;
        rot.y += mouseX * rotateSpeed * 100 * Time.deltaTime;
        targetTransform.rotation = Quaternion.Euler(rot);
    }
    void SetCameraVertical()
    {
        var mouseY = Input.GetAxis("Mouse Y");
        var rot = targetTransform.rotation.eulerAngles;
        rot.x -= mouseY * rotateSpeed * 100 * Time.deltaTime;
        rot.x = Mathf.Clamp(rot.x, 10, 80);
        //Debug.Log(rot.x);
        targetTransform.rotation = Quaternion.Euler(rot);
    }
    void SetCameraZoom()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        //Debug.Log(scroll);
        camTransform.position -= camTransform.position.normalized * scroll * zoomSpeed * Time.deltaTime * 1000;
    }
}
