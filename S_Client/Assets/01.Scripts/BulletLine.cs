using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BulletLine : MonoBehaviour
{
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
        Vector3 dir = hitPos - gunPos;
        while(Vector3.Distance(gunPos,hitPos) > 0.1f&&(hitPos-gunPos).normalized == dir.normalized)
        {

            gunPos += dir.normalized * Time.deltaTime * 50f;
            lineRenderer.SetPosition(0,gunPos);
            yield return null;
        }
        Destroy(gameObject);
    }

}
