using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLine : MonoBehaviour
{
    [SerializeField]float endTime = 5f;
    LineRenderer lineRenderer;
    public void SetLine(Vector3 gunPos,Vector3 hitPos)
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0,gunPos);
        lineRenderer.SetPosition(1,hitPos);
        StartCoroutine(DisappearSmoke(gunPos,hitPos));
    }
    private IEnumerator DisappearSmoke(Vector3 gunPos,Vector3 hitPos)
    {
        Vector3 deltaPos = hitPos - gunPos;
        while(Vector3.Distance(hitPos,gunPos)>0.5){
            gunPos += deltaPos.normalized*Time.deltaTime*50; 
            lineRenderer.SetPosition(0,gunPos);
            yield return null;
        }
        Destroy(gameObject);
    }

}
