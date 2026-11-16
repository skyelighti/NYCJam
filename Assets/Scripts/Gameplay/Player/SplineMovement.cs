using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineMovement : MonoBehaviour
{
    [SerializeField] SplineContainer spline;
    [SerializeField] float speed;
    float initSpeed;
    float dist;
    float splineLength;
    float t;
    float3 pos;
    float3 tangent;
    float3 up;
    bool finished = false;
    [SerializeField] Movement player1;
    bool player1Col = false;
    bool player2Col = false;
    [SerializeField] Movement player2;
    Dictionary<ActionMap, float> playerSpace = new() { { ActionMap.D1Gameplay, 4f}, {ActionMap.D2Gameplay, 3f }, { ActionMap.D3Gameplay, 1.6f} };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splineLength = spline.Spline.GetLength();
        finished = false;
        initSpeed = speed; 
        if (player1 != null) {
            player1.playerCollided += OnPlayerCollision;
            player1.playerCollisionFin += OnPlayerCollisionFin;
        }
        if (player2 != null) {
            player2.playerCollided += OnPlayerCollision;
            player2.playerCollisionFin += OnPlayerCollisionFin;
        }
        GameManager.Instance.ActionMapChanged += OnActionMapChanged;
        Events.increaseSpeed += OnIncreaseSpeed;
    }

    public void OnIncreaseSpeed(float amount) {
        UpdateSpeed(speed + amount);
    }
    public void UpdateSpeed(float newSpeed) {
        initSpeed = newSpeed;
        if (speed != 0) {
            speed = newSpeed;
        }
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

    void Pause() {
        speed = 0f;
    }
    void Unpause() {
        speed = initSpeed;
    }

    void OnPlayerCollision(string Ptag) {
        Pause();
        if (Ptag == player1.gameObject.tag)
        {
            player1Col = true;
        }
        if (Ptag == player2.gameObject.tag) {
            player2Col = true;
        }
    }

    void OnPlayerCollisionFin(string Ptag) {
        if (Ptag == player1.gameObject.tag) {
            player1Col = false;
        }
        if (Ptag == player2.gameObject.tag)
        {
            player2Col = false;
        }
        if (!player1Col && !player2Col) {
            Unpause();
        }
    }
    void OnActionMapChanged(ActionMap map)
    {
        if (playerSpace.ContainsKey(map))
        {
            player1.SetBaseX(-playerSpace[map] / 2);
            player2.SetBaseX(playerSpace[map] / 2);
        }
    }
}
