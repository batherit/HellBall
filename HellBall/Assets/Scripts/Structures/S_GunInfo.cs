using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_ShotMode { Independent, Group };   // 독립, 집단
                                                 // 독립 : 단발, 연사, 미사일, 레이져, 다연발
                                                 // 집단 : 점사, 샷건

[System.Serializable]
public class S_GunInfo {
    public E_ShotMode shotMode;
    public float effectiveRange;       // 유효 사정 거리
    public float reboundDegree;        // 반동 정도(총이 뒤로 밀리는 정도)
    public float delay;                // 다음 발사까지 딜레이
    public float delayInGroupMode;     // 그룹 모드에서의 각 탄환의 발사 시간 간격
    public int maxBulletNum;           // 최대 탄환 수 (불변)
    public float reloadTime;           // 재장전 시간

    public S_GunInfo(E_ShotMode _shotMode, float _effectiveRange, float _reboundDegree, float _delay, float _delayInGroupMode, int _maxBulletNum, float _reloadTime)
    {
        effectiveRange = _effectiveRange;
        shotMode = _shotMode;
        reboundDegree = _reboundDegree;
        delay = _delay;
        delayInGroupMode = _delayInGroupMode;
        maxBulletNum = _maxBulletNum;
        reloadTime = _reloadTime;
    }
}
