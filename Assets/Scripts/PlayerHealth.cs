using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerTookAHit;
    
    [SerializeField] private int maxHits = 2;
    public int MaxHits => maxHits;
    
    private int currentHits;

    private void Start()
    {
        currentHits = 0;
    }

    public void TakeAHit()
    {
        currentHits++;
        
        OnPlayerTookAHit?.Invoke();
    }
    
}
