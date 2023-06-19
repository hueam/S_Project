using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashFeedback : Feedback
{
    
    public List<Light> lights = new List<Light>();
    public override void CreateFeedback()
    {
        StartCoroutine(DelayCor());
    }
    IEnumerator DelayCor()
    {
        lights.ForEach(lights=> lights.enabled = true);
        yield return new WaitForSeconds(0.1f);
        lights.ForEach(lights=> lights.enabled = false);
    }

    public override void FinishFeedback()
    {
        StopAllCoroutines();
        lights.ForEach(lights=> lights.enabled = false);
    }
}
