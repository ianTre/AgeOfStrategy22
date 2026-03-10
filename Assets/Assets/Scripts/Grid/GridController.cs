using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int rows = 10;
    public int columns = 15;
    public float scale = 1.5f;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    public GameObject[,] gridArray;
    public List<GridCell> cellsList;
    public int startX = 0;
    public int startY = 0;
    public int endX = 0;
    public int endY = 0;
    public List<GridCell> pathList = new List<GridCell>();

    private void Awake()
    {
        cellsList = new List<GridCell>();
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
                obj.GetComponent<GridCell>().x = i;
                obj.GetComponent<GridCell>().y = j;
                WriteNameOnTextComponent(obj);
                gridArray[i, j] = obj;
                var isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                obj.GetComponent<GridCell>().Init(isOffset);
                cellsList.Add(obj.GetComponent<GridCell>());
            }
        }
    }

    private void WriteNameOnTextComponent(GameObject obj)
    {
        var textTransform = obj.transform.Find("InfoText");
        if(textTransform != null)
        {
            var text = textTransform.GetComponent<TextMeshPro>();
            if(text != null)
                text.text = obj.name;
        }
    }

    private void WriteDistanceOnTextComponent(GridCell obj)
    {
        var textTransform = obj.transform.Find("InfoText");
        if (textTransform != null)
        {
            var text = textTransform.GetComponent<TextMeshPro>();
            if (text != null)
                text.text += " D: " +obj.visited.ToString();
        }
    }

    void InitialSetup()
    {
        foreach (GameObject obj in gridArray)
        {
            obj.GetComponent<GridCell>().visited = -1;
        }
        gridArray[startX, startY].GetComponent<GridCell>().visited = 0;
    }

    void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        pathList.Clear();
        if (gridArray[x,y] && gridArray[x, y].GetComponent<GridCell>().visited > 0)
        {
            pathList.Add(gridArray[x, y].GetComponent<GridCell>());
            step = gridArray[x,y].GetComponent<GridCell>().visited - 1 ;
        }
        else
        {
            Debug.Log("No path found");
            return;
        }

        for (int i = step; step > -1 ; step--)
        {
            GridCell lastObj = pathList.Find(grid => grid.visited == step+1);
            x = lastObj.x;
            y = lastObj.y;
            if (TestDirection(x, y, step, 1))
            {
                pathList.Add(gridArray[x, y + 1].GetComponent<GridCell>());
                continue;
            }
            if (TestDirection(x, y, step, 2))
            {
                pathList.Add(gridArray[x + 1 , y ].GetComponent<GridCell>());
                continue;
            }
            if (TestDirection(x, y, step, 3))
            {
                pathList.Add(gridArray[x, y - 1].GetComponent<GridCell>());
                continue;
            }
            if (TestDirection(x, y, step, 4))
            {
                pathList.Add(gridArray[x - 1, y].GetComponent<GridCell>());
                continue;
            }
        }
    }

    public List<GridCell> FindShortestPath(GridCell origin, GridCell destiniy)
    {
        this.startX = origin.x;
        this.startY = origin.y;
        this.endX = destiniy.x;
        this.endY = destiniy.y;

        SetDistance();
        SetPath();
        return pathList.OrderBy(cell => cell.visited).ToList();
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
                if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridCell>().visited == step && !gridArray[x , y + 1 ].GetComponent<GridCell>().isOccupied)
                    return true;
                else
                    return false;

            case 2:
                if (x + 1 < columns && gridArray[x + 1 , y ] && gridArray[x + 1, y ].GetComponent<GridCell>().visited == step && !gridArray[x+1 , y].GetComponent<GridCell>().isOccupied)
                    return true;
                else
                    return false;

            case 3:
                if (y - 1 > -1  && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridCell>().visited == step && !gridArray[x,y-1].GetComponent<GridCell>().isOccupied)
                    return true;
                else
                    return false;

            case 4:
                if (x - 1 > -1 && gridArray[x - 1 , y] && gridArray[x - 1 , y ].GetComponent<GridCell>().visited == step && !gridArray[x-1,y].GetComponent<GridCell>().isOccupied)
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
        {
            GridCell gridCell = gridArray[x, y].GetComponent<GridCell>();
            gridCell.visited = step;
            WriteDistanceOnTextComponent(gridCell);
        }
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
                if(obj.GetComponent<GridCell>().visited == step-1)
                    TestFourDirections(obj.GetComponent<GridCell>().x, obj.GetComponent<GridCell>().y, step);
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
