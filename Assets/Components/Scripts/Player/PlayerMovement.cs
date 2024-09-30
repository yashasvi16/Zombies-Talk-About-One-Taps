using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController m_characterController;
    PlayerInput m_playerInput;

    [Header("Player POV")]
    [SerializeField] float _rotationSpeed = 0.5f;

    [SerializeField] GameObject _cinemachineCameraTarget;
    [SerializeField] float _topClamp = 70.0f;
    [SerializeField] float _bottomClamp = -30.0f;
    [SerializeField] bool _invertX = false;
    [SerializeField] bool _invertY = false;
    float m_cinemachineTargetPitch;
    float m_upDown;
    float m_cameraHeight;
    float m_curleAngle = 0f;

    [Header("Player Movement")]
    [SerializeField] float _walkSpeed = 2f;
    [SerializeField] float _runSpeed = 5f;
    [SerializeField] Animator _handAnimator;
    [SerializeField] float _crouchSpeed = 1f;
    [SerializeField] float _crouchHeight = 1.2f;
    float m_targetSpeed;
    float m_normalHeight;
    float m_transitionHeight;

    [SerializeField] float _jumpHeight = 1f;
    [SerializeField] float _gravity = -15.0f;
    float m_verticalVelocity;
    float m_terminalVelocity = 53.0f;
    float m_fallTimeOut;
    float m_fallTimeOutDelta;
    float m_jumpTimeOut;
    float m_jumpTimeOutDelta;

    [Header("Playing Ground")]
    [SerializeField] LayerMask _groundLayers;
    [SerializeField] float _groundedOffset = -0.14f;
    [SerializeField] float _groundedRadius = 0.28f;
    [SerializeField] bool _grounded = true;

    const float m_transitionSpeed = 0.4f;

    [HideInInspector]
    public bool _playerMoving;

    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_playerInput = GetComponent<PlayerInput>();

        m_normalHeight = m_characterController.height;
        m_transitionHeight = m_normalHeight;
        m_cameraHeight = _cinemachineCameraTarget.transform.position.y;
        _playerMoving = false;
    }
    void Update()
    {
        CheckGrounded();
        CameraRotation();
        JumpAndGravity();
        Crouch();
        Move();
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (_grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z),
            _groundedRadius);
    }

    void CameraRotation()
    {
        float yInverter = -1;
        if (_invertY)
        {
            yInverter = 1;
        }
        float xInverter = 1;
        if (_invertX)
        {
            xInverter = -1;
        }

        Vector3 leftRight = xInverter * Vector3.up * m_playerInput.look.x * _rotationSpeed;
        m_upDown += yInverter * m_playerInput.look.y * _rotationSpeed;
        m_upDown = ClampAngle(m_upDown, _bottomClamp + (-89.98f), _topClamp + (-89.98f));

        m_cinemachineTargetPitch += yInverter * m_playerInput.look.y * _rotationSpeed;
        m_cinemachineTargetPitch = ClampAngle(m_cinemachineTargetPitch, _bottomClamp, _topClamp);

        _cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(m_cinemachineTargetPitch, 0.0f, m_curleAngle);

        transform.Rotate(leftRight);
    }

    void Move()
    {
        m_targetSpeed = m_playerInput.sprint ? _runSpeed : _walkSpeed;
        m_targetSpeed = m_playerInput.crouch ? _crouchSpeed : m_targetSpeed;


        Vector3 inputDirection = new Vector3(m_playerInput.move.x, 0, m_playerInput.move.y).normalized;

        if (m_playerInput.move == Vector2.zero)
        {
            m_targetSpeed = 0f;
            _playerMoving = false;
        }

        SetAniamationSpeed(m_targetSpeed);

        if (m_playerInput.move != Vector2.zero)
        {
            inputDirection = transform.right * m_playerInput.move.x + transform.forward * m_playerInput.move.y;
            _playerMoving = true;
        }

        m_characterController.Move(inputDirection.normalized * (m_targetSpeed * Time.deltaTime) + new Vector3(0, m_verticalVelocity, 0) * Time.deltaTime);


    }
    void JumpAndGravity()
    {
        if (_grounded && !m_playerInput.crouch)
        {
            m_fallTimeOutDelta = m_fallTimeOut;

            if (m_verticalVelocity < 0.0f)
            {
                m_verticalVelocity = -2f;
            }

            if (m_playerInput.jump && m_jumpTimeOutDelta <= 0.0f)
            {
                m_verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }

            if (m_jumpTimeOutDelta >= 0.0f)
            {
                m_jumpTimeOutDelta -= Time.deltaTime;
            }
        }
        else
        {
            m_jumpTimeOutDelta = m_jumpTimeOut;

            if (m_fallTimeOutDelta >= 0.0f)
            {
                m_fallTimeOutDelta -= Time.deltaTime;
            }
            m_playerInput.jump = false;
        }

        if (m_verticalVelocity < m_terminalVelocity)
        {
            m_verticalVelocity += _gravity * Time.deltaTime;
        }
    }

    void Crouch()
    {
        if (_grounded)
        {
            if (m_playerInput.crouch)
            {
                m_transitionHeight = Mathf.Lerp(m_transitionHeight, _crouchHeight, m_transitionSpeed * 0.4f);
            }
            else
            {
                m_transitionHeight = Mathf.Lerp(m_transitionHeight, m_normalHeight, m_transitionSpeed * 0.4f);
            }

            _cinemachineCameraTarget.transform.localPosition = new Vector3(_cinemachineCameraTarget.transform.localPosition.x, (m_transitionHeight / m_normalHeight) * m_cameraHeight, _cinemachineCameraTarget.transform.localPosition.z);
            m_characterController.center = new Vector3(m_characterController.center.x, m_transitionHeight / 2f, m_characterController.center.z);
            m_characterController.height = m_transitionHeight;
        }

    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    void CheckGrounded()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset,
                transform.position.z);
        _grounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers,
            QueryTriggerInteraction.Ignore);
    }

    void SetAniamationSpeed(float animationSpeed)
    {
        if (animationSpeed == 0)
        {
            animationSpeed = 1;
        }
        else if(animationSpeed == _walkSpeed)
        {
            animationSpeed = 3;
        }
        else if(animationSpeed == _runSpeed)
        {
            animationSpeed = 5;
        }

        _handAnimator.SetFloat("animationSpeed", animationSpeed);
    }
}
