using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_Bullet : MonoBehaviour {

    float velocity;
    Vector2 dir;
    int damage;
    bool isActivated;

	// Use this for initialization
	void Awake () {
        // 정보가 없다면 '평범한 총알'을 흉내낸 정보가 담긴다.
        velocity = 15.0f; // unit sec
        damage = 2;
        isActivated = false;
	}

    public void SetDirection(Vector2 _dir)
    {
        isActivated = true;
        dir = _dir;
    }

    void Update()
    {
        if (isActivated)
        {   
            float deltaTime = Time.deltaTime;
            transform.Translate(new Vector2(dir.x * velocity * deltaTime, dir.y * velocity * deltaTime));
        }
    }
}
