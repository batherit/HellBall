using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CS_ObstacleGenerator : MonoBehaviour {
    public BoxCollider2D boundary;

    public const int initObstacleNum = 50;
    GameObject prefabObstacle;
    GameObject[] Obstacles;

   public const float upVelPerSec = 2.2f;

	// Use this for initialization
	void Start () {
        prefabObstacle = Resources.Load("Prefabs/ObstacleSet") as GameObject;
        Obstacles = new GameObject[initObstacleNum];

        for(int i = 0; i < initObstacleNum; i++)
        {
            Obstacles[i] = Instantiate(prefabObstacle);
            Obstacles[i].transform.localPosition = new Vector3(Random.Range(-2.0f, 2.0f), -i * 4.0f - 5.0f, 0.0f);
        }
	}

    private void Update()
    {
        //float deltaDistance;

        //for (int i = 0; i < initObstacleNum; i++)
        //{
        //    deltaDistance = upVelPerSec * Time.deltaTime;
        //    Obstacles[i].transform.Translate(new Vector3(0.0f, deltaDistance, 0.0f));
        //    if (Obstacles[i].transform.position.y >= boundary.bounds.max.y + 5.0f)
        //    {
        //        DestroyObject(Obstacles[i]);
        //        Obstacles[i] = Instantiate(prefabObstacle);
        //        Obstacles[i].transform.localPosition = new Vector3(Random.Range(-7.3f, 7.3f), -(initObstacleNum - 2) * 5.0f, 0.0f);
        //    }
        //}
    }
}
