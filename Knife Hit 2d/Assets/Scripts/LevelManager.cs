using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Wheel[] wheels;
    public Boss[] bosses;

    [SerializeField] private GameObject knifePrefab;

    [Header(header: "Wheel settings")]
    [SerializeField] private Transform wheelSpawnPosition;
    [Range(0, 1)] [SerializeField] private float wheelScale;

    [Header(header: "Knife settings")]
    [SerializeField] private Transform knifeSpawnPosition;
    [Range(0, 1)] [SerializeField] private float knifeScale;

    private string bossName;
    private Wheel currentWheel;
    private Knife currentKnife;

    public int TotalSpawnKnife { get; set; }
    public string BossName => bossName;

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

    private void Start()
    {
        InitializedGame();
    }

    private void Update()
    {
        if (currentKnife==null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !currentKnife.IsReleased)
        {
            knifeCounter.Instance.KnifeHit(TotalSpawnKnife);
            currentKnife.FireKnife();
            StartCoroutine(routine: GenerateKnife());
        }
    }

    private void InitializedGame()
    {
        GameManager.Instance.IsGameOver = false;
        GameManager.Instance.Score = 0;
        GameManager.Instance.Stage = 1;

        SetupGame();
    }

    private void SetupGame()
    {
        SpawnWheel();
        knifeCounter.Instance.SetupKnife(currentWheel.AvailableKnifes);

        TotalSpawnKnife = 0;
        StartCoroutine(routine: GenerateKnife());
    }

    private void SpawnWheel()
    {
        GameObject tmpWheel = new GameObject();
        if (GameManager.Instance.Stage % 5 ==0 )
        {
            Boss newBoss = bosses[Random.Range(0, bosses.Length)];
            tmpWheel = Instantiate(newBoss.bossPrefab, wheelSpawnPosition.position, Quaternion.identity, wheelSpawnPosition).gameObject;
            bossName = "Boss" + newBoss.bossName;
        }

        else
        {
            tmpWheel = Instantiate(wheels[GameManager.Instance.Stage - 1], wheelSpawnPosition.position,
                Quaternion.identity, wheelSpawnPosition).gameObject;
        }

        float wheelScaleInScreen = GameManager.Instance.ScreenWidth * wheelScale / tmpWheel.GetComponent<SpriteRenderer>().bounds.size.x;
        tmpWheel.transform.localScale = Vector3.one * wheelScaleInScreen;
        currentWheel = tmpWheel.GetComponent<Wheel>();
    }

    private IEnumerator GenerateKnife()
    {
        yield return new WaitUntil(predicate: (() =>knifeSpawnPosition.childCount==0));

        if (currentWheel.AvailableKnifes>TotalSpawnKnife && !GameManager.Instance.IsGameOver)
        {
            TotalSpawnKnife ++;
            GameObject tmpKnife = new GameObject();

            if (GameManager.Instance.SelectedKnifePrefab==null)
            {
                tmpKnife = Instantiate(knifePrefab, knifeSpawnPosition.position, Quaternion.identity, knifeSpawnPosition).gameObject;
            }

            else
            {
                tmpKnife = Instantiate(GameManager.Instance.SelectedKnifePrefab, knifeSpawnPosition.position, Quaternion.identity, knifeSpawnPosition).gameObject;
            }

            float knifeScaleInScreen = GameManager.Instance.ScreenHeight * knifeScale / tmpKnife.GetComponent<SpriteRenderer>().bounds.size.x;
            tmpKnife.transform.localScale = Vector3.one * knifeScaleInScreen;
            currentKnife = tmpKnife.GetComponent<Knife>();
        }
    }

    public void NextLevel()
    {
        if (currentWheel !=null)
        {
            currentWheel.DestroyKnife();
        }

        if (GameManager.Instance.Stage %5==0)
        {
            GameManager.Instance.Stage++;
            StartCoroutine(routine: BossDefeated());
        }

        else
        {
            GameManager.Instance.Stage++;
            if (GameManager.Instance.Stage%5==0)
            {
                StartCoroutine(routine: BossFight());
            }

            else
            {
                Invoke(nameof(SetupGame), time:0.3f);
            }
        }
    }

    private IEnumerator BossFight()
    {
        StartCoroutine(routine: UIManager.Instance.BossStart());
        yield return new WaitForSeconds(seconds: 2f);
        SetupGame();
    }

    private IEnumerator BossDefeated()
    {
        StartCoroutine(routine: UIManager.Instance.BossDefeated());
        yield return new WaitForSeconds(seconds: 2f);
        SetupGame();
    }

    [Serializable]
    public class Boss
    {
        public GameObject bossPrefab;
        public string bossName;
    }
}
