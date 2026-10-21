using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateUIController : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float bounceDistance = 0.2f;
    [SerializeField] private float bounceTime = 1f;

    [Header("Scale Settings")]
    [SerializeField] private float minScale = 0.95f;
    [SerializeField] private float maxScale = 1.05f;

    private Vector3 initialPosition;
    private Vector3 initialScale;

    private void Awake()
    {
        initialPosition = transform.localPosition;
        initialScale = transform.localScale;
    }

    private void Update()
    {
        // Smooth vertical bounce
        float cycle = Mathf.PingPong(
            Time.time / Mathf.Max(bounceTime, 0.01f), 1f);

        float bounce = Mathf.Sin(cycle * Mathf.PI) * bounceDistance;

        transform.localPosition = initialPosition +
            Vector3.up * bounce;

        // Smooth scale animation
        float scale = Mathf.Lerp(minScale, maxScale, cycle);

        transform.localScale = initialScale * scale;
    }
}
