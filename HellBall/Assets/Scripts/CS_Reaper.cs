using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Reaper : MonoBehaviour {

    private GameObject player;
    private BoxCollider2D mapBoundary;

    public CircleCollider2D wanderCircle;
    public CircleCollider2D searchRange;

    private float directionDegree;
    private Vector3 target;
    private const float jitter = 10.0f;
    private const float radius = 10.0f;
    private const float velocity = 3.0f;

    private bool isActived;

    private void Start()
    {
        CS_Managers.Instance.gameManager.ED_StartGame += StartGame;
        CS_Managers.Instance.gameManager.ED_ResetGame += ResetGame;

        player = GameObject.Find("Player");
        mapBoundary = GameObject.Find("ReaperBoundary").GetComponent<BoxCollider2D>();
        ResetGame();
    }

    private void Update()
    {
        if(isActived)
        {
            // 매 프레임당 0.6% 확률로 방향을 플레이어 쪽으로 갱신.
            if (Random.Range(0.0f, 1.0f) < 0.006f)
            {
                float dot = Vector2.Dot((player.transform.position - transform.position).normalized, Vector2.right);
                directionDegree = Mathf.Acos(dot) * Mathf.Rad2Deg;
                if (player.transform.position.y < transform.position.y) directionDegree *= -1.0f;

            }

            directionDegree += Random.Range(-jitter, jitter);

            UpdatePosition(directionDegree);
        }
    }

    private void StartGame()
    {
        isActived = true;
    }

    private void ResetGame()
    {
        transform.position = 
            new Vector2(
                Random.Range(mapBoundary.bounds.min.x, mapBoundary.bounds.max.x),
                Random.Range(mapBoundary.bounds.min.y, mapBoundary.bounds.max.y));
        directionDegree = Random.Range(0.0f, 360.0f);
        UpdatePosition(directionDegree);
        isActived = false;
    }

    private void UpdatePosition(float dirDeg)
    {
        target = new Vector2(Mathf.Cos(directionDegree * Mathf.Deg2Rad), Mathf.Sin(directionDegree * Mathf.Deg2Rad));
        target.Normalize();
        target *= (velocity * Time.deltaTime);

        Vector2 posToReach = transform.position + target;

        CorrectAngle(posToReach);

        transform.Translate(target);
    }

    private void CorrectAngle(Vector3 expectedLoc)
    {
        Vector2 E = (expectedLoc - transform.position);

        if(mapBoundary.bounds.min.x > expectedLoc.x)
        {
            target = Vector2.Dot(-E, Vector2.right) * 2.0f * Vector2.right + E;
        }
        else if(mapBoundary.bounds.max.x < expectedLoc.x)
        {
            target = Vector2.Dot(-E, -Vector2.right) * 2.0f * -Vector2.right + E;
        }

        if(mapBoundary.bounds.min.y > expectedLoc.y)
        {
            target = Vector2.Dot(-E, Vector2.up) * 2.0f * Vector2.up + E;
        }
        else if(mapBoundary.bounds.max.y < expectedLoc.y)
        {
            target = Vector2.Dot(-E, -Vector2.up) * 2.0f * -Vector2.up + E;
        }

        directionDegree = Mathf.Acos(Vector2.Dot(target.normalized, Vector2.right)) * Mathf.Rad2Deg;
        if (target.y < Vector2.right.y) directionDegree *= -1.0f;
    }
}
