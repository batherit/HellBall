using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_PlayerController : MonoBehaviour {

    public Transform startPoint;
    private float dirX, dirY;
    Rigidbody2D rb;

    public Text TEXT_HP;
    private int currentHP;
    private const int maxHP = 10;

    public delegate void DELEGATE_Dead();
    public DELEGATE_Dead ED_Dead;
    bool isKnockBack;
    private IEnumerator KnockBackCoroutine;

    // Use this for initialization
    void Start () {
        CS_Managers.Instance.gameManager.ED_StartGame += InitForStart;
        CS_Managers.Instance.gameManager.ED_ResetGame += ResetGame;

        ED_Dead += Died;
 
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        ResetGame();

    }

    public void InitForStart()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void ResetGame()
    {
        isKnockBack = false;
        currentHP = maxHP;
        TEXT_HP.text = currentHP.ToString("00") + '/'
                + maxHP.ToString("00");
        transform.position = startPoint.position;
    }
	
	// Update is called once per frame
	void Update () {
        if(rb.bodyType == RigidbodyType2D.Dynamic && !isKnockBack)
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
	}

    void Died()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }

    IEnumerator KnockBack(float duration, Vector2 dir)
    {
        isKnockBack = true;
        rb.gravityScale = 0.0f;
        rb.interpolation = RigidbodyInterpolation2D.None;
        float elapsedTime = 0.0f;

        Vector2 posToReach = dir * 0.8f + new Vector2(transform.position.x, transform.position.y);
        Vector2 target;
        float deltaTime;
        //rb.bodyType = RigidbodyType2D.Static;
        while(elapsedTime < duration)
        {
            deltaTime = Time.deltaTime;
            elapsedTime += deltaTime;
            yield return new WaitForSeconds(deltaTime);
            target = (posToReach - new Vector2(transform.position.x, transform.position.y)) * 0.2f;
            transform.position =
                new Vector3(
                    transform.position.x + target.x,
                    transform.position.y + target.y,
                    0.0f);
            
        }
        //rb.bodyType = RigidbodyType2D.Dynamic;
        isKnockBack = false;
        rb.gravityScale = 1.0f;
        rb.interpolation = RigidbodyInterpolation2D.Extrapolate;
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
            }    

            //if (KnockBackCoroutine != null)
            //{
            //    StopCoroutine(KnockBackCoroutine);
            //    KnockBackCoroutine = null;
            //}
            KnockBackCoroutine = KnockBack(0.2f, new Vector2(rb.velocity.x, rb.velocity.y).normalized);
            StartCoroutine(KnockBackCoroutine);
            
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
