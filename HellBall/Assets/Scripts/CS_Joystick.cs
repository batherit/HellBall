using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CS_Joystick : MonoBehaviour {

    public Transform stick;     // 조이스틱
    public Image image;         // 조이스틱
    private Sprite normSprite;
    private Sprite execSprite;

    Vector3 stickFirstPos;      // 조이스틱 처음 위치
    Vector3 stickDir;           // 조이스틱의 방향

    public CircleCollider2D marginArea;   // 조이스틱 움직임 한계 범위
    float marginRadius;
    public CircleCollider2D standbyArea;   // 리로드 가능 범위
    float standbyRadius;
    bool isOutsideOfReloadRadius;

    public delegate void DELEGATE_OnReloadRadiusEnter();
    public DELEGATE_OnReloadRadiusEnter ED_Standby;
    public delegate void DELEGATE_OnReloadRadiusExit();
    public DELEGATE_OnReloadRadiusExit ED_Action;
    public delegate void DELEGATE_OnPointerDown();
    public DELEGATE_OnPointerDown ED_StickDown;
    //public delegate void DELEGATE_OnPointerUp();
    //public DELEGATE_OnPointerUp ED_Up;

    // Use this for initialization
    private void Start()
    {
        stickFirstPos = stick.transform.position;
        marginRadius = marginArea.radius;
        standbyRadius = standbyArea.radius;
        normSprite = Resources.Load<Sprite>("Textures/TX_Joypad");
        execSprite = Resources.Load<Sprite>("Textures/TX_Joypad_Exec");
        isOutsideOfReloadRadius = false;
    }

    public bool IsStickDragging()
    {
        if (stickDir.x != 0.0f || stickDir.y != 0.0f) return true;
        return false;
    }

    public bool IsOutsideOfReloadRadius()
    {
        return isOutsideOfReloadRadius;
    }
    
    public Vector2 GetStickDirection()
    {
        return stickDir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ED_Standby();
        image.sprite = normSprite;
        isOutsideOfReloadRadius = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        image.sprite = execSprite;
        isOutsideOfReloadRadius = true;
    }

    public void OnPointerDown(BaseEventData _data)
    {
        ED_StickDown();
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
