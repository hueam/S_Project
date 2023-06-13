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



    private void Start() {
        beforePos = transform.position;
        beforeQua = transform.rotation;
        offSetVec = transform.position;
    }
    private void Update()
    {
        offSetVec += (deltaPos*Time.deltaTime)*5;
        transform.position = offSetVec;
        Quaternion q=transform.rotation;
        q.eulerAngles += deltaQua.eulerAngles*Time.deltaTime;
        transform.rotation = q;
        
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
