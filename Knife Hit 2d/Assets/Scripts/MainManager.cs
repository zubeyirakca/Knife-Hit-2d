using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] private Image selectedKnife;
    private string gameURL = "";
    private string appID = "";

    public Image SelectedKnife => selectedKnife;

    public void Rate()
    {
        Application.OpenURL(gameURL + appID);
        SoundManager.Instance.PlayButton();
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
        SoundManager.Instance.PlayButton();
    }
}
