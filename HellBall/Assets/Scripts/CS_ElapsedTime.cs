using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_ElapsedTime : MonoBehaviour {

    public Text TEXT_elapsedTime;
    private bool isOn;
    private float elapsedTime;
	// Use this for initialization
	void Start () {
        elapsedTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if(isOn)
        {
            elapsedTime += Time.deltaTime;

            UpdateText();
        }
	}

    public void UpdateText()
    {
        TEXT_elapsedTime.text =
                ConvertToHour(elapsedTime).ToString("00") +
                ":" + (ConvertToMinute(elapsedTime) % 60).ToString("00") +
                ":" + (ConvertToSecond(elapsedTime) % 60).ToString("00");
    }

    public void On()
    {
        isOn = true;
    }

    public void Off()
    {
        isOn = false;
    }

    public void Zero()
    {
        elapsedTime = 0.0f;

        UpdateText();
    }

    public float Get()
    {
        return elapsedTime;
    }

    int ConvertToHour(float elapsedTime)
    {
        return ConvertToMinute(elapsedTime) / 60;
    }

    int ConvertToMinute(float elapsedTime)
    {
        return (int)Mathf.Floor(elapsedTime) / 60;
    }

    int ConvertToSecond(float elapsedTime)
    {
        return (int)Mathf.Floor(elapsedTime);
    }
}
