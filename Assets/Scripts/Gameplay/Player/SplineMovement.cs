using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplineMovement : MonoBehaviour
{
    [SerializeField] SplineContainer spline;
    [SerializeField] float speed;
    float dist;
    float splineLength;
    float t;
    float3 pos;
    float3 tangent;
    float3 up;
    bool finished = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineLength = spline.Spline.GetLength();
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        dist = speed * Time.deltaTime;
        t += dist / splineLength;
        spline.Evaluate(t, out pos, out tangent, out up);
        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(((Vector3)tangent).normalized, up);
        if (t >= 1f && !finished)
        {
            finished = true;
        }
    }
}
