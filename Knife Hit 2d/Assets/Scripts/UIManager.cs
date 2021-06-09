﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header (header: "UI Settings")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text stageText;
    [SerializeField] private GameObject stageContainer;
    [SerializeField] private Color stageCompletedColor;
    [SerializeField] private Color stageNormalColor;
    public List<Image> stageIcons;

    [Header(header: "UI BOSS")]
    [SerializeField] private GameObject bossFight;
    [SerializeField] private GameObject bossDefeated;

    [Header(header: "GameOver UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text gameOverScore;
    [SerializeField] private Text gameOverStage;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }

    }

    private void Update()
    {
        scoreText.text = GameManager.Instance.Score.ToString();
        gameOverScore.text = GameManager.Instance.Score.ToString();

        stageText.text = "Stage" + GameManager.Instance.Stage;
        gameOverStage.text = "Stage" + GameManager.Instance.Stage;
    }

    public IEnumerator BossStart()
    {
        bossFight.SetActive(true);
        yield return new WaitForSeconds(seconds: 1f);
        bossFight.SetActive(false);
    }
    
    public IEnumerator BossDefeated()
    {
        bossDefeated.SetActive(true);
        yield return new WaitForSeconds(seconds: 1f);
        bossDefeated.SetActive(false);
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        stageContainer.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }
    public void OpenFacebook()
    {
        Application.OpenURL("");
    }

    public void OpenShop()
    {
        GeneralUI.Instance.OpenShop();
    }

    public void OpenOptions()
    {
        GeneralUI.Instance.OpenSettings();
    }

    private void UpdateUI()
    {
        if (GameManager.Instance.Stage % 5 == 0)
        {
            foreach(var icon in stageIcons)
            {
                icon.gameObject.SetActive(false);
                stageIcons[stageIcons.Count - 1].color = stageNormalColor;
                stageText.text = "Boss " + LevelManager.Instance.BossName;
            }
        }

        else
        {
            for (int i = 0; i < stageIcons.Count; i++)
            {
                stageIcons[i].gameObject.SetActive(true);
                stageIcons[i].color = GameManager.Instance.Stage % 5 <= i ? stageNormalColor : stageCompletedColor;
            }
        }
    }
}
