using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILabels : MonoBehaviour
{
    private Transform trans;
    private Vector3 offset = new Vector3(0, 180, 0);

    // Start is called before the first frame update
    void Start()
    {
        trans = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(trans);
        this.transform.Rotate(offset);
    }
}
