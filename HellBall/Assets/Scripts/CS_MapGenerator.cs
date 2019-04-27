using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CS_MapGenerator : MonoBehaviour {
    public BoxCollider2D checkPointBoundary;
    public Text TEXT_CheckedPointsNum;

    public const int initObstacleNum = 40;
    public const int initCheckPointNum = 30;
    private int CheckedPointNum;

    GameObject prefabObstacle;
    GameObject prefabCheckPoint;
    GameObject[] obstacles;
    GameObject[] checkPoints;

	// Use this for initialization
	void Start () {
        obstacles = null;
        checkPoints = null;
        prefabObstacle = Resources.Load("Prefabs/ObstacleSet") as GameObject;
        prefabCheckPoint = Resources.Load("Prefabs/CheckPoint") as GameObject;
        CS_Managers.Instance.gameManager.ED_ResetGame += ResetGame;
        TEXT_CheckedPointsNum.text = CheckedPointNum.ToString("00") + '/' + initCheckPointNum.ToString("00");
        GenerateMap();
	}

    void GenerateMap()
    {
        // 정상적인 체크포인트 생성을 위해 장애물을 먼저 생성한다.
        Invoke("GenerateObstacles", 0.0f);

        // 약간의 시간 지연이 존재하여야 장애물이 제대로 설치된다.
        Invoke("GenerateCheckPoints", 0.05f);
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

        
        obstacles = new GameObject[initObstacleNum];

        for (int i = 0; i < initObstacleNum; i++)
        {
            obstacles[i] = Instantiate(prefabObstacle);
            obstacles[i].transform.position = new Vector3(Random.Range(-2.0f, 2.0f), -i * 5.0f - 5.0f, 0.0f);
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
        float gapY = (endY - startY) / initCheckPointNum;
        
        checkPoints = new GameObject[initCheckPointNum];

        for (int i = 0; i < initCheckPointNum; i++)
        {
            bool isGenerated = false;
            checkPoints[i] = Instantiate(prefabCheckPoint);
            float radius = checkPoints[i].GetComponent<CircleCollider2D>().radius * checkPoints[i].transform.localScale.x;

            while (!isGenerated)
            {
                Vector2 pos = new Vector2(Random.Range(startX, endX), Random.Range(startY + i * gapY, startY + (i + 1) * gapY));
                Collider2D hit = Physics2D.OverlapCircle(pos, radius, LayerMask.GetMask(new string[] { "Obstacle" }));
                
                if (!hit)
                {
                    checkPoints[i].transform.position = pos;
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
