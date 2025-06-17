using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterController _controller;
    [SerializeField] Transform _groundCheckTransform;
    private PlayerInputs _inputActions;
    [SerializeField] Camera _cam;
    [SerializeField] CinemachineCamera _vCam;

    [Header("Player Settings")]
    [SerializeField] float _walkSpeed = 3;
    [SerializeField] float _runSpeed = 6;
    [SerializeField] float _jumpHeight = 3;
    [SerializeField] float _gravity = 9.81f;
    [SerializeField] float _turnSpeed = 3;
    private float verticalVelocity = 0;

    private Vector3 _inputs;
    private Vector3 _moveDirection;
    private PhotonView pv;

    #region LifeCycle

    private void Awake()
    {
        _inputActions = new PlayerInputs();
        pv = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        _inputActions.BasicMovement.Enable();
    }

    void Start()
    {
        if (pv.IsMine)
        {
            _vCam.gameObject.SetActive(true);
            _cam.gameObject.SetActive(true);
        }
    }



    private void FixedUpdate()
    {
        if (!pv.IsMine) return;
        //Debug.Log($"{pv.Owner.NickName} | IsMine: {pv.IsMine}");
        Move();
        PlayerRotation();
    }

    private void OnDisable()
    {
        _inputActions.BasicMovement.Disable();
    }

    #endregion


    private void Move()
    {
        _inputs.z = _inputActions.BasicMovement.Move.ReadValue<Vector2>().y;
        _inputs.x = _inputActions.BasicMovement.Move.ReadValue<Vector2>().x;

        _moveDirection = new Vector3(_inputs.x, 0f, _inputs.z) * _runSpeed;
        _moveDirection.y = Gravity();
        _moveDirection = transform.TransformDirection(_moveDirection);

        _controller.Move(_moveDirection * Time.fixedDeltaTime);
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

    private float Gravity()
    {
        if (IsGrounded())
        {
            verticalVelocity = -2f;

            if (_inputActions.BasicMovement.Jump.triggered)
            {
                verticalVelocity = Mathf.Sqrt(_jumpHeight * _gravity * 2);
            }
        }
        else
        {
            verticalVelocity -= _gravity * Time.fixedDeltaTime;
        }
        return verticalVelocity;
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
