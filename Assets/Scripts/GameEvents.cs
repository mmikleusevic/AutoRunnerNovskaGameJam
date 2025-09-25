using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private const string IS_GROUNDED = "isGrounded";
    private const string IS_SLIDING = "isSliding";
    private const string CAUGHT = "Caught";
    private const string PULL = "Pull";
    public static string HIGH_SCORE = "HighScore";
    public static string WIN = "Win";
    
    public static readonly int IsGrounded = Animator.StringToHash(IS_GROUNDED);
    public static readonly int IsSliding = Animator.StringToHash(IS_SLIDING);
    public static readonly int Caught = Animator.StringToHash(CAUGHT);
    public static readonly int Pull = Animator.StringToHash(PULL);
}
