using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int incomeAmount;
    [SerializeField]private int rateOfIncome;
    [SerializeField] private int timerLimit;
    private int currentTime;
    private int playerCurrency;

    private bool gameOver;

    public int CurrentTime => currentTime;

    public int TimerLimit => timerLimit;
    
    public int PlayerCurrency => playerCurrency;

    public static GameManager Instance;

    public delegate void UpdatePLayerUIAction();

    public static event UpdatePLayerUIAction OnPLayerUIAction;

    public delegate void GameOverAction(bool isWon);

    public static event GameOverAction OnGameOver;

    public delegate void UpdateTimerUI();

    public static event UpdateTimerUI OnUpdateTimer;
    


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
        if(Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }
        
        StartCoroutine(IncrementIncome());
        StartCoroutine(GameTimer());
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
        
        if(gameOver)
            return;
        
        OnGameOver?.Invoke(false);
         Time.timeScale = 0f;
        StopAllCoroutines();
        gameOver = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    private IEnumerator GameTimer()
    {
        while (true)
        {
            currentTime++;
            OnUpdateTimer?.Invoke();
            if(currentTime >= timerLimit)
            {
                OnGameOver?.Invoke(true);
                gameOver = true;
                break;
            }

            yield return new WaitForSeconds(1);
        }
    }
    
    
}
