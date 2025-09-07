using System;
using System.Collections;
using UnityEngine;

public class PlayerLifeControl : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private float _invulnerabilityTime = 0.5f;

    private static int s_currentHealth = -1;
    private static int s_maxHealth = -1;

    private bool _invulnerable;


    public event Action<int, int> Damaged;
    public event Action Died;

    public int MaxHealth => _maxHealth;                 
   
    public static int StaticCurrentHealth => s_currentHealth;

    private void Start()
    {

        if (s_currentHealth < 0) s_currentHealth = _maxHealth;
        if (s_maxHealth < 0) s_maxHealth = _maxHealth;

        SceneFlow.Instance?.RegisterPlayer(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            TakeDamage(1);
    }

    private void TakeDamage(int amount)
    {
        if (_invulnerable) return;

        s_currentHealth = Mathf.Max(0, s_currentHealth - amount);
        Debug.Log("La mia vita è " + s_currentHealth);

        if (s_currentHealth <= 0) Died?.Invoke();
        else { Damaged?.Invoke(s_currentHealth, s_maxHealth); StartCoroutine(InvulnerabilityWindow()); }
    }

    private IEnumerator InvulnerabilityWindow()
    {
        _invulnerable = true;
        yield return new WaitForSeconds(_invulnerabilityTime);
        _invulnerable = false;
    }

    // Reset comodo per SceneFlow
    public static void ResetHealthToMax() => s_currentHealth = (s_maxHealth > 0) ? s_maxHealth : 1;
}