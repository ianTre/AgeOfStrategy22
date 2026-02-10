using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStats : MonoBehaviour
{
    public int Visited = 1;
    public int x = 0;
    public int y = 0;
    public Material Normal, Offset, Highligh;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(bool isOffset)
    {
        this.GetComponent<MeshRenderer>().material = isOffset ? Offset : Normal;
    }
}
