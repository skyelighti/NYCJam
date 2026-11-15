using UnityEngine;
using UnityEngine.Splines;

[ExecuteAlways]
public class TrackSpawning : MonoBehaviour
{
    [SerializeField] SplineContainer spline;
    [SerializeField] float distOnTrack;
    [SerializeField] float horiDist = 0f;
    [SerializeField] float height = 0f;

    void Update()
    {
        if (Application.isPlaying || spline == null) return;

        float t = Mathf.Clamp01(distOnTrack / spline.Spline.GetLength());

        spline.Evaluate(t, out var pos, out var tan, out var up);
        Vector3 fwd = ((Vector3)tan).normalized;
        Vector3 right = Vector3.Cross((Vector3)up, fwd).normalized;
        transform.position = (Vector3)pos + right * horiDist + (Vector3)up * height;

    }
}