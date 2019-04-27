﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Equipment : MonoBehaviour {

    protected float springArmLength;
    protected Transform imagePosL;
    protected Vector2 currentDir;
    protected float currentDegree;
    protected Vector2 targetDir;
    protected float targetDegree;
    protected float rotCoefficient;

    private void Awake()
    {
        springArmLength = GameObject.Find("Player").GetComponent<CircleCollider2D>().radius + 0.5f;
        imagePosL = transform.Find("Image").gameObject.transform;
        imagePosL.position = new Vector3(springArmLength, 0.0f, 0.0f);
        // 무기가 위쪽을 향하도록 각도를 조정.
        currentDegree = 0.0f;
        currentDir = new Vector2(1.0f, 0.0f);
        targetDegree = 90.0f;
        targetDir = new Vector2(0.0f, 1.0f);
        rotCoefficient = 0.08f;
    }

    public virtual void SetTargetDir(Vector2 _targetDir)
    {
        if (_targetDir.x == 0.0f && _targetDir.y == 0.0f) return;
        targetDir = _targetDir.normalized;
        float dot = Vector2.Dot(targetDir, Vector2.right);
        float targetDegree = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (targetDir.y <= 0.0f) targetDegree = 360.0f - targetDegree;
    }

    private void Update()
    {
        // 타겟 각도를 구함.
        float dot = Vector2.Dot(currentDir, targetDir);
        float toDegree = Mathf.Acos(dot) * Mathf.Rad2Deg;

        // 회전축을 구함.
        Vector3 cross = Vector3.Cross(currentDir, targetDir); ;
        // 타겟 각도를 보정함.
        if (cross.z < 0.0f) toDegree *= -1.0f;

        // 타겟으로의 회전.
        toDegree *= rotCoefficient;
        transform.Rotate(new Vector3(0.0f, 0.0f, toDegree));

        currentDegree += toDegree;
        currentDir = new Vector2(Mathf.Cos(currentDegree * Mathf.Deg2Rad), Mathf.Sin(currentDegree * Mathf.Deg2Rad));
    }

    public float GetTargetDeg()
    {
        return targetDegree;
    }

    public virtual void Action()
    {
        Debug.Log("EquipmentAction");
    }
}