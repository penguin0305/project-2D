using System.Collections;
using UnityEngine;

public class EnemyHitDummy : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void OnHit()
    {
        StartCoroutine(HitFlash());
    }

    private IEnumerator HitFlash()
    {
        sr.color = Color.red;      // 맞으면 빨간색
        yield return new WaitForSeconds(0.2f);
        sr.color = originalColor;  // 0.2초 후 원래 색 복귀
    }
}