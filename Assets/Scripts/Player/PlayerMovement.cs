using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using System;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private int _speed;
    [SerializeField] private int _jumpForce;
    private Rigidbody2D _rb;
    private bool _isGrounded;
    private int _jumpCount;
    private bool _canDash = true;
    private int _dashingPower = 12;
    private float _dashingTime = 0.2f;
    private float _dashCooldown = 1f;
    private int xDisplacement;
    private Vector3 displacementVector;
    Camera cam;
    public TextMeshProUGUI _text;
    private int _hitPoints = 3;
    private Animator _animator;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Reset();
        Jump();
        Move();
        Sprint();
        Die();
        
        _text.text = _hitPoints.ToString();

        if (Input.GetKeyDown(KeyCode.LeftControl) && _canDash)
        {
            StartCoroutine(Dash());

        }

    }
    private void Reset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      
        }
    }

    void Move()
    {
        float xDisplacement = Input.GetAxis(Axis.HORIZONTAL);
  

        if(xDisplacement > 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    
        else if(xDisplacement < 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        _rb.velocity = new Vector2(xDisplacement * _speed, _rb.velocity.y);
        _animator.SetFloat(AnimatorValues.Speed, MathF.Abs(_rb.velocity.x));
    }
    
    private void Jump()
    {
        if (Input.GetButtonDown(Axis.JUMP) && (_isGrounded || _jumpCount < 2))
        {
            _animator.SetBool(AnimatorValues.isJumping, true);
            _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            _jumpCount++;
        }
    }
    public void Die()
    {
        if(_hitPoints < 1)
        {

            _animator.SetBool(AnimatorValues.isDead, true);
            _speed = 0;
        }
    }
    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed = _speed * 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _speed / 2;
        }
    }


    private IEnumerator Dash()
    {
        _canDash = false;
        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;
        _speed *= _dashingPower;
        yield return new WaitForSeconds(_dashingTime);
        _rb.gravityScale = originalGravity;
        _speed /= _dashingPower;
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.GROUND) || collision.gameObject.CompareTag(Tag.SUPRISE))
        {
            _isGrounded = true;
            _animator.SetBool(AnimatorValues.isJumping, false);
            _jumpCount = 0;
            if (collision.gameObject.CompareTag(Tag.SUPRISE))
            {
                
                cam.orthographicSize *= -1.0f;
            }
        }

        if (collision.gameObject.CompareTag(Tag.ENEMY))
        {
            _hitPoints = _hitPoints - 1;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.GROUND))
        {
            _isGrounded = false;

        }
        if (collision.gameObject.CompareTag(Tag.SUPRISE))
        {
            cam.orthographicSize *= -1.0f;
        }
    }

}
