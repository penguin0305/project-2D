using UnityEngine;
using System.Collections;
public class fallingTrapController : MonoBehaviour
{
    [Header("��鸲")]
    public float shakeTime = 1f;
    public float shakePower = 0.05f;

    [Header("����")]
    public float gravity = 5f;
    public int damage = 10;

    private Rigidbody2D rb;
    private bool activated = false;
    private Vector3 originalPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        originalPos = transform.position;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Activate()
    {
        if (activated) return;

        activated = true;
        StartCoroutine(ShakeAndDrop());
    }

    IEnumerator ShakeAndDrop()
    {
        float timer = 0f;

        while (timer < shakeTime)
        {
            float offsetX = Random.Range(-shakePower, shakePower);
            transform.position = originalPos + new Vector3(offsetX, 0, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
        rb.gravityScale = gravity;
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats ps = collision.GetComponent<PlayerStats>();
            if (ps != null)
            {
                ps.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Player p = collision.collider.GetComponent<Player>();
            if (p != null)
            {
                p.ApplyDamage(damage, 0.2f, true);
                Destroy(gameObject);
            }
        }
    }
}
