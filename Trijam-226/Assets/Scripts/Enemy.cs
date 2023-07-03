using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    AudioSource loseCoins;

    private void Awake()
    {
        loseCoins = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().AddCoins(-5);
            loseCoins.pitch = Random.Range(0.8f, 1.2f);
            loseCoins.Play();
        }
    }
}
