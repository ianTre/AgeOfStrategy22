using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HealthValue : MonoBehaviour
{
    // Start is called before the first frame update
    public string healthValue;
    [SerializeField] private TextMeshProUGUI healthValueText;
    public string totalHealthValue;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowHealthValue(string healthValue)
    {
        var color = healthValueText.color;
        this.healthValue = healthValue;
        healthValueText.text = healthValue + " / " + totalHealthValue;
        healthValueText.color = new Color(color.r, color.g, color.b, 1f);
    }

    public void HideHealthValue()
    {
        var color = healthValueText.color;
        healthValueText.color = new Color(color.r, color.g, color.b, 0f);
    }
    public void SetInitialHealthValue(int healthTotalValue)
    {
        totalHealthValue = healthTotalValue.ToString();
    }
}
