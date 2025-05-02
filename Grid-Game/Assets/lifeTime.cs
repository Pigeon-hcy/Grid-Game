using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class lifeTime : MonoBehaviour
{
   
    [SerializeField]
    float life;
    float time;
    [SerializeField]
    AnimationCurve lifeCurve;
    [SerializeField]
    AnimationCurve AnimationCurve;

    private void Start()
    {
        life = 1f;

    }

    private void Update()
    {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time/life);
        float curveT = lifeCurve.Evaluate(t);
        transform.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(0, 0, 0), curveT);

        if (time > life)
        { 
            Destroy(this.gameObject);
        }
    }

}
