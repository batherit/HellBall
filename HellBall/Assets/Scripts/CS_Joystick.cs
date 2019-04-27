using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CS_Joystick : MonoBehaviour {

    public Transform stick;     // 조이스틱

    Vector3 stickFirstPos;      // 조이스틱 처음 위치
    Vector3 stickDir;           // 조이스틱의 방향

    public CircleCollider2D marginCircle;   // 조이스틱 움직임 한계 범위
    float marginRadius;
    public CircleCollider2D reloadCircle;   // 리로드 가능 범위
    float reloadRadius;

	// Use this for initialization
	void Start () {
        stickFirstPos = stick.transform.position;
        marginRadius = marginCircle.radius;
        reloadRadius = reloadCircle.radius;
	}

    public bool IsStickDragging()
    {
        if (stickDir.x != 0.0f || stickDir.y != 0.0f) return true;
        return false;
    }
    
    public Vector2 GetStickDirection()
    {
        return stickDir;
    }

    public void Drag(BaseEventData _data)
    {
        PointerEventData data = _data as PointerEventData;
        Vector3 pos = data.position;

        // 조이스틱 벡터를 구함.
        stickDir = (pos - stickFirstPos).normalized;

        // 조이스틱의 처음 위치와 현재 내가 터치하고 있는 위치의 거리를 구함.
        float dist = Vector3.Distance(pos, stickFirstPos);

        // 거리가 반지름보다 작으면 조이스틱을 현재 터치하고 있는 곳으로 이동.
        if(dist < marginRadius)
        {
            stick.position = stickFirstPos + stickDir * dist;
        }
        else
        {
            stick.position = stickFirstPos + stickDir * marginRadius;
        }
    }

    public void DragEnd()
    {
        stick.position = stickFirstPos;     // 스틱을 원래 위치로.
        stickDir = Vector3.zero;            // 벡터를 초기화
    }
}
