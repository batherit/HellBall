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
    bool isReloadCompleted;     // 장전이 완료되었는지?

    private void Start()
    {
        S_GunInfo defaultGunInfo = CS_DataBase.Instance.GetGunInfo("Gun");
        // 정보가 없다면 '권총'을 흉내낸 정보가 담긴다.
        bulletPrefab = Resources.Load("Prefabs/Bullet") as GameObject;
        SetGunInfo(defaultGunInfo);
        CS_Managers.Instance.InputManager.joystick.ED_Enter += ResetDelay;
        CS_Managers.Instance.InputManager.joystick.ED_Exit += Reload;
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

            if(!isReloadCompleted)
            {
                if(elapsedTime >= reloadTime)
                {
                    curBulletNum = maxBulletNum;
                    isReloadCompleted = true;
                    elapsedTime = reloadTime;
                }
            }
            else
            {
                if (elapsedTime >= delay)
                {
                    if (curBulletNum > 0)
                    {
                        curBulletNum--;
                        soundBang.Play();
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
        // Debug.Log("ResetDelay");
        elapsedTime = 0.0f;
        isReloadCompleted = false;
        // elapsedTime = 0.0f;
        // throw new System.NotImplementedException();
    }

    public override void Reload()
    {
        // Debug.Log("Reload");
        // curBulletNum = maxBulletNum;
        soundReload.Play();
        // throw new System.NotImplementedException();
    }
}
