using System.Collections.Generic;
using UnityEngine;

namespace Supercyan.FreeSample
{
    public class SimpleSampleCharacterControl : MonoBehaviour
    {
        private enum ControlMode
        {
            /// <summary>
            /// Up moves the character forward, left and right turn the character gradually and down moves the character backwards
            /// </summary>
            Tank,
            /// <summary>
            /// Character freely moves in the chosen direction from the perspective of the camera
            /// </summary>
            Direct
        }

        [SerializeField] private float m_moveSpeed = 2;
        [SerializeField] private float m_turnSpeed = 200;
        [SerializeField] private float m_jumpForce = 4;

        [SerializeField] private Animator m_animator = null;
        //[SerializeField] private Rigidbody m_rigidBody = null;
        [SerializeField] private CharacterController m_characterController = null;
        [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;
        [SerializeField] private Actor _actor;
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

        private void FixedUpdate()
        {
            m_animator.SetBool("Grounded", m_characterController.isGrounded);

            switch (m_controlMode)
            {
                case ControlMode.Direct:
                    DirectUpdate();
                    break;

                case ControlMode.Tank:
                    break;

                default:
                    Debug.LogError("Unsupported state");
                    break;
            }

            m_jumpInput = false;
        }
        
        private void DirectUpdate()
        {
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

                m_animator.SetFloat("MoveSpeed", direction.magnitude);
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
            _actor.SetPosition(transform.position);
            _actor.SetRotation(transform.eulerAngles);
            _actor.SetSpeed(m_currentDirection * m_moveSpeed + Vector3.up * m_curJumpSpeed);
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
    }
}
