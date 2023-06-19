using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Events;

public class OtherPlayer : MonoBehaviour
{
    Vector3 beforePos = Vector3.zero;
    
    Quaternion deltaQua = Quaternion.identity;
    Quaternion beforeQua = Quaternion.identity;

    AgentAnimator agentAnimator;

    private float maxHP = 50f;
    public float currentHP;

    public string socketID;

    float lastTime;

    private bool IsDead = false; 

    [SerializeField]
    Transform pivot;
    [SerializeField]
    Transform visual;
    [SerializeField]
    CharacterController characterController;
    [SerializeField]
    Transform firePos;
    [SerializeField]
    BulletLine bulletLine;

    [SerializeField]
    public UnityEvent FireEvent; 

    private float acceleration = 0;
    private float beforeTime;
    private void Awake()
    {
        IsDead = true;
        visual = transform.Find("Visual");
        agentAnimator = visual.GetComponent<AgentAnimator>();
        currentHP = maxHP;
    }
    private void Start() {
        beforePos = transform.position;
        beforeQua = transform.rotation;
        beforeTime = Time.time;
    }
    private void Update() {
        if(IsDead == false)return;
        transform.position = Vector3.Lerp(transform.position,beforePos,0.5f);            
        transform.rotation = Quaternion.Slerp(transform.rotation,beforeQua,0.5f);            
        agentAnimator?.SetFloatSpeed(Mathf.Abs(acceleration));
    }
    public void SetVelocity(Vector3 Vec,Quaternion quaternion)
    {
        float deltaTime = Time.time - beforeTime;
        acceleration = (Vec.magnitude - beforePos.magnitude)/deltaTime;
        beforePos = Vec;
        beforeQua = quaternion;
        beforeTime = Time.time;
    }
    public void SetDie()
    {
        IsDead = false;
        characterController.enabled = false;
        visual.gameObject.SetActive(false);
    }
    public void ReSapwn(Vector3 pos, Quaternion rot)
    {
        currentHP = maxHP;
        transform.position = pos;
        transform.rotation = rot;
        IsDead = true;
        characterController.enabled = true;
        visual.gameObject.SetActive(true);
        beforePos = transform.position;
        beforeQua = transform.rotation;
    }
    public bool HitDamage(int damage)
    {
        if(IsDead == false) return false;
        currentHP -= damage;
        ((Client)GameManager.Instance.Managers[Managers.Client]).SendData((int)Events.InGame, (int)InGameTypes.Hit, JsonUtility.ToJson(new DamagePacket(socketID, damage)));
        return currentHP <= 0;            
    }
    public void Fire(Vector3 endPos)
    {
        FireEvent?.Invoke();
        BulletLine line = Instantiate(bulletLine.gameObject,Vector3.zero,Quaternion.identity).GetComponent<BulletLine>();
        line.SetLine(firePos.position,endPos);

    }
        
}
