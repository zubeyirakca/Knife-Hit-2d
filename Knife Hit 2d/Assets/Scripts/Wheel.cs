using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private int availableKnifes;

    [SerializeField] private Sprite firstWheel;
    [SerializeField] private Sprite secondWheel;
    [SerializeField] private Sprite thirdWheel;

    [SerializeField] private bool isBoss;

    [Header(header: "Prefabs")]
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private GameObject knifePrefab;

    [Header(header: "Settings")]
    [SerializeField] private float rotationZ;

    public List<Level> levels;
    [HideInInspector]
    public List<Knife> Knifes;

    private int levelIndex;

    public int AvailableKnifes => availableKnifes;

    private void Start()
    {
        if (!isBoss)
        {
            if (GameManager.Instance.Stage<5)
            {
                GetComponent<SpriteRenderer>().sprite = firstWheel;
            }
            else if (GameManager.Instance.Stage> 5 && GameManager.Instance.Stage<10)
            {
                GetComponent<SpriteRenderer>().sprite = secondWheel;
            }

            else if (GameManager.Instance.Stage>10)
            {
                GetComponent<SpriteRenderer>().sprite = thirdWheel;
            }
        }


        levelIndex = UnityEngine.Random.Range(0, levels.Count);

        if (levels[levelIndex].appleChance > UnityEngine.Random.value)
        {
            SpawnApple();
        }

        SpawnKnifes();
    }

    private void Update()
    {
        RotateWheel();
    }
    private void RotateWheel()
    {
        transform.Rotate(xAngle: 0f, yAngle: 0f, zAngle: rotationZ * Time.deltaTime);
    }

    private void SpawnKnifes()
    {
        foreach(float knifeAngle in levels[levelIndex].knifeAngleFromWheel)
        {
            GameObject knifeTmp = Instantiate(knifePrefab);
            knifeTmp.transform.SetParent(transform);

            SetRotationFromWheel(wheel: transform, objectToPlace: knifeTmp.transform, knifeAngle, spaceFromObject: 0.20f, objectRotation: 180f);
            knifeTmp.transform.localScale = Vector3.one;
        }
    }

    private void SpawnApple()
    {
        foreach(float appleAngle in levels[levelIndex].appleAngleFromWheel)
        {
            GameObject appleTmp = Instantiate(applePrefab);
            appleTmp.transform.SetParent(transform);

            SetRotationFromWheel(wheel: transform, objectToPlace: appleTmp.transform, appleAngle, spaceFromObject: 0.25f, objectRotation: 0f);
            appleTmp.transform.localScale = Vector3.one;
        }
    }

    public void SetRotationFromWheel(Transform wheel, Transform objectToPlace, float angle, float spaceFromObject, float objectRotation)
    {
        Vector2 offSet = new Vector2(x: Mathf.Sin(f: angle * Mathf.Deg2Rad), y: Mathf.Cos(f: angle * Mathf.Deg2Rad)) * (wheel.GetComponent<CircleCollider2D>().radius + spaceFromObject);
        objectToPlace.localPosition = (Vector2)wheel.localPosition + offSet;
        objectToPlace.localRotation = Quaternion.Euler(x: 0, y: 0, z: -angle + objectRotation);
    }

    public void KnifeHit(Knife knife)
    {
        knife.myRigidbody2D.isKinematic = true;
        knife.myRigidbody2D.velocity = Vector2.zero;
        knife.transform.SetParent(transform);
        knife.Hit = true;

        Knifes.Add(knife);
        if (Knifes.Count >= AvailableKnifes)
        {
            LevelManager.Instance.NextLevel();
        }

        GameManager.Instance.Score++;
    }

    public void DestroyKnife()
    {
        foreach(var knife in Knifes)
        {
            Destroy(knife.gameObject);
        }
        Destroy(gameObject);
    }

    [Serializable]
    public class Level
    {
        [Range(0, 1)] [SerializeField] public float appleChance;

        public List<float> appleAngleFromWheel = new List<float>();
        public List<float> knifeAngleFromWheel = new List<float>();
    }

}
