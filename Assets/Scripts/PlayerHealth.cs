using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerCaught;
    public static event Action OnPlayerTookAHit;
    
    [SerializeField] private int maxHits = 2;
    
    private int currentHits;

    private void Start()
    {
        currentHits = 0;
    }

    public void TakeAHit()
    {
        currentHits++;
        
        OnPlayerTookAHit?.Invoke();

        if (currentHits >= maxHits) OnPlayerCaught?.Invoke();
    }
}
