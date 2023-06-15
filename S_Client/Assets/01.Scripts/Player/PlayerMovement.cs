using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    AgentAnimator animator;
    [SerializeField]
    float speed, gravity= -9.8f, jumpPower;
    float movementY = 0;
    Vector3 inputVec;
    bool isJumping;

    public bool isMyClient;
     
    [SerializeField]
    Transform eye;
    private void Awake() {
        characterController = GetComponent<CharacterController>();
        animator = transform.Find("Visual").GetComponent<AgentAnimator>();
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
        animator?.SetFloatSpeed(inputVec.sqrMagnitude);
        if(inputVec.sqrMagnitude>0)
        {
            animator?.SetFloatInputX(inputVec.x);
            animator?.SetFloatInputY(inputVec.z);
        }
        Vector3 moveVec = (inputVec.x * transform.right + inputVec.z * transform.forward).normalized * speed;
        

        SetYvelocity(ref moveVec);

        characterController.Move(moveVec * Time.fixedDeltaTime);
    }
    private void SetYvelocity(ref Vector3 moveVec)
    {
        if(CheckGround(out RaycastHit hit) == false){
            movementY += gravity* Time.fixedDeltaTime;
        }
        else if(isJumping == false) movementY = 0;
        else isJumping = false;
        moveVec.y = movementY;
    }
    [SerializeField]
    LayerMask whatIsLayer;
    public bool CheckGround(out RaycastHit hit)
    {
        Vector3 p1 = transform.position + characterController.center + Vector3.up * -characterController.height * 0.25F;
        Vector3 p2 = p1 + Vector3.up * characterController.height+ Vector3.up * -characterController.height * 0.25F;
        return Physics.CapsuleCast(p1,p2,characterController.radius,Vector3.down,out hit,0.1f,whatIsLayer);
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
        if(CheckGround(out RaycastHit hit))
        {
            isJumping = true;
            movementY = jumpPower;
        }
    }
    private void OnDestroy() {
        ((Client)GameManager.Instance.Managers[Managers.Client]).alwayEvnet -= SendTransform;
    }
}
