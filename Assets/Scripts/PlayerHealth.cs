using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerTookAHit;
    
    [SerializeField] private int maxHits = 2;
    public int MaxHits => maxHits;

    public void TakeAHit()
    {
        OnPlayerTookAHit?.Invoke();
    }
}