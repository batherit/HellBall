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
        delay = 0.25f;
        delayInGroupMode = 0.0f;
        maxBulletNum = 6;
        curBulletNum = maxBulletNum;
        reloadTime = 0.2f;
        elapsedTime = reloadTime;
        CS_Managers.Instance.InputManager.joystick.ED_Enter += ResetDelay;
        CS_Managers.Instance.InputManager.joystick.ED_Exit += Reload;
    }

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
                    bullet.SetDirection(currentDir);
                    Debug.Log("Action!");
                }
                elapsedTime = 0.0f;
            }
        }
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
