using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private const string IS_GROUNDED = "isGrounded";
    private const string IS_SLIDING = "isSliding";
    public static string HIGH_SCORE = "highScore";
    
    public static readonly int IsGrounded = Animator.StringToHash(IS_GROUNDED);
    public static readonly int IsSliding = Animator.StringToHash(IS_SLIDING);
}
