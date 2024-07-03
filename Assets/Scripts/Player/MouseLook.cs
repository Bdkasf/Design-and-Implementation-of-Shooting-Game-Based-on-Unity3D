using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 摄像机的旋转
/// 玩家左右旋转控制实现左右移动
/// 摄像机上下旋转控制视线上下移动
/// </summary>
public class MouseLook : MonoBehaviour
{
    [Tooltip("视野灵敏度")]public float mouseSentivity = 400f;
    private Transform playerBody;//玩家位置
    private float yRotation = 0f;//摄像机上下旋转的数值

    private CharacterController characterController;
    [Tooltip("当前摄像机高度")]public float height = 1.8f;
    private float interpolationSpeed = 12f;//高度变化的平滑值

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//鼠标锁定
        playerBody = transform.GetComponentInParent<PlayerController>().transform;//获取玩家当前的位置
        characterController = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSentivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSentivity * Time.deltaTime;

        yRotation -= mouseY;//将上下旋转的轴值进行累计
        yRotation = Mathf.Clamp(yRotation, -60f, 60f);//摄像机上下旋转的极值角度
        transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);//摄像机上下旋转
        playerBody.Rotate(Vector3.up * mouseX); //玩家左右移动

        /* 当人物下蹲或者站立的时候，随着高度变化，摄像机的高度也要发生变化 */
        float heightTarget = characterController.height * 0.9f;
        height = Mathf.Lerp(height, heightTarget, interpolationSpeed * Time.deltaTime);
        //设置下蹲站立时候的摄像机高度
        transform.localPosition = Vector3.up * height;
    }
}
