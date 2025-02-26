using System;
using UnityEngine;

public class SplashEffectSpawner : MonoBehaviour
{
    [SerializeField] GameObject _splashEffect;
    
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(_splashEffect, other.transform.position, Quaternion.identity);
    }
}
