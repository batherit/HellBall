using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CS_MapGenerator : MonoBehaviour {
    public BoxCollider2D checkPointBoundary;
    public Text TEXT_CheckedPointsNum;

    public const int initObstacleNum = 40;
    public const int initCheckPointNum = 1;
    private int CheckedPointNum;

    GameObject prefabObstacle;
    GameObject prefabCheckPoint;
    GameObject[] obstacles;
    GameObject[] checkPoints;

	// Use this for initialization
	void Start () {
        CS_Managers.Instance.gameManager.ED_ResetGame += ResetGame;
        TEXT_CheckedPointsNum.text = CheckedPointNum.ToString("00") + '/' + initCheckPointNum.ToString("00");
        GenerateMap();
	}

    void GenerateMap()
    {
        // 정상적인 체크포인트 생성을 위해 장애물을 먼저 생성한다.
        GenerateObstacles();
        GenerateCheckPoints();
    }

    void GenerateObstacles()
    {
        if (obstacles != null)
        {
            for (int i = 0; i < initObstacleNum; i++)
            {
                Destroy(obstacles[i]);
                obstacles[i] = null;
            }
            obstacles = null;
        }

        prefabObstacle = Resources.Load("Prefabs/ObstacleSet") as GameObject;
        obstacles = new GameObject[initObstacleNum];

        for (int i = 0; i < initObstacleNum; i++)
        {
            obstacles[i] = Instantiate(prefabObstacle);
            obstacles[i].transform.localPosition = new Vector3(Random.Range(-2.0f, 2.0f), -i * 5.0f - 5.0f, 0.0f);
        }
    }

    void GenerateCheckPoints()
    {
        CheckedPointNum = 0;
        if (checkPoints != null)
        {
            for (int i = 0; i < initCheckPointNum; i++)
            {
                Destroy(checkPoints[i]);
                checkPoints[i] = null;
            }
            checkPoints = null;
        }

        float startX = checkPointBoundary.bounds.min.x;
        float startY = checkPointBoundary.bounds.min.y;
        float endX = checkPointBoundary.bounds.max.x;
        float endY = checkPointBoundary.bounds.max.y;
        float gapY = (endY - startY) / (float)initCheckPointNum;
        prefabCheckPoint = Resources.Load("Prefabs/CheckPoint") as GameObject;
        checkPoints = new GameObject[initCheckPointNum];

        for (int i = 0; i < initCheckPointNum; i++)
        {
            bool isGenerated = false;
            while (!isGenerated)
            {
                Ray2D ray = new Ray2D(new Vector2(Random.Range(startX, endX), Random.Range(startY + (float)i * gapY, startY + (float)(i + 1) * gapY)), Vector2.zero);

                if (!Physics.Raycast(ray.origin, ray.direction))
                {
                    checkPoints[i] = Instantiate(prefabCheckPoint);
                    checkPoints[i].transform.localPosition = ray.origin;
                    checkPoints[i].GetComponent<CS_CheckPoint>().mapGenerator = this;
                    isGenerated = true;
                }
            }
        }

        TEXT_CheckedPointsNum.text = CheckedPointNum.ToString("00") + '/' + initCheckPointNum.ToString("00");
    }

    public void IncreaseCheckedPointsNum()
    {
        CheckedPointNum++;
        TEXT_CheckedPointsNum.text = CheckedPointNum.ToString("00") + '/' + initCheckPointNum.ToString("00");

        if (CheckedPointNum == initCheckPointNum)
        {
            CS_Managers.Instance.gameManager.player.ED_Dead();
        }
    }

    void ResetGame()
    {
        GenerateMap();
    }
}
