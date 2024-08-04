using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _jumpPower = 16f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;

    private float _horizontal;
    private bool _isFacingRight = true;
    private Rigidbody2D _rb;
    private Animator _animator;

    #endregion

    #region PROPERTIES

    public float Speed { get => _speed; set => _speed = value; }
    public float JumpPower { get => _jumpPower; set => _jumpPower = value; }
    public Transform GroundCheck { get => _groundCheck; set => _groundCheck = value; }
    public LayerMask GroundLayer { get => _groundLayer; set => _groundLayer = value; }
    public float Horizontal { get => _horizontal; set => _horizontal = value; }
    public bool IsFacingRight { get => _isFacingRight; set => _isFacingRight = value; }
    public Rigidbody2D Rb { get => _rb; set => _rb = value; }
    public Animator Animator { get => _animator; set => _animator = value; }

    #endregion

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (GameManager.IsPaused) return;

        HandleAnimatorMovement();

        if (!GameManager.LockedControls)
            _horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded() && !GameManager.LockedControls)
        {
            AudioManager.Instance.PlaySFX("Jump");
            Rb.velocity = new Vector2(Rb.velocity.x, JumpPower);
        }

        if (Input.GetButtonUp("Jump") && Rb.velocity.y > 0f && !GameManager.LockedControls)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, Rb.velocity.y * 0.5f);
        }

        Flip();
        
    }

    private void FixedUpdate()
    {
        Rb.velocity = new Vector2(Horizontal * Speed, Rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
    }

    private void Flip()
    {
        if (IsFacingRight && Horizontal < 0f || !IsFacingRight && Horizontal > 0f)
        {
            IsFacingRight = !IsFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void HandleAnimatorMovement()
    {
        if (!IsGrounded())
        {
            Animator.SetBool("IsGrounded", false);
        }
        else
        {
            Animator.SetBool("IsGrounded", true);
            if (Rb.velocity.x > 0.05)
            {
                Animator.SetBool("IsMoving", true);
                Animator.SetBool("IsIdle", false);
            }
            else
            {
                Animator.SetBool("IsMoving", false);
                Animator.SetBool("IsIdle", true);
            }
        }
    }
}
