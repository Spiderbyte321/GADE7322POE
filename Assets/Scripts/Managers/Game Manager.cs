using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int incomeAmount;
    [SerializeField]private int rateOfIncome;
    private int playerCurrency;
    
    public int PlayerCurrency => playerCurrency;

    public static GameManager Instance;

    public delegate void UpdatePLayerUIAction();

    public static event UpdatePLayerUIAction OnPLayerUIAction;

    public delegate void GameOverAction(bool isWon);

    public static event GameOverAction OnGameOver;


    private void OnEnable()
    {
        TowerBase.OnTowerDied += GameOver;
    }

    private void OnDisable()
    {
        TowerBase.OnTowerDied -= GameOver;
    }


    private void Start()
    {

        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        StartCoroutine(IncrementIncome());
    }



    private IEnumerator IncrementIncome()
    {
        while(true)
        {
            playerCurrency += incomeAmount;
            OnPLayerUIAction?.Invoke();
            //float SecondsToWait;
            //SecondsToWait = MathUtilities.RoundToTwoDecimalPLaces(rateOfIncome / 60);
            yield return new WaitForSeconds(rateOfIncome);
        }
    }


    public void IncreaseIncomeRate(int AAmount)
    {
        rateOfIncome += AAmount;
    }

    public void InreaseIncomeAmount(int AAmount)
    {
        incomeAmount += AAmount;
    }
    


    private void GameOver(TowerBase ADeadTower)
    {
        if(ADeadTower is not PlayerNexus)
            return;
        
        
        
        OnGameOver?.Invoke(false);
        StopAllCoroutines();

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
    
    
}
