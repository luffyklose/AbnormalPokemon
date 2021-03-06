using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float checkInterval;
    [SerializeField] private float BattleProbablity;
    [SerializeField] string name;
    [SerializeField] Sprite sprite;

    public Sprite Sprite
    {
        get => sprite;
    }

    public string Name
    {
        get => name;
    }
    
    private bool b_isMoving;
    private float counter;

    public Rigidbody2D m_rigidbody;
    public Animator m_animator;
    public LayerMask EncounterArea;
    public LayerMask interactable;
    public LayerMask DetectRange;

    public event Action onEncountered;
    public event Action<Collider2D> onTrainerEncounter;
    
    [Header("Audio")] 
    private AudioSource m_audioSource;
    public AudioClip FootstepOnDirtFX;
    public AudioClip FootstepOnGrassFX;

    [Header("Particle System")] 
    public ParticleSystem FootstepPS;
    private ParticleSystem.ColorOverLifetimeModule PSColorOverLifetime;
    
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
        PSColorOverLifetime = FootstepPS.colorOverLifetime;
    }

    // Update is called once per frame
    public void HandleUpdate()
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
            m_animator.SetFloat("moveX", m_rigidbody.velocity.x);
            m_animator.SetFloat("moveY", m_rigidbody.velocity.y);

            counter += Time.deltaTime;
            if (counter > checkInterval)
            {
                CheckEncounterBattle();
                counter = 0.0f;
            }
        }
        else
        {
            b_isMoving = false;
        }
        
        m_animator.SetBool("isMoving",b_isMoving);
        //Debug.Log(m_animator.GetFloat("moveX") + " " + m_animator.GetFloat("moveY"));

        if (b_isMoving)
        {
            if (Physics2D.OverlapBox(transform.position, transform.localScale / 2, 0.0f, EncounterArea))
            {
                m_audioSource.clip = FootstepOnGrassFX;
                if(!m_audioSource.isPlaying)
                    m_audioSource.Play();
            }
            else
            {
                m_audioSource.clip = FootstepOnDirtFX;
                if(!m_audioSource.isPlaying)
                    m_audioSource.Play();
            }

            PSColorOverLifetime.color = Color.white;
        }
        else
        {
            m_audioSource.Stop();
            FootstepPS.Stop();
            PSColorOverLifetime.color = Color.clear;
        }

        CheckInTrainDetectRange();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }

    private void OnMoveOver()
    {
        CheckEncounterBattle();
        CheckInTrainDetectRange();
    }

    private void CheckEncounterBattle()
    {
        if (Physics2D.OverlapBox(transform.position, transform.localScale / 2, 0.0f, EncounterArea))
        {
            if (UnityEngine.Random.Range(1, 101) < 100 * BattleProbablity)
            {
                //Debug.Log("jinlaile");
                m_rigidbody.velocity=Vector2.zero;
                m_animator.SetBool("isMoving",false);
                b_isMoving = false;
                onEncountered();
            }
            else
            {
                //Debug.Log("meijinlai");
            }
        }
    }

    private void CheckInTrainDetectRange()
    {
        var collider = Physics2D.OverlapBox(transform.position, transform.localScale / 2, 0.0f, DetectRange);
        
        if (collider != null)
        {
            m_rigidbody.velocity=Vector2.zero;
            m_animator.SetBool("isMoving",false);
            b_isMoving = false;
            m_audioSource.Stop();
            FootstepPS.Stop();
            PSColorOverLifetime.color = Color.clear;
            onTrainerEncounter.Invoke(collider);
        } 
    }

    void Interact()
    {
        Vector3 faceDir = new Vector3(m_animator.GetFloat("moveX") * 0.25f, m_animator.GetFloat("moveY") * 0.25f);
        Vector3 interactPos = transform.position + faceDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactable);
        
        if (collider != null)
        {
            collider.GetComponent<Interactable>().Interact(transform);
        }
        
        //Debug.Log("interact");
    }

    public void PlayFootstepPS()
    {
        FootstepPS.Play();
    }
}
