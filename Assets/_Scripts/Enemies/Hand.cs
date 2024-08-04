using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private Animator _animator;
    private PlayerHealth _playerHealth;

    private bool _hasPlayedSound;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName("Attack")) return;
        if (stateInfo.normalizedTime < 0.6f) return;

        if (!_hasPlayedSound)
        {
            _hasPlayedSound = true;
            AudioManager.Instance.PlaySFX("Rock Smash");
        }

        if (stateInfo.normalizedTime < 0.9f) return;
        if (_playerHealth == null) return;

        _playerHealth.Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHitbox>() != null)
        {
            GameManager.LockedControls = true;

            collision.GetComponentInParent<PlayerController>().Rb.constraints = RigidbodyConstraints2D.FreezePosition;

            PlayerHealth playerHealth = collision.GetComponentInParent<PlayerHealth>();

            _playerHealth = playerHealth;

            _animator.SetTrigger("TrAttack");
        }
    }
}
