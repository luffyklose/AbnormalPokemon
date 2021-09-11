using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed;

    private bool b_isMoving;

    public Rigidbody2D m_rigidbody;
    public Animator m_animator;
    
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        //实现只允许横向或纵向移动的废弃代码
        /*if (b_isMoving)
        {
            if (Mathf.Abs(m_rigidbody.velocity.x) > 0 && Input.GetAxisRaw("Vertical") != 0)
            {
                m_rigidbody.velocity = new Vector2(0.0f, Input.GetAxisRaw("Vertical"));
                //m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_moveSpeed;
            }
            else if (Mathf.Abs(m_rigidbody.velocity.y) > 0 && Input.GetAxisRaw("Horizontal") != 0)
            {
                m_rigidbody.velocity = new Vector2( Input.GetAxisRaw("Horizontal"),0.0f);
                //m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_moveSpeed;
            }
            else
            {
                m_rigidbody.velocity=Vector2.zero;
            }
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                m_rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), 0.0f);
            }
            else if (Input.GetAxisRaw("Vertical") != 0)
            {
                m_rigidbody.velocity = new Vector2(0.0f, Input.GetAxisRaw("Vertical"));
            }
            //m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_moveSpeed;
        }*/
        
        //Basic Movement Scripts
        m_rigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_moveSpeed;
        if (m_rigidbody.velocity.magnitude > Double.Epsilon)
        {
            b_isMoving = true;
            m_animator.SetBool("isMoving",b_isMoving);
        }
        else
        {
            b_isMoving = false;
            m_animator.SetBool("isMoving",b_isMoving);
        }

        m_animator.SetFloat("moveX", m_rigidbody.velocity.x);
        m_animator.SetFloat("moveY", m_rigidbody.velocity.y);
        Debug.Log(m_animator.GetFloat("moveX") + " " + m_animator.GetFloat("moveY"));

    }
}
