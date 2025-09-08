using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoneyTextBox;
    [SerializeField] private CanvasGroup WinScreen;
    [SerializeField] private CanvasGroup LooseScreen;
    [SerializeField] private CanvasGroup MainPLayerUI;


    private void OnEnable()
    {
        GameManager.OnPLayerUIAction += UpdateMoneyText;
        GameManager.OnGameOver += ShowGameOverScreen;
    }

    private void OnDisable()
    {
        GameManager.OnPLayerUIAction -= UpdateMoneyText;
        GameManager.OnGameOver -= ShowGameOverScreen;
    }


    private void UpdateMoneyText()
    {
        MoneyTextBox.text = GameManager.Instance.PlayerCurrency.ToString();
    }

    private void ShowGameOverScreen(bool isWon)
    {

        MainPLayerUI.alpha = 0;
        MainPLayerUI.interactable = false;
        MainPLayerUI.blocksRaycasts = false;
        
        if (isWon)
        {
            WinScreen.alpha = 1;
            WinScreen.interactable = true;
            WinScreen.blocksRaycasts = true;
        }
        else
        {
            LooseScreen.alpha =1;
            LooseScreen.interactable = true;
            LooseScreen.blocksRaycasts = true;
        }
        
    }


    public void Quit()
    {
        Application.Quit();
    }
}
