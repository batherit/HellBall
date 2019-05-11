using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_Bullet : MonoBehaviour {

    float effectiveRange;
    float travelDistance;
    float velocity;
    Vector2 dir;
    int damage;
    bool isActivated;

    public AudioSource soundAttack;

	// Use this for initialization
	void Awake () {
        // 정보가 없다면 '평범한 총알'을 흉내낸 정보가 담긴다.
        velocity = 15.0f; // unit sec
        damage = 2;
        travelDistance = 0.0f;
        isActivated = false;
	}

    public void SetInitInfo(Vector2 _dir, float _effectiveRange)
    {
        isActivated = true;
        effectiveRange = _effectiveRange;
        dir = _dir;
    }

    void Update()
    {
        if (isActivated)
        {   
            float deltaTime = Time.deltaTime;
            Vector2 translate = new Vector2(dir.x * velocity * deltaTime, dir.y * velocity * deltaTime);
            travelDistance += translate.magnitude;
            
            if(travelDistance >= effectiveRange)
            {
                DestroyObject(this.gameObject);
            }

            transform.Translate(translate);
        }
    }

    public Vector2 GetVelocity()
    {
        return dir * velocity;
    }
}
