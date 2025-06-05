using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterController _controller;
    [SerializeField] Transform _groundCheckTransform;
    private PlayerInputs _inputActions;
    [SerializeField] Camera _cam;

    [Header("Player Settings")]
    [SerializeField] float _walkSpeed = 3;
    [SerializeField] float _runSpeed = 6;
    [SerializeField] float _jumpHeight= 3;
    [SerializeField] float _gravity = 9.81f;
    [SerializeField] float _turnSpeed = 3;

    private Vector3 _inputs;
    private Vector3 _moveDirection;
    private PhotonView pv;

    #region LifeCycle

    private void Awake()
    {
        _inputActions = new PlayerInputs();
        _cam = FindAnyObjectByType<Camera>();
        pv = GetComponent<PhotonView>();
        //Debug.Log(pv.Owner.NickName);
    }

    private void OnEnable()
    {
        _inputActions.BasicMovement.Enable();
        _inputActions.BasicMovement.Jump.started += Jump;
    }


    private void FixedUpdate()
    {
        if (!pv.IsMine) return;
        Debug.Log($"{pv.Owner.NickName} | IsMine: {pv.IsMine}");
        Movement();
        Gravity();
        PlayerRotation();

        _controller.Move(_moveDirection * Time.deltaTime);
    }

    private void OnDisable()
    {
        _inputActions.BasicMovement.Jump.started -= Jump;
        _inputActions.BasicMovement.Disable();
    }

    #endregion

    private void Movement()
    {
        if (_inputActions.BasicMovement.Move.IsInProgress())
        {
            Move();
        }
        else
        {
            _moveDirection.x = 0;
            _moveDirection.z = 0;
        }
    }

    private void Move()
    {
        _inputs.z = _inputActions.BasicMovement.Move.ReadValue<Vector2>().y;
        _inputs.x = _inputActions.BasicMovement.Move.ReadValue<Vector2>().x;

        _moveDirection = (transform.right * _inputs.x + transform.forward * _inputs.z + transform.up * _inputs.y) * _runSpeed;

        #region PC settings
        //if (_inputActions.BasicMovement.Run.IsPressed())
        //{
        //    _moveDirection = (transform.right * _inputs.x + transform.forward * _inputs.z + transform.up * _inputs.y) * _runSpeed;
        //}
        //else
        //{
        //    _moveDirection = (transform.right * _inputs.x + transform.forward * _inputs.z + transform.up * _inputs.y) * _walkSpeed;
        //}
        #endregion
    }

    private void Jump(InputAction.CallbackContext callback)
    {
        Debug.Log(transform.up);
        if (IsGrounded())
        {
            _inputs.y = Mathf.Sqrt(_jumpHeight * _gravity);
            Debug.Log(_inputs.y);
        }
    }

    private void Gravity()
    {
        if (IsGrounded())
        {
            _moveDirection.y = -2f;
        }
        else
        {
            _moveDirection.y -= 9.81f * Time.deltaTime  ;
        }
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(_groundCheckTransform.position, 0.1f, LayerMask.NameToLayer("Player"), QueryTriggerInteraction.Ignore);
    }

    void PlayerRotation()
    {
        float camRotation = _cam.transform.rotation.eulerAngles.y;
        _controller.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, camRotation, 0), _turnSpeed * Time.deltaTime);
    }
}
