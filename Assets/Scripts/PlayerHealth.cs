using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;
    public int recoveryFactor = 20;

    public Image bloodImage;
    private Color alphaAmount;

    public float recoveryRate = 5f;
    private float recoveryTimer;

    public bool isDead;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        recoveryTimer += Time.deltaTime;
        Debug.Log(recoveryTimer);

        if (recoveryTimer > recoveryRate)
        {
            StartCoroutine(RecoveryHealth());
        }
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;

        alphaAmount = bloodImage.color;
        alphaAmount.a += ((float) damage / 100);

        bloodImage.color = alphaAmount;

        if (health <= 0)
        {
            GameController.gameController.ShowGameOver();
            isDead = true;
            Debug.Log("Game Over");
        }

        recoveryTimer = 0f;
    }

    IEnumerator RecoveryHealth()
    {
        while (health < maxHealth)
        {
            health += recoveryFactor;

            alphaAmount.a -= ((float)recoveryFactor / 100);
            bloodImage.color = alphaAmount;
            yield return new WaitForSeconds(2f);
        }
    }
}
