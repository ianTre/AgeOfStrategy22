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
    public Color playerColor;
    public Color enemyColor;
    public HealthValue healthValue;
    public Vector3 LowScale = new Vector3(0.1f, 0.07f, 0.07f);
    public Vector3 HighScale = new Vector3(0.2f, 0.15f, 0.15f);
    void Start()
    {
        cam = Camera.main.transform;
        healthValue.SetInitialHealthValue(health);
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

        var unit = GetComponentInParent<Unit>();
        var fill = healthCanvas.transform.Find("Fill");
        if (unit == null || fill == null)
        {
            Debug.LogError("Can find Unit in parent object or Slider named Fill");
        }

        fill.GetComponent<Image>().color = unit.isPlayerUnit ? playerColor : enemyColor;

    }

    private void LateUpdate()
    {
        this.transform.forward = -cam.forward;
    }
    public void OnBarHover()
    {
            healthValue?.ShowHealthValue(health.ToString());
            this.transform.localScale = HighScale;

    }
    public void OnBarExit()
    {
            healthValue?.HideHealthValue();
            this.transform.localScale = LowScale;        
    }
}
