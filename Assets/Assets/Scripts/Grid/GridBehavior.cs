using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    public int rows = 10;
    public int columns = 15;
    public float scale = 1.5f;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    public GameObject[,] gridArray;
    public int startX = 0;
    public int startY = 0;
    public int endX = 0;
    public int endY = 0;

    private void Awake()
    {
        gridArray = new GameObject[columns, rows];
        if (gridPrefab)
            GenerateGrid();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + (scale * (float)i), leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);
                obj.name = $"Grid {i} {j}";
                obj.transform.SetParent(gameObject.transform);
                obj.GetComponent<GridStats>().x = i;
                obj.GetComponent<GridStats>().y = j;
                gridArray[i, j] = obj;

                var isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                obj.GetComponent<GridStats>().Init(isOffset);
            }
        }
    }

    void InitialSetup()
    {
        foreach (GameObject obj in gridArray)
        {
            obj.GetComponent<GridStats>().Visited = -1;
        }
        gridArray[startX, startY].GetComponent<GridStats>().Visited = 0;
    }

    bool TestDirection(int x, int y , int step , int direction)
    {
        // 1 is UP
        // 2 is Right
        // 3 is Down
        // 4 is Left

        switch (direction)
        {
            case 1:
                if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStats>().Visited == step)
                    return true;
                else
                    return false;

            case 2:
                if (x + 1 < columns && gridArray[x + 1 , y ] && gridArray[x + 1, y ].GetComponent<GridStats>().Visited == step)
                    return true;
                else
                    return false;

            case 3:
                if (y - 1 > -1  && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStats>().Visited == step)
                    return true;
                else
                    return false;

            case 4:
                if (x - 1 > -1 && gridArray[x - 1 , y] && gridArray[x - 1 , y ].GetComponent<GridStats>().Visited == step)
                    return true;
                else
                    return false;

            default:
                return false;
        }
    }

    void SetupVisited(int x , int y , int step)
    {
        if (gridArray[x, y])
            gridArray[x, y].GetComponent<GridStats>().Visited = step;
    }

    void SetDistance()
    {
        InitialSetup();
        int x = startX;
        int y = startY;
        int[] testArray = new int[rows * columns];
        for (int step = 1; step < columns * rows; step++)
        {
            foreach (GameObject obj in gridArray)
            {
                if(obj.GetComponent<GridStats>().Visited == step-1)
                    TestFourDirections(obj.GetComponent<GridStats>().x, obj.GetComponent<GridStats>().y, step);

            }
        }
    }

    void TestFourDirections(int x , int y , int step)
    {
        if(TestDirection(x,y,-1, 1))
            SetupVisited(x, y + 1, step);
        if(TestDirection(x, y, -1, 2))
            SetupVisited(x + 1, y, step);
        if(TestDirection(x, y, -1, 3))
            SetupVisited(x, y - 1, step);
        if(TestDirection(x, y, -1, 4))
            SetupVisited(x - 1, y, step);
    }
}
