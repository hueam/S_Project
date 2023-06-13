using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField]
    float speed, gravity= -9.8f, jumpPower;
    float movementY = 0;
    Vector3 inputVec;

    public bool isMyClient;
     
    Transform eye;
    private void Awake() {
        characterController = GetComponent<CharacterController>();
        eye = transform.Find("Visual/Eye");
        ((Client)GameManager.Instance.Managers[Managers.Client]).alwayEvnet += SendTransform;
    }
    private void SendTransform()
    {
        TransformPaket paket = new TransformPaket(transform.position,transform.rotation);
        ((Client)GameManager.Instance.Managers[Managers.Client]).SendData((int)Events.Room,(int)RoomTypes.Move,JsonUtility.ToJson(paket));
    }
    private void FixedUpdate() {
        
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        Vector3 moveVec = (inputVec.x * transform.right + inputVec.z * transform.forward).normalized * speed;

        if(isMyClient == true)
        {
            SetYvelocity(ref moveVec);
        }
        characterController.Move(moveVec * Time.fixedDeltaTime);
    }
    private void SetYvelocity(ref Vector3 moveVec)
    {
        if(CheckGround() == false)
            movementY += gravity* Time.fixedDeltaTime;
        moveVec.y = movementY;
    }
    [SerializeField]
    LayerMask whatIsLayer;
    public bool CheckGround()
    {
        return Physics.Raycast(transform.position,Vector3.down,0.1f,whatIsLayer);;
    }

    public void SetMovementVelocity(Vector3 dir)
    {
        inputVec = dir;
    }
    public void StopImmediately()
    {
        inputVec = Vector3.zero;
    }
    public void PlayerRotate(Vector2 mouseInput)
    {
        eye.localRotation = Quaternion.Euler(-mouseInput.y,0,0);
        transform.rotation = Quaternion.Euler(0,mouseInput.x,0);
    }
    public void Jump()
    {
        movementY = jumpPower;
    }
    private void OnDestroy() {
        ((Client)GameManager.Instance.Managers[Managers.Client]).alwayEvnet -= SendTransform;
    }
}
