using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    [SerializeField] private GameObject vfx;
    [SerializeField] private AudioClip soundEffect;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Instantiate(vfx, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(soundEffect, transform.position);
            Destroy(gameObject);
        }
    }
}
