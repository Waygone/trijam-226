using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{

    Collider2D coll;
    SpriteRenderer spriteRenderer;
    AudioSource coinCollected;
    
    public float coinsAdded = 1f;
    public int respawnTime = 10;

    float timer = 0f;
    bool collected = false;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coinCollected = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (collected)
        {
            timer += Time.deltaTime;
            if(timer > respawnTime)
            {
                timer = 0f;
                collected = false;
                coll.enabled = true;
                spriteRenderer.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<PlayerController>().AddCoins(coinsAdded);
            
            collected = true;
            coll.enabled = false;
            spriteRenderer.enabled = false;

            coinCollected.pitch = Random.Range(0.7f, 1.3f);
            coinCollected.Play();
        }
    }
}
