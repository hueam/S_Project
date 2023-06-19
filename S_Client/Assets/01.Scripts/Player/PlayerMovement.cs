using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    AgentAnimator animator;
    AgentController agentController;

    [SerializeField]
    float speed, gravity= -9.8f, jumpPower;
    float movementY = 0;
    Vector3 inputVec;
    bool isJumping;

     
    [SerializeField]
    Transform eye;
    private void Awake() {
        characterController = GetComponent<CharacterController>();
        animator = transform.Find("Visual").GetComponent<AgentAnimator>();
        agentController = GetComponent<AgentController>();
    }
    private void Start() {
        ((Client)GameManager.Instance.Managers[Managers.Client]).alwayEvnet += SendTransform;
    }
    public void SendTransform()
    {
        TransformPaket paket = new TransformPaket(transform.position,eye.rotation);
        ((Client)GameManager.Instance.Managers[Managers.Client]).SendData((int)Events.Room,(int)RoomTypes.Move,JsonUtility.ToJson(paket));
    }
    private void FixedUpdate() {
        if(agentController.IsDead == false)
            CalculateMovement();
        else DieMove();
    }
    public void DieMove()
    {
        
        characterController.Move((Define.MainCam.transform.forward*inputVec.z)*speed*Time.fixedDeltaTime);
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
    void OnApplicationQuit()
    {
        //나갈때 코드 제대로 써라
    }
}
