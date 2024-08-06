using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private Animator _animator;
    public PlayerHealth PlayerHealth;

    private float _offset;
    private bool _hasPlayedSound;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _offset = (transform.position - PlayerHealth.transform.position).y;
    }

    private void Update()
    {
        FollowPlayerY();
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName("Attack")) return;
        if (stateInfo.normalizedTime < 0.6f) return;

        if (!_hasPlayedSound)
        {
            _hasPlayedSound = true;
            AudioManager.Instance.PlaySFX("Rock Smash");
        }

        if (stateInfo.normalizedTime < 0.9f) return;
        if (PlayerHealth == null) return;

        PlayerHealth.Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHitbox>() != null)
        {
            GameManager.LockedControls = true;

            collision.GetComponentInParent<PlayerController>().Rb.constraints = RigidbodyConstraints2D.FreezePosition;

            PlayerHealth playerHealth = collision.GetComponentInParent<PlayerHealth>();

            PlayerHealth = playerHealth;

            _animator.SetTrigger("TrAttack");
        }
    }

    public void FollowPlayerY()
    {
        transform.position = new Vector2(transform.position.x, PlayerHealth.transform.position.y + _offset);
    }
}
