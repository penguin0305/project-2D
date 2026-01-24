using System.Collections;
using UnityEngine;

public class PlayerHitFeedback : MonoBehaviour
{
    [Header("Blink")]
    [SerializeField] private float blinkDuration = 0.2f;
    [SerializeField] private float blinkInterval = 0.05f;

    [Header("Knockback")]
    [SerializeField] private float knockbackX = 4f;
    [SerializeField] private float knockbackY = 2f;

    private SpriteRenderer sr;
    private PlayerAnimator anim;
    private Rigidbody2D rb;
    private PlayerMovement movement;

    private Coroutine blinkRoutine;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<PlayerAnimator>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    public void PlayHitFeedback()
    {
        DoKnockbackOppositeFacing();
        DoBlink();
    }

    private void DoKnockbackOppositeFacing()
    {
        if (rb == null || anim == null) 
            return;

        movement.DisableMovement(0.12f);
        float dir = anim.isFacingRight ? -1f : 1f;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        Vector2 force = new Vector2(dir * knockbackX, knockbackY);
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void DoBlink()
    {
        if (sr == null) return;

        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        float t = 0f;
        bool visible = true;

        while (t < blinkDuration)
        {
            visible = !visible;
            sr.enabled = visible;

            yield return new WaitForSecondsRealtime(blinkInterval);
            t += blinkInterval;
        }

        sr.enabled = true;
        blinkRoutine = null;
    }
}
