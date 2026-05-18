using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    // Start is called before the first frame update
    Transform cam;
    int health;
    Slider slider = null;
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayDamage()
    {
        this.slider.value = health;
    }

    public void calculateDamage(int health)
    {
        this.health = health;
    }

    public void InitializeMaxHealth(int Maxhealth)
    {
        this.health = Maxhealth;
        var healthCanvas = this.transform.Find("HealthCanvas");
        healthCanvas?.TryGetComponent<Slider>(out slider);
        if (slider != null)
        {
            slider.maxValue = health;
            slider.value = health;
        }
    }

    private void LateUpdate()
    {
        this.transform.forward = -cam.forward;
    }
}
