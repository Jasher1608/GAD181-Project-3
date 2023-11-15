using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    public PlayerNumber playerNumber;

    public float maxHealth;
    public float health;
    public bool isDead = false;

    [SerializeField] private Slider healthSlider;

    [SerializeField] private GameObject playerOne;
    [SerializeField] private GameObject playerTwo;

    [SerializeField] private GameObject fireBallProjectile;
    [SerializeField] private GameObject witchProjectile;

    [SerializeField] private float lerpDuration;

    private void Start()
    {
        animator = GetComponent<Animator>();
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    public IEnumerator Attack()
    {
        animator.SetTrigger("Attack");

        if (playerNumber == PlayerNumber.PlayerOne)
        {
            GameObject inst = Instantiate(fireBallProjectile, this.transform.position, Quaternion.identity);
            inst.GetComponent<Projectile>().target = playerTwo;
        }
        else if (playerNumber == PlayerNumber.PlayerTwo)
        {
            GameObject inst = Instantiate(witchProjectile, this.transform.position, Quaternion.identity);
            inst.GetComponent<Projectile>().target = playerOne;
        }
        yield return null;
    }

    public void TakeDamage(float damage)
    {
        if ((health - damage) > 0)
        {
            animator.SetTrigger("Hurt");
        }
        else if ((health - damage) <= 0)
        {
            animator.SetBool("isDead", true);
            isDead = true;
        }

        float endValue = health - damage;
        StartCoroutine(LerpHealthValue(endValue));
    }

    IEnumerator LerpHealthValue(float endValue)
    {
        // Record the time at which the lerp starts
        float timeStartedLerping = Time.time;
        float startValue = healthSlider.value; // The current slider value

        while (Time.time < timeStartedLerping + lerpDuration)
        {
            // Calculate the percentage complete
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = timeSinceStarted / lerpDuration;

            // Lerp the value of the slider
            healthSlider.value = Mathf.Lerp(startValue, endValue, percentageComplete);

            yield return null; // Wait until the next frame to continue execution
        }

        healthSlider.value = endValue; // Ensure the final value is set
        health = endValue;
    }
}
