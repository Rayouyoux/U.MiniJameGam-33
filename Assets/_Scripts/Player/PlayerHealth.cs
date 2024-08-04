using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    #region FIELDS

    // Health
    [SerializeField] private float _maxHealth = 100f;
    [Range(0, 1)] [SerializeField] private float _damageReduction = 0;
    private float _currentHealth;
    private bool _isAlive = true;

    // Invulnerability
    [SerializeField] private float _invulnerabilityDuration = 2f;
    private bool _isInvulnerable = false;
    private int _activeCollisions = 0;
    private List<float> _currentCollisions = new List<float>();
    private float _invulnerabilityTimer;

    // Other
    [SerializeField] private CameraMovement _mainCamera;
    private DamageFlash _damageFlashScript;
    private Animator _animator;

    #endregion

    #region PROPERTIES

    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float DamageReduction { get => _damageReduction; set => _damageReduction = value; }
    public float InvulnerabilityDuration { get => _invulnerabilityDuration; set => _invulnerabilityDuration = value; }
    public bool IsAlive { get => _isAlive; set => _isAlive = value; }
    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    public bool IsInvulnerable { get => _isInvulnerable; set => _isInvulnerable = value; }
    public int ActiveCollisions { get => _activeCollisions; set => _activeCollisions = value; }
    public List<float> CurrentCollisions { get => _currentCollisions; set => _currentCollisions = value; }
    public float InvulnerabilityTimer { get => _invulnerabilityTimer; set => _invulnerabilityTimer = value; }
    public CameraMovement MainCamera { get => _mainCamera; set => _mainCamera = value; }
    public DamageFlash DamageFlashScript { get => _damageFlashScript; set => _damageFlashScript = value; }
    public Animator Animator { get => _animator; set => _animator = value; }

    #endregion

    #region MAIN METHODS

    private void Start()
    {
        CurrentHealth = MaxHealth;
        DamageFlashScript = GetComponentInChildren<DamageFlash>();
        Animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (GameManager.IsPaused) return;

        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Die"))
        {
            if (stateInfo.normalizedTime >= 0.9f)
            {
                IsAlive = false;
            }
        }

        if (InvulnerabilityTimer > 0)
        {
            InvulnerabilityTimer -= Time.deltaTime;

            if (InvulnerabilityTimer <= 0)
            {
                InvulnerabilityTimer = 0;
                IsInvulnerable = false;
            }
        }

        if (!IsInvulnerable && ActiveCollisions > 0)
        {
            TakeDamage(CurrentCollisions[0]);
        }
    }

    #endregion

    #region DAMAGE METHODS

    public void TakeDamage(float amount)
    {
        if (IsInvulnerable) return;

        AudioManager.Instance.PlaySFX("Hit");

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
        GetComponentInChildren<PlayerHitbox>().gameObject.GetComponent<Collider2D>().enabled = false;

        MainCamera.MoveCamera = false;
        GameManager.LockedControls = true;
        GetComponent<PlayerController>().Rb.constraints = RigidbodyConstraints2D.FreezePosition;

        Animator.SetTrigger("TrDying");
    }

    public void ToggleAlive()
    {
        IsAlive = false;
    }

    #endregion
}
