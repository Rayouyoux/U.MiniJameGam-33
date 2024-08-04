using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    #region FIELDS

    [Header("Movement")]
    [SerializeField] private float _speed = 2f;
    private bool _isFacingRight;

    [Header("Health & Damage")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _attackDamage = 10f;
    [Range(0, 1)][SerializeField] private float _damageReduction = 0;
    private float _currentHealth;
    private bool _isAlive = true;

    [Header("Invulnearbility")]
    [SerializeField] private PlayerHitbox _playerHitbox;
    [SerializeField] private float _invulnerabilityDuration = 2f;
    private bool _isInvulnerable = false;
    private float _invulnerabilityTimer;

    [Header("Display")]
    private DamageFlash _damageFlashScript;
    private Animator _animator;

    #endregion

    #region PROPERTIES

    // Movement
    public float Speed { get => _speed; set => _speed = value; }
    public bool IsFacingRight { get => _isFacingRight; set => _isFacingRight = value; }

    // Health & Damage
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float AttackDamage { get => _attackDamage; set => _attackDamage = value; }
    public float DamageReduction { get => _damageReduction; set => _damageReduction = value; }
    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    public bool IsAlive { get => _isAlive; set => _isAlive = value; }

    // Invulnerability
    public PlayerHitbox PlayerHitbox { get => _playerHitbox; set => _playerHitbox = value; }
    public float InvulnerabilityDuration { get => _invulnerabilityDuration; set => _invulnerabilityDuration = value; }
    public bool IsInvulnerable { get => _isInvulnerable; set => _isInvulnerable = value; }
    public float InvulnerabilityTimer { get => _invulnerabilityTimer; set => _invulnerabilityTimer = value; }

    // Display
    public DamageFlash DamageFlashScript { get => _damageFlashScript; set => _damageFlashScript = value; }
    public Animator Animator { get => _animator; set => _animator = value; }

    #endregion

    #region MAIN METHODS

    // Initialization and update logic
    void Start()
    {
        DamageFlashScript = GetComponent<DamageFlash>();
        CurrentHealth = MaxHealth;
        InvulnerabilityTimer = 0;
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.IsPaused || !GameManager.HasStarted) return;

        // Handle Death
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Dying"))
        {
            if (stateInfo.normalizedTime >= 0.8f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Move();
        }

        // Handle invulnerability timer
        if (InvulnerabilityTimer > 0)
        {
            InvulnerabilityTimer -= Time.deltaTime;

            if (InvulnerabilityTimer <= 0)
            {
                InvulnerabilityTimer = 0;
                IsInvulnerable = false;
            }
        }
    }

    #endregion

    #region MOVEMENT METHODS

    public void Move()
    {
        
    }

    #endregion

    #region COLLISION METHODS

    // Handling collisions with the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != PlayerHitbox.gameObject) return;

        PlayerHealth playerHealth = PlayerHitbox.gameObject.GetComponentInParent<PlayerHealth>();

        playerHealth.ActiveCollisions += 1;
        playerHealth.CurrentCollisions.Add(AttackDamage);

        playerHealth.TakeDamage(AttackDamage);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != PlayerHitbox.gameObject) return;

        PlayerHealth playerHealth = PlayerHitbox.gameObject.GetComponentInParent<PlayerHealth>();

        playerHealth.ActiveCollisions -= 1;
        playerHealth.CurrentCollisions.Remove(AttackDamage);
    }

    #endregion

    #region IDAMAGEABLE

    // Methods related to taking damage and dying
    public void TakeDamage(float amount)
    {
        if (IsInvulnerable) return;

        AudioManager.Instance.PlaySFX("Enemy Hit");

        InvulnerabilityTimer = InvulnerabilityDuration;
        IsInvulnerable = true;

        float damageDealt = amount - amount * DamageReduction;
        CurrentHealth -= damageDealt;

        DamageFlashScript.CallDamageFlash();

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
    }

    public void Die()
    {
        Collider2D collider = GetComponent<Collider2D>();

        collider.enabled = false;
            
        Animator.SetTrigger("TrDying");
    }

    #endregion
}
