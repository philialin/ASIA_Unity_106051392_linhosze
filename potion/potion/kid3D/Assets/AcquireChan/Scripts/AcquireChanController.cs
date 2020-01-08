using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*!
 *	----------------------------------------------------------------------
 *	@brief	アクワイアちゃん簡易操作スクリプト
 *	
*/
public class AcquireChanController : MonoBehaviour
{
	// Inspector
	[SerializeField] private float	m_WalkSpeed		= 2.0f;
	[SerializeField] private float	m_RunSpeed		= 3.5f;
	[SerializeField] private float	m_RotateSpeed	= 8.0f;
	[SerializeField] private float	m_JumpForce		= 300.0f;

	// member
	private Rigidbody	m_RigidBody	= null;
	private Animator	m_Animator	= null;
	private float		m_MoveTime	= 0;
	private float		m_MoveSpeed	= 0.0f;
	private bool		m_IsGround	= true;


    public float imtime;
    private bool canm;
    private bool is_catch;
    private bool is_trigg;
    private itemset_a targ;
    public GameObject m_tar;



    /*!
	 *	----------------------------------------------------------------------
	 *	@brief	生成
	*/
    private void Awake()
	{
		m_RigidBody = this.GetComponentInChildren<Rigidbody>();
		m_Animator = this.GetComponentInChildren<Animator>();
		m_MoveSpeed = m_WalkSpeed;
        canm = true;
        is_trigg = false;
	}

	/*!
	 *	----------------------------------------------------------------------
	 *	@brief	初期化
	*/
//	private void Start()
//	{
//	}

	/*!
	 *	----------------------------------------------------------------------
	 *	@brief	更新
	*/
	private void Update()
	{
		if( null == m_RigidBody ) return;
		if( null == m_Animator ) return;

		// check ground
		float rayDistance = 0.3f;
		Vector3 rayOrigin = (this.transform.position + (Vector3.up * rayDistance * 0.5f));
		bool ground = Physics.Raycast( rayOrigin, Vector3.down, rayDistance, LayerMask.GetMask( "Default" ) );
		if( ground != m_IsGround )
		{
			m_IsGround = ground;

			// landing
			if( m_IsGround )
			{
				m_Animator.Play( "landing" );
			}
		}

		// input
		Vector3 vel = m_RigidBody.velocity;
		float h = Input.GetAxis( "Horizontal" );
		float v = Input.GetAxis( "Vertical" );
        bool isMove = ((0 != h) || (0 != v)) && canm;

		m_MoveTime = isMove? (m_MoveTime + Time.deltaTime) : 0;
        bool isRun = isMove && (Input.GetKey(KeyCode.Q));

        if (canm)
        {
            // move speed (walk / run)
            float moveSpeed = isRun ? m_RunSpeed : m_WalkSpeed;
            m_MoveSpeed = isMove ? Mathf.Lerp(m_MoveSpeed, moveSpeed, (8.0f * Time.deltaTime)) : m_WalkSpeed;
            //		m_MoveSpeed = moveSpeed;

            Vector3 inputDir = new Vector3(h, 0, v);
            if (1.0f < inputDir.magnitude) inputDir.Normalize();

            if (0 != h) vel.x = (inputDir.x * m_MoveSpeed);
            if (0 != v) vel.z = (inputDir.z * m_MoveSpeed);

            m_RigidBody.velocity = vel;

            if (isMove)
            {
                // rotation
                float t = (m_RotateSpeed * Time.deltaTime);
                Vector3 forward = Vector3.Slerp(this.transform.forward, inputDir, t);
                this.transform.rotation = Quaternion.LookRotation(forward);
            }
        }
		m_Animator.SetBool( "isMove", isMove );
		m_Animator.SetBool( "isRun", isRun );


		// jump
		if( Input.GetButtonDown( "Jump" ) && m_IsGround		)
		{
			m_Animator.Play( "jump" );
			m_RigidBody.AddForce( Vector3.up * m_JumpForce );
		}

        if (Input.GetKeyDown(KeyCode.Mouse0) && canm) 
        {
            canm = false;
            m_Animator.SetBool("spdebug", true);
            Invoke("sp", imtime);

            if (is_trigg && !is_catch)
            {
                is_catch = true;
                targ.rig.useGravity = false;
                targ.coli.enabled = false;
                targ.target = m_tar;
            }
            else if (is_catch)
            {
                is_catch = false;
                targ.rig.useGravity = true;
                targ.coli.enabled = true;
                targ.target = null;
            }

        }

		// quit
		if( Input.GetKeyDown( KeyCode.Escape ) ) Application.Quit();
	}

    void sp()
    {
        Invoke("m", 0.25f);
        m_Animator.SetBool("spdebug", false);
    }

    void m()
    {
        canm = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "item"&&!is_catch)
        {
            is_trigg = true;
            targ = other.transform.parent.parent.GetComponent<itemset_a>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "item")
        {
            is_trigg = false;
        }
    }
}
