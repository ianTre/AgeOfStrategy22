using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionLightController : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.5f; // altura del movimiento
    [SerializeField] private float speed = 0.5f;     // velocidad (frecuencia) del movimiento
    [SerializeField] private float upLerpSpeed = 4f; // velocidad para mantener el 'up' alineado

    private Vector3 startLocalPos;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Mantener la orientaci�n hacia arriba de forma suave
        transform.up = Vector3.Lerp(transform.up, Vector3.up, Time.deltaTime * upLerpSpeed);

        // Movimiento vertical infinito usando seno
        float y = Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = new Vector3(transform.localPosition.x, startLocalPos.y + (Vector3.up * y).y, transform.localPosition.z);
        
    }
}
