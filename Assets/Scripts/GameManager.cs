using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text LevelText;
    [Header("UI")]
    public GameObject GameOverUI;
    public GameObject GameWinUI;
    public GameObject Tutorial;

    [Header("Level")]
    public GameObject[] Levels;
    // Start is called before the first frame update
    int Level;

    bool isGameEnd;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    void Start()
    {
        isGameEnd = false;
        GameOverUI.SetActive(false);
        GameWinUI.SetActive(false);
         Level = PlayerPrefs.GetInt("Level", 0);
        LevelText.text = "Level " + (Level+1);
        Instantiate(Levels[Level]);
        if (Level == 0)
        {
            Tutorial.SetActive(true);
            StartCoroutine(EndTutorial());
        }

    }


    public void LoadMenu() {

        SceneManager.LoadScene("Menu");
    }

    public void RetryLevel() {

        SceneManager.LoadScene("Game");
    }

    public void GameOver() {
        isGameEnd = true;
        GameOverUI.SetActive(true);
    }

    public void LevelCompleted() {
        if (!isGameEnd)
        {
            Adcontrol.instance.ShowInterstitial();
            Level++;
            PlayerPrefs.SetInt("Level", Level);
            GameWinUI.SetActive(true);
        }
    }

    IEnumerator EndTutorial() {
        yield return new WaitForSeconds(4.0f);
        Tutorial.SetActive(false);
    }


}
