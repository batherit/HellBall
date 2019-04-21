using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CS_PlayerController : MonoBehaviour {

    public Transform startPoint;
    private float dirX, dirY;
    Rigidbody2D rb;

    public delegate void DELEGATE_Dead();
    public DELEGATE_Dead ED_Dead;

    // Use this for initialization
    void Start () {
        CS_Managers.Instance.gameManager.ED_StartGame += InitForStart;
        CS_Managers.Instance.gameManager.ED_ResetGame += ResetGame;

        ED_Dead += Died;
 
        transform.position = startPoint.position;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
	}

    public void InitForStart()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void ResetGame()
    {
        transform.position = startPoint.position;
    }
	
	// Update is called once per frame
	void Update () {
        if(rb.bodyType == RigidbodyType2D.Dynamic)
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

            rb.AddForce(new Vector2(dirX * Time.deltaTime * 200.0f, dirY * Time.deltaTime * 500.0f));
        }
	}

    void Died()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Obstacle":
            case "Reaper":
                ED_Dead();
                break;
        }
    }
}
