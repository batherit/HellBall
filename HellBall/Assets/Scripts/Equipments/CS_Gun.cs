using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_Gun : CS_Equipment {
    

    enum E_ShotMode { Independent,  Group};   // 독립, 집단
                                              // 독립 : 단발, 연사, 미사일, 레이져, 다연발
                                              // 집단 : 점사, 샷건

    E_ShotMode shotMode;        // 샷 모드

    GameObject bulletPrefab;

    float effectiveRange;       // 유효 사정 거리
    float reboundDegree;        // 반동 정도(총이 뒤로 밀리는 정도)
    float delay;                // 다음 발사까지 딜레이
    float delayInGroupMode;     // 그룹 모드에서의 각 탄환의 발사 시간 간격
    int maxBulletNum;           // 최대 탄환 수 (불변)
    int curBulletNum;           // 현재 탄환 수 (고정)
    float reloadTime;           // 재장전 시간
    float elapsedTime;          // 경과 시간

    private void Start()
    {
        // 정보가 없다면 '권총'을 흉내낸 정보가 담긴다.
        bulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
        shotMode = E_ShotMode.Independent;
        effectiveRange = 100.0f;    // unit m
        reboundDegree = 0.2f;       // 반동 정도
        delay = 0.25f;
        delayInGroupMode = 0.0f;
        maxBulletNum = 6;
        curBulletNum = maxBulletNum;
        reloadTime = 0.2f;
        elapsedTime = reloadTime;
        CS_Managers.Instance.InputManager.joystick.ED_Enter += ResetDelay;
        CS_Managers.Instance.InputManager.joystick.ED_Exit += Reload;
    }

    //private void Update()
    //{
    //    // 반동에 대한 회복
    //    float translate = (springArmLength - imagePosL.localPosition.x) * 0.4f;
    //    imagePosL.localPosition = new Vector3(imagePosL.localPosition.x + translate, 0.0f, 0.0f);
    //}

    public override void AxisAction()
    {
        if(CS_Managers.Instance.InputManager.IsOutsideOfReloadRadius())
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime >= delay)
            {
                if(curBulletNum > 0)
                {
                    curBulletNum--;
                    GameObject newObject = Instantiate(bulletPrefab);
                    CS_Bullet bullet = newObject.GetComponent<CS_Bullet>();
                    bullet.transform.position = GetPosition();
                    bullet.SetInitInfo(currentDir, effectiveRange);
                    ReboundAgainstShot();
                    Debug.Log("Action!");
                }
                elapsedTime = 0.0f;
            }
        }

        // 반동에 대한 회복
        float translate = (springArmLength - imagePosL.localPosition.x) * 0.4f;
        imagePosL.localPosition = new Vector3(imagePosL.localPosition.x + translate, 0.0f, 0.0f);
    }

    public void ReboundAgainstShot()
    {
        imagePosL.localPosition = new Vector3(springArmLength - reboundDegree, 0.0f, 0.0f);
    }

    public override void ResetDelay()
    {
        Debug.Log("ResetDelay");
        elapsedTime = reloadTime;
        //throw new System.NotImplementedException();
    }

    public override void Reload()
    {
        Debug.Log("Reload");
        curBulletNum = maxBulletNum;
        //throw new System.NotImplementedException();
    }
}
