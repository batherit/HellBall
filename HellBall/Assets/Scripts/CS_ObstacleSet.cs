using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ObstacleSet : MonoBehaviour {

    public GameObject ObstacleLeft;
    public GameObject ObstacleRight;

    // Use this for initialization
    void Start()
    {
        // this의 position y값은 ObstacleGenerator가 지정한다.
        ObstacleLeft.transform.localPosition = new Vector3(Random.Range(-7.85f, -6.25f), 0.0f, 0.0f);
        ObstacleRight.transform.localPosition = new Vector3(Random.Range(6.25f, 7.85f), 0.0f, 0.0f);
    }
}
