using System.Collections.Generic;
using UnityEngine;



public class SimpleSampleCharacterControl : Actor
    {
       
        [SerializeField] private float m_moveSpeed = 2;
        [SerializeField] private float m_turnSpeed = 200;
        [SerializeField] private float m_jumpForce = 4;

        //[SerializeField] private Rigidbody m_rigidBody = null;
        [SerializeField] private CharacterController m_characterController = null;
        private float m_currentV = 0;
        private float m_currentH = 0;

        private readonly float m_interpolation = 10;
        private readonly float m_walkScale = 0.33f;
        private readonly float m_backwardsWalkScale = 0.16f;
        private readonly float m_backwardRunScale = 0.66f;

        private bool m_wasGrounded;
        private Vector3 m_currentDirection = Vector3.zero;

        private float m_jumpTimeStamp = 0;
        private float m_minJumpInterval = 0.25f;
        private bool m_jumpInput = false;
        private float m_curJumpSpeed = 0;
        private bool m_isJumping;
        
        // 输入历史队列（用于回溯）
        private Queue<PlayerInput> _inputQueue = new Queue<PlayerInput>();
        
       
        
        private void Awake()
        {
            if (!m_animator) { gameObject.GetComponentInChildren<Animator>(); }
            if (!m_characterController) { gameObject.GetComponent<CharacterController>(); }
        }

        private void Update()
        {
            if (!m_jumpInput && Input.GetKey(KeyCode.Space))
            {
                m_jumpInput = true;
            }
        }

        public override void DoFixedUpdate()
        {
            base.DoFixedUpdate();
            _animationController.DoFixedUpdate();
            m_jumpInput = false;
        }
        

        #region Player

        protected override void DirectUpdate()
        {
            _animationController.SetBool("Grounded", m_characterController.isGrounded);

            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            Transform camera = Camera.main.transform;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                v *= m_walkScale;
                h *= m_walkScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.fixedDeltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.fixedDeltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.fixedDeltaTime * m_interpolation);

                transform.rotation = Quaternion.LookRotation(m_currentDirection);
                //transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

                // m_animator.SetFloat("MoveSpeed", direction.magnitude);
                _animationController.SetFloat("MoveSpeed", direction.magnitude);
            }

            JumpingAndLanding(Time.fixedDeltaTime);
        }

        private void JumpingAndLanding(float dt)
        {
            bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

            if (jumpCooldownOver && m_characterController.isGrounded && m_jumpInput)
            {
                m_jumpTimeStamp = Time.time;
                m_curJumpSpeed = m_jumpForce;
                m_isJumping = true;
            }
            // m_characterController.SimpleMove(m_currentDirection * m_moveSpeed + Vector3.up * m_curJumpSpeed);
            m_characterController.Move((m_currentDirection * m_moveSpeed + Vector3.up * m_curJumpSpeed) * dt);
            if (GetActorState() == EActorState.Syncing)
            {
                _inputQueue.Enqueue(new PlayerInput
                {
                    deltaTime = dt,
                    MoveDirection = (m_currentDirection * m_moveSpeed + Vector3.up * m_curJumpSpeed)
                });
            }
            
            SetSpeed(transform.position - GetPosition() / dt);
            SetPosition(transform.position);
            SetRotation(transform.eulerAngles);
            if (!m_characterController.isGrounded)
            {
                m_curJumpSpeed -= 9.8f * dt;
            }
            if (!m_isJumping) return;
         

            if (m_characterController.isGrounded && m_curJumpSpeed < 0)
            {
                m_curJumpSpeed = 0;
                m_isJumping = false;
            }
        }

        protected override void OnChangeState(EActorState state)
        {
            base.OnChangeState(state);
            if (state == EActorState.Syncing)
            {
                _inputQueue.Clear();
            }
        }

        public override void SetServerPosition(Vector3 position)
        {
            base.SetServerPosition(position);
            if (IsHost())
            {
                //做一个误差校正
                transform.position = position;
                Vector3 serverPos = transform.position;
                int count = _inputQueue.Count;
                while (_inputQueue.Count > 0)
                {
                    PlayerInput input = _inputQueue.Dequeue();
                    m_characterController.Move(input.MoveDirection * input.deltaTime);
                }
                // Debug.Log( $"SetServerPosition: {tfPos} -> {serverPos} count: {count}");
            }
        }

        #endregion
        
       
    }
    
// 输入数据结构
public struct PlayerInput
{
    public float deltaTime;
    public Vector3 MoveDirection;
}