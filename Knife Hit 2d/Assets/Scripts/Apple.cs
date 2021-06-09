﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [SerializeField] private ParticleSystem appleParticle;

    private CircleCollider2D myCollider2D;
    private SpriteRenderer sp;
    private void Start()
    {
        myCollider2D = GetComponent<CircleCollider2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Knife"))
        {
            myCollider2D.enabled = false;
            sp.enabled = false;
            transform.parent = null;
            SoundManager.Instance.PlayAppleHit();

            appleParticle.Play();
            Destroy(gameObject, t:2f);
        }
    }

}
