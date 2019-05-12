using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum E_BulletType { Independent, Group };   // 독립, 집단
                                                 // 독립 : 단발, 연사, 미사일, 레이져, 다연발
                                                 // 집단 : 점사, 샷건

public class CS_Bullet : MonoBehaviour {

    E_BulletType bulletType;
    GameObject bulletPrefab;
    float effectiveRange;
    float travelDistance;
    int spreadNum;        // 슈팅할 탄환 수
    float spreadDegree;     // 분사각
    float velocity;
    Vector2 dir;
    int damage;
    bool isActivated;

    public AudioSource soundAttack;

	// Use this for initialization
	void Awake () {
        // 정보가 없다면 '평범한 총알'을 흉내낸 정보가 담긴다.
        bulletType = E_BulletType.Independent;
        spreadNum = 1;
        spreadDegree = 0.0f;
        velocity = 15.0f; // unit sec
        damage = 2;
        travelDistance = 0.0f;
        isActivated = false;
	}

    public void SetInitInfo(Vector2 _dir, float _velocity, float _effectiveRange, E_BulletType _bulletType = E_BulletType.Independent,int _spreadNum = 1, float _spreadDegree = 0.0f, string _bulletName = "Bullet")
    {
        isActivated = true;
        bulletType = _bulletType;
        effectiveRange = _effectiveRange;
        dir = _dir;
        velocity = _velocity;
        spreadNum = _spreadNum;
        spreadDegree = _spreadDegree;

        if (bulletType == E_BulletType.Group)
        {
            bulletPrefab = Resources.Load("Prefabs/" + _bulletName) as GameObject;
            
            for(int i = 0; i < spreadNum; i++)
            {
                Quaternion rot = Quaternion.Euler(0.0f, 0.0f, Random.Range(-spreadDegree / 2.0f, spreadDegree / 2.0f));  // 회전각 
                Vector2 rottedDir = rot * dir;
                GameObject newObject = Instantiate(bulletPrefab);
                CS_Bullet bullet = newObject.GetComponent<CS_Bullet>();
                bullet.transform.position = transform.position;
                bullet.SetInitInfo(rottedDir, velocity, effectiveRange);
            }

            DestroyObject(this.gameObject);
        }
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
