using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CS_Gun : CS_Equipment {

    E_ShotMode shotMode;        // 샷 모드

    GameObject bulletPrefab;
    public AudioSource soundBang;
    public AudioSource soundReload;

    float effectiveRange;       // 유효 사정 거리
    float reboundDegree;        // 반동 정도(총이 뒤로 밀리는 정도)
    float delay;                // 다음 발사까지 딜레이
    float delayInGroupMode;     // 그룹 모드에서의 각 탄환의 발사 시간 간격
    int maxBulletNum;           // 최대 탄환 수 (불변)
    int curBulletNum;           // 현재 탄환 수 (유동)
    float reloadTime;           // 재장전 시간
    float elapsedTime;          // 경과 시간
    float remainingTime;        // 잔여 시간
    bool isReloadCompleted;     // 장전이 완료되었는지?

    private void Start()
    {
        S_GunInfo defaultGunInfo = CS_DataBase.Instance.GetGunInfo("Rifle");
        // 정보가 없다면 '권총'을 흉내낸 정보가 담긴다.
        bulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
        SetGunInfo(defaultGunInfo);
        CS_Managers.Instance.InputManager.joystick.ED_Standby += UpdateRemainingTime;
        CS_Managers.Instance.InputManager.joystick.ED_Action += UpdateElapsedTime;
        CS_Managers.Instance.InputManager.joystick.ED_StickDown += Reload;
    }

    public void SetGunInfo(S_GunInfo gunInfo)
    {
        shotMode = gunInfo.shotMode;
        effectiveRange = gunInfo.effectiveRange;
        reboundDegree = gunInfo.reboundDegree;
        delay = gunInfo.delay;
        delayInGroupMode = gunInfo.delayInGroupMode;
        maxBulletNum = gunInfo.maxBulletNum;
        curBulletNum = maxBulletNum;
        reloadTime = gunInfo.reloadTime;
        isReloadCompleted = false;
        elapsedTime = 0.0f;
    }

    public override void AxisAction()
    {
        // 장전 요청이 들어오면 장전을 수행함.
        if (!isReloadCompleted)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= reloadTime)
            {
                curBulletNum = maxBulletNum;
                isReloadCompleted = true;
                // 장전이 완료되면 바로 쏠 수 있도록 함.
                remainingTime = 0.0f;
                elapsedTime = delay - remainingTime;
            }
        }

        if (CS_Managers.Instance.InputManager.IsOutsideOfReloadRadius())
        {
            //elapsedTime = delay - remainingTime;
            if(isReloadCompleted)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= delay)
                {
                    if (curBulletNum > 0)
                    {
                        curBulletNum--;
                        soundBang.Play();
                        GameObject newObject = Instantiate(bulletPrefab);
                        CS_Bullet bullet = newObject.GetComponent<CS_Bullet>();
                        bullet.transform.position = GetPosition();
                        //bullet.SetInitInfo(currentDir, 25.0f, 4.0f, E_BulletType.Group, 8, 20.0f);
                        bullet.SetInitInfo(currentDir, 25.0f, 100.0f);
                        ReboundAgainstShot();
                    }
                    elapsedTime = 0.0f;
                }
            }
        }
        else
        {
            remainingTime = Mathf.Clamp(remainingTime - Time.deltaTime, 0.0f, delay);
        }

        // 반동에 대한 회복
        float translate = (springArmLength - imagePosL.localPosition.x) * 0.4f;
        imagePosL.localPosition = new Vector3(imagePosL.localPosition.x + translate, 0.0f, 0.0f);
    }

    public void ReboundAgainstShot()
    {
        imagePosL.localPosition = new Vector3(springArmLength - reboundDegree, 0.0f, 0.0f);
    }

    // 스텐바이 상태로 돌입할 때 갱신된다.
    public void UpdateRemainingTime()
    {
        Debug.Log("Standby");
        remainingTime = delay - elapsedTime;    
    }

    public void UpdateElapsedTime()
    {
        Debug.Log("Action");
        elapsedTime = delay - remainingTime;
    }

    public void Reload()
    {
        elapsedTime = 0.0f;
        isReloadCompleted = false;
        soundReload.Play();
    }
}
