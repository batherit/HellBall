using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CS_PlayerController : MonoBehaviour {

    private float dirX, dirY;
    CS_UIButtonManager UIButtonManager;
    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        UIButtonManager = GameObject.Find("Managers").GetComponent<CS_UIButtonManager>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        dirX = 0.0f;
        dirY = 0.0f;
        if(UIButtonManager.IsLeftDown()) { dirX -= 1.0f; }
        if(UIButtonManager.IsRightDown()) { dirX += 1.0f; }
        if(UIButtonManager.IsJumpDown()) { dirY += 1.0f; }
        if(UIButtonManager.IsDiveDown()) { dirY -= 1.0f; }

        rb.AddForce(new Vector2(dirX * Time.deltaTime * 200.0f, dirY * Time.deltaTime * 500.0f));
	}
}
