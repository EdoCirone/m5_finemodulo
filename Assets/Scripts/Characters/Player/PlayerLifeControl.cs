using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeControl : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;

    private int currentHealth;


    private Collider _collider;
    private bool isDead => currentHealth <= 0;
    private void Start()
    {
        _collider = GetComponent<Collider>();
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentHealth--;

        if (isDead)
        {
            SceneManager.LoadScene("MainMenu");
        }

        else
        {
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.name);
        }
    }
}
