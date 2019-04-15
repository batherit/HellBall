using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CameraController : MonoBehaviour {
    public BoxCollider2D boundary;

    // 박스 컬라이더 영역의 최소 최대 xyz값
    private Vector3 minBound;
    private Vector3 maxBound;

    // 카메라 영역의 반너비, 반높이
    private float halfWidth;
    private float halfHeight;

    private Camera mainCamera;

    public GameObject player;

	// Use this for initialization
	void Start () {
        mainCamera = GetComponent<Camera>();
        minBound = boundary.bounds.min;
        maxBound = boundary.bounds.max;
        halfHeight = mainCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 vTranslate = (player.transform.position - transform.position) * 0.1f;

        transform.Translate(vTranslate);

        float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        this.transform.position = new Vector3(clampedX, clampedY, -10.0f);
	}
}
