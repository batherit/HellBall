using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Reaper : MonoBehaviour {

    private GameObject player;
    private BoxCollider2D mapBoundary;
    private Rigidbody2D rb;

    public CircleCollider2D wanderCircle;
    public CircleCollider2D searchRange;

    private float directionDegree;
    private Vector3 target;
    private bool isKnockBack;
    
    private float knockbackLength;
    const float knockbackTime = 0.21f;
    private Vector2 knockbackDir;
    private Vector2 posToReach;
    private float elapsedTime;

    private const float jitter = 10.0f;
    private const float radius = 10.0f;
    private const float velocity = 3.0f;

    public bool isActived;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            if(!isKnockBack)
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

        if (isKnockBack)
        {
            float deltaTime = Time.deltaTime;
            if (elapsedTime < knockbackTime)
            {
                elapsedTime += deltaTime;

                Vector2 target = (posToReach - new Vector2(transform.position.x, transform.position.y)) * 0.2f;
                knockbackLength -= target.magnitude;
                transform.position =
                new Vector3(
                    transform.position.x + target.x,
                    transform.position.y + target.y,
                    0.0f);
            }
            else
            {
                elapsedTime = 0.0f;
                isKnockBack = false;
            }
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
        elapsedTime = 0.0f;
        isKnockBack = false;
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

    void Attacked(CS_Bullet _bullet)
    {
        isKnockBack = true;
        knockbackLength = 0.8f;

        knockbackDir = new Vector2(_bullet.GetVelocity().x, _bullet.GetVelocity().y).normalized;
        posToReach = knockbackDir * knockbackLength + new Vector2(transform.position.x, transform.position.y);

        _bullet.soundAttack.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            CS_Bullet bullet = collision.gameObject.GetComponent<CS_Bullet>();
            Attacked(bullet);

            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            
            DestroyObject(collision.gameObject, 3f);
        }
    }
}
