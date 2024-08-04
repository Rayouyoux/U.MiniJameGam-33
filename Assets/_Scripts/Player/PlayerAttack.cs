using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private float _attackDamage = 20f;
    [SerializeField] private Transform _attackHitbox;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private Collider2D _attackCollider;

    private Animator _animator;
    private float _hitBoxTimer;
    private float _hitDelayTimer;

    #endregion

    #region PROPERTIES

    public float AttackDamage { get => _attackDamage; set => _attackDamage = value; }
    public Transform AttackHitbox { get => _attackHitbox; set => _attackHitbox = value; }
    public float AttackCooldown { get => _attackCooldown; set => _attackCooldown = value; }
    public Collider2D AttackCollider { get => _attackCollider; set => _attackCollider = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public float HitBoxTimer { get => _hitBoxTimer; set => _hitBoxTimer = value; }
    public float HitDelayTimer { get => _hitDelayTimer; set => _hitDelayTimer = value; }

    #endregion

    #region MAIN METHODS

    void Start()
    {
        AttackHitbox.gameObject.SetActive(false);
        Animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (GameManager.IsPaused) return;

        if (_hitBoxTimer > 0)
        {
            _hitBoxTimer -= Time.deltaTime;

            if (_hitBoxTimer <= 0)
            {
                _hitBoxTimer = 0;
            }
        }

        if (HitDelayTimer > 0)
        {
            HitDelayTimer -= Time.deltaTime;

            if (HitDelayTimer <= 0)
            {
                HitDelayTimer = 0;

                AudioManager.Instance.PlaySFX("Sword Slash");
                HitBoxTimer = AttackCooldown;

                StartCoroutine(HitBoxRoutine());
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !GameManager.LockedControls)
            {
                Attack();
            }

        }

    }

    #endregion

    #region COLLISION METHODS


    #endregion

    #region ATTACK METHODS

    private void Attack()
    {
        if (HitBoxTimer > 0f) return;

        Animator.SetTrigger("TrAttack");

        HitDelayTimer = 0.2f;
    }

    private IEnumerator HitBoxRoutine()
    {
        float activeAmount = 0.05f;
        if (AttackCooldown < 0.05f)
            activeAmount = AttackCooldown;

        AttackHitbox.gameObject.SetActive(true);

        yield return new WaitForSeconds(activeAmount);

        AttackHitbox.gameObject.SetActive(false);
    }

    #endregion
}
