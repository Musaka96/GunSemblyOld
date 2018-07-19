using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SimpleCharacterControl : MonoBehaviour
{

    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;
    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;
    [SerializeField] private Object projectile;

    //[SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 100;
    private readonly float m_walkScale = 2f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    //   Jumping
    private bool shift_mod;
    private bool double_jumped;
    private bool can_double_jump;
    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;
    private bool m_isGrounded;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private List<Collider> m_collisions = new List<Collider>();
    public object component;

    public GameObject IntBox { get; set; }

    private bool canInteract = true;
    private bool canWork = true;
    public bool isWorking = false;

    public Image progressBar;
    private bool itemHasProgressbar;

    void Start()
    {
        shift_mod = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                onGrounded();
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            onGroundedStay();
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

    void Update()
    {
        MovementUpdate();
        JumpingAndLandingUpdate();

        Interakcija skripta = GetComponentInChildren<Interakcija>();
        m_animator.SetBool("Grounded", m_isGrounded);


        //progress barr stuff
        progressBar.transform.parent.parent.eulerAngles = new Vector3(
             Camera.main.transform.eulerAngles.x,
             Camera.main.transform.gameObject.transform.eulerAngles.y,
             transform.eulerAngles.z);


        m_wasGrounded = m_isGrounded;

        if (Input.GetKey(KeyCode.E))
        {
            if (Interakcija._triggered && canInteract)
            {
                skripta.Interact();
                canInteract = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            canInteract = true;
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (Interakcija._triggered && canWork)
            {
                print("zove work na interakciju");
                skripta.Work();
                canWork = false;
                isWorking = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (Interakcija._triggered && !canWork)
            {
                skripta.stopWork();
                canWork = true;
                isWorking = false;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            shift_mod = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            shift_mod = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jump();
            can_double_jump = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jump();
            can_double_jump = true;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            GameObject bullet = Instantiate(projectile, m_rigidBody.transform.position + m_rigidBody.transform.forward * 4, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody>().AddForce(m_rigidBody.transform.forward * 1000);
        }

    }

    private void jump()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        //just space held
        if (jumpCooldownOver && m_isGrounded)
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        // double jump
        if (!m_isGrounded && !double_jumped && can_double_jump)
        {
            if (jumpCooldownOver)
            {
                m_jumpTimeStamp = Time.time;
                m_rigidBody.velocity = new Vector3(0,0,0);
                m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
                //m_rigidBody.useGravity = true;
            }
            else
            {
                double_jumped = true;
            }
        }
    }

    private void onGrounded()
    {
        m_isGrounded = true;
        double_jumped = false;

    }

    private void onGroundedStay()
    {

    }

    private void Interact()
    {

    }

    //private void TankUpdate()
    //{
    //    float v = Input.GetAxis("Vertical");
    //    float h = Input.GetAxis("Horizontal");

    //    bool walk = Input.GetKey(KeyCode.LeftShift);

    //    if (v < 0) {
    //        if (walk) { v *= m_backwardsWalkScale; }
    //        else { v *= m_backwardRunScale; }
    //    } else if(walk)
    //    {
    //        v *= m_walkScale;
    //    }

    //    m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
    //    m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

    //    transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
    //    transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

    //    m_animator.SetFloat("MoveSpeed", m_currentV);

    //    JumpingAndLanding();

    //}

    private void MovementUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Transform camera = Camera.main.transform;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            v *= m_walkScale;
            h *= m_walkScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", direction.magnitude);
        }
    }

    private void JumpingAndLandingUpdate()
    {

        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetTrigger("Land");
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            m_animator.SetTrigger("Jump");
        }
    }

}
