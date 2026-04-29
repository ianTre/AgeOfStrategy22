using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;



public class DeployCardController : MonoBehaviour
{
    [SerializeField] private RectTransform image;
    [SerializeField] private RectTransform numberPanel;
    public UnitData unitsInitialData;
    private int unitsLeftToBeCreated = 0;
    GameManager gameManager;
    bool cardIsEnable;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color RegularColor;
    private Color disableColor;
    private Color disableColorImage;

    public void Start()
    {
        this.GetComponent<UnityEngine.UI.Image>().color = RegularColor;
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

        disableColor = Color.gray;
        disableColorImage = new Color(0.8018868f, 0.6619349f, 0.6619349f, 1);
        cardIsEnable = true;

    }

    public void ShowUnitData(UnitData data, int number)
    {
        unitsInitialData = data;
        unitsLeftToBeCreated = number;
        this.transform.gameObject.SetActive(true);
        UpdateText(number);
    }

    private void UpdateText(int number)
    {
        var numberText = numberPanel.transform.GetChild(0);
        var textComponent = numberText.GetComponent<TextMeshProUGUI>();
        image.GetComponent<UnityEngine.UI.Image>().sprite = unitsInitialData.photo;
        if (textComponent != null)
        {
            textComponent.text = number.ToString();
        }
        else
        {
            Debug.LogError("DeployCardController: TextMeshPro component not found in numberPanel.");
        }
    }

    public void DisableCard()
    {
        unitsLeftToBeCreated = 0;
        this.transform.GetComponent<UnityEngine.UI.Image>().color = disableColor;
        image.GetComponent<UnityEngine.UI.Image>().color = disableColorImage;
        UpdateText(0);
        cardIsEnable = false;
    }



    public void ClickOnCard()
    {
        if (!cardIsEnable)
            return;
        gameManager.StageUpdate_PlayerDeploy_CardSelected(this);
        SelectCard();
    }

    public void DeselectCard()
    {
        this.GetComponent<UnityEngine.UI.Image>().color = RegularColor;
    }

    public void SelectCard()
    {
        this.GetComponent<UnityEngine.UI.Image>().color = selectedColor;
    }


}
