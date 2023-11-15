using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject target;

    public float damage;

    [SerializeField] private float speed;

    private new Collider2D collider2D;
    private PlayerController playerController;

    private void Start()
    {
        collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;
        StartCoroutine(EnableColliderAfterDelay(collider2D, 0.5f));
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController = collision.GetComponent<PlayerController>();
            playerController.TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }

    private IEnumerator EnableColliderAfterDelay(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }
}
