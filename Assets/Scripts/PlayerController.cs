using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    public PlayerNumber playerNumber;

    public float maxHealth;
    public float health;
    public static bool isGameOver = false;

    [SerializeField] private Slider healthSlider;

    [SerializeField] private GameObject playerOne;
    [SerializeField] private GameObject playerTwo;

    [SerializeField] private GameObject fireBallProjectile;
    [SerializeField] private GameObject witchProjectile;

    [SerializeField] private float lerpDuration;

    [SerializeField] private TextMeshProUGUI gameOver;

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
            isGameOver = true;
            Invoke(nameof(Death), 1f);
        }

        float endValue = health - damage;
        StartCoroutine(LerpSlider(healthSlider, endValue));
        health = endValue;
        StartCoroutine(BlinkObject(healthSlider.gameObject, 3, 0.15f));
    }

    private IEnumerator LerpSlider(Slider slider, float endValue)
    {
        // Record the time at which the lerp starts
        float timeStartedLerping = Time.time;
        float startValue = slider.value; // The current slider value

        while (Time.time < timeStartedLerping + lerpDuration)
        {
            // Calculate the percentage complete
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = timeSinceStarted / lerpDuration;

            // Lerp the value of the slider
            slider.value = Mathf.Lerp(startValue, endValue, percentageComplete);

            yield return null; // Wait until the next frame to continue execution
        }

        slider.value = endValue; // Ensure the final value is set
    }

    private IEnumerator BlinkObject(GameObject gameObject, int blinks, float interval)
    {
        for (int i = 0; i < blinks; i++)
        {
            gameObject.SetActive(false);
            yield return new WaitForSeconds(interval);
            gameObject.SetActive(true);
            yield return new WaitForSeconds(interval);
        }
    }

    private void Death()
    {
        if (playerNumber == PlayerNumber.PlayerOne)
        {
            gameOver.text = "Player Two wins!";
            gameOver.gameObject.SetActive(true);
        }
        else if (playerNumber == PlayerNumber.PlayerTwo)
        {
            gameOver.text = "Player One wins!";
            gameOver.gameObject.SetActive(true);
        }
    }
}
