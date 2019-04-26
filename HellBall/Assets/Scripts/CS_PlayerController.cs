using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_PlayerController : MonoBehaviour {

    public Transform startPoint;
    float dirX, dirY;
    Rigidbody2D rb;

    public Text TEXT_HP;
    int currentHP;
    const int maxHP = 10;

    public delegate void DELEGATE_Dead();
    public DELEGATE_Dead ED_Dead;
    bool isKnockBack;
    bool isUpdated;
    Vector2 target;
    Vector2 posToReach;
    Vector2 dir;
    float length;
    float elapsedTime;
    SpriteRenderer sr;
    bool alphaToggle;
    float toggleTime;

    // Use this for initialization
    void Start () {
        CS_Managers.Instance.gameManager.ED_StartGame += InitForStart;
        CS_Managers.Instance.gameManager.ED_ResetGame += ResetGame;

        ED_Dead += Died;
 
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;

        sr = GetComponent<SpriteRenderer>();

        ResetGame();
    }

    public void InitForStart()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void ResetGame()
    {
        isKnockBack = false;
        isUpdated = true;
        elapsedTime = 0.0f;
        currentHP = maxHP;
        TEXT_HP.text = currentHP.ToString("00") + '/'
                + maxHP.ToString("00");
        transform.position = startPoint.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            if (!isKnockBack)
            {
                dirX = 0.0f;
                dirY = 0.0f;
                if (CS_Managers.Instance.InputManager.IsLeftDown()) { dirX -= 1.0f; }
                if (CS_Managers.Instance.InputManager.IsRightDown()) { dirX += 1.0f; }
                if (CS_Managers.Instance.InputManager.IsJumpDown()) { dirY += 1.0f; }
                if (CS_Managers.Instance.InputManager.IsDiveDown()) { dirY -= 1.0f; }

                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    dirX = Input.GetAxisRaw("Horizontal");
                    dirY = Input.GetAxisRaw("Vertical");
                }
                float deltaTime = Time.deltaTime;
                rb.AddForce(new Vector2(dirX * deltaTime * 200.0f, dirY * deltaTime * 500.0f));
            }
            else
            {
                float deltaTime = Time.deltaTime;
                if (!isUpdated)
                {
                    dir = new Vector2(rb.velocity.x, rb.velocity.y).normalized;
                    rb.gravityScale = 0.0f;
                    rb.velocity = (dir * length * 0.65f) / 0.21f;
                    rb.interpolation = RigidbodyInterpolation2D.None;
                    posToReach = dir * length + new Vector2(transform.position.x, transform.position.y);
                    isUpdated = true;
                }

                if (elapsedTime < 0.21f)
                {
                    toggleTime += deltaTime;
                    elapsedTime += deltaTime;

                    if (toggleTime >= 0.05f - Mathf.Lerp(0.0f, 0.02f, elapsedTime / 0.21f))
                    {
                        alphaToggle = !alphaToggle ? true : false;
                        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alphaToggle ? 1.0f : 0.1f);
                        toggleTime = 0.0f;
                    }
                    target = (posToReach - new Vector2(transform.position.x, transform.position.y)) * 0.2f;
                    length -= target.magnitude;
                    transform.position =
                    new Vector3(
                        transform.position.x + target.x,
                        transform.position.y + target.y,
                        0.0f);
                }
                else
                {
                    elapsedTime = 0.0f;
                    rb.gravityScale = 1.0f;
                    rb.interpolation = RigidbodyInterpolation2D.Extrapolate;
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f);
                    isKnockBack = false;
                }

            }
        }
	}

    void Died()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }

    void Attacked()
    {
        if(rb.bodyType == RigidbodyType2D.Dynamic)
        {
            if(!isKnockBack)
            {
                currentHP--;
                TEXT_HP.text = currentHP.ToString("00") + '/'
                + maxHP.ToString("00");
                isKnockBack = true;
                length = 0.8f;
            }

            isUpdated = false;
            toggleTime = 0.0f;
 
            if(currentHP <= 0)
            {
                ED_Dead();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Obstacle":
            case "Reaper":
                Attacked();
                break;
        }
    }
}
