using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class DeployCardController : MonoBehaviour
{
    [SerializeField] private RectTransform image;
    [SerializeField] private RectTransform numberPanel;
    private UnitData unitData;
    GameManager gameManager;

    public void Start()
    {
        if (image == null)
        {
            Debug.LogError("DeployCardController: image is not assigned.");
        }

        if (numberPanel == null)
        {
            Debug.LogError("DeployCardController: numberPanel is not assigned.");
        }

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("DeployCardController: GameManager not found in the scene.");
        }

    }

    public void ShowUnitData(UnitData data, int number)
    {
        this.transform.gameObject.SetActive(true);
        var numberText = numberPanel.transform.GetChild(0);
        var textComponent = numberText.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = number.ToString();
        }
        else
        {
            Debug.LogError("DeployCardController: TextMeshPro component not found in numberPanel.");
        }
    }

    public void ClickOnCard()
    {
        gameManager.ClickOnCardDeploy(unitData);
    }


}
