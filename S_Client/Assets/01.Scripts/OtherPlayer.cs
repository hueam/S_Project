using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    Vector3 deltaPos = Vector3.zero;
    Vector3 beforePos = Vector3.zero;
    Vector3 offSetVec = Vector3.zero;
    
    Quaternion deltaQua = Quaternion.identity;
    Quaternion beforeQua = Quaternion.identity;

    AgentAnimator agentAnimator;

    public string socketID;

    [SerializeField]
    Transform pivot;
    private PlayerHealth playerHealth;
    public PlayerHealth PlayerHealthCompo => playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        pivot = transform.Find("pivot");
        playerHealth.Init();
    }
    private void Start() {
        beforePos = transform.position;
        beforeQua = transform.rotation;
        offSetVec = transform.position;
    }
    private void Update()
    {
        agentAnimator.SetFloatInputX((beforeQua * deltaPos).x);
        agentAnimator.SetFloatInputY((beforeQua * deltaPos).z);
        
        offSetVec += (deltaPos*Time.deltaTime)*5;
        agentAnimator.SetFloatSpeed(offSetVec.normalized.sqrMagnitude);
        transform.position = offSetVec;
        Vector3 q=transform.rotation.eulerAngles;
        q.y += deltaQua.eulerAngles.y*Time.deltaTime;
        transform.rotation = Quaternion.Euler(q);


        q=pivot.localRotation.eulerAngles;
        q.x += deltaQua.eulerAngles.x*Time.deltaTime;
        pivot.localRotation = Quaternion.Euler(q);
    }
    public void SetVelocity(Vector3 Vec,Quaternion quaternion)
    {
        deltaPos = Vec-transform.position;
        deltaQua.eulerAngles = beforeQua.eulerAngles - quaternion.eulerAngles;
        transform.position = Vec;
        beforePos = Vec;
        transform.rotation = quaternion;
        beforeQua = quaternion;
    }
        
}
