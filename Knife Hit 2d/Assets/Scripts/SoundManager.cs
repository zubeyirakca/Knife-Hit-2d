using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;


    [SerializeField] private AudioClip buttonClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip appleHitClip;
    [SerializeField] private AudioClip appleRewardClip;
    [SerializeField] private AudioClip wheelClip;
    [SerializeField] private AudioClip knifeClip;
    [SerializeField] private AudioClip knifeFireClip;
    [SerializeField] private AudioClip unlockedClip;
    [SerializeField] private AudioClip fightStartClip;
    [SerializeField] private AudioClip fightEndClip;
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
        DontDestroyOnLoad(gameObject);
    }

    private void PlaySound(AudioClip clip, float vol = 1)
    {
        if (GameManager.Instance.SoundSettings)
        {
            if (Camera.main !=null)
            {
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, vol);
            }
        }
    }

    public void PlayButton()
    {
        PlaySound(buttonClip);
    }

    public void PlayAppleHit()
    {
        PlaySound(appleHitClip);
    }

    public void PlayWheelHit()
    {
        PlaySound(wheelClip);
    }

    public void PlayKnifeHit()
    {
        PlaySound(knifeClip);
    }

    public void PlayKnifeFireHit()
    {
        PlaySound(knifeFireClip);
    }

    public void PlayUnlock()
    {
        PlaySound(unlockedClip);
    }

    public void PlayBossStartFight()
    {
        PlaySound(fightStartClip);
    }

    public void PlayBossEndFight()
    {
        PlaySound(fightEndClip);
    }

    public void PlayGameOver()
    {
        PlaySound(gameOverClip);
    }

    public void PlayAppleReward()
    {
        PlaySound(appleRewardClip);
    }

    public void Vibrate()
    {
        if (GameManager.Instance.VibrationSettings)
        {
            Handheld.Vibrate();
        }
    }
}
