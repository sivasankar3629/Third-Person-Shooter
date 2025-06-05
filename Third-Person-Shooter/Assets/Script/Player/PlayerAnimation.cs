using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using Photon.Pun;

public class PlayerAnimaition : MonoBehaviour
{
    [SerializeField] Animator animator;
    PlayerInputs _inputActions;
    PhotonView pv;

    private void Awake()
    {
        _inputActions = new PlayerInputs();
        _inputActions.BasicMovement.Enable();
        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!pv.IsMine) return;
        Move();
    }

    void Move()
    {
        animator.SetFloat("X", _inputActions.BasicMovement.Move.ReadValue<Vector2>().x);
        animator.SetFloat("Y", _inputActions.BasicMovement.Move.ReadValue<Vector2>().y);

        #region PC settings
        //if (_inputActions.BasicMovement.Run.IsPressed())
        //{
        //    animator.SetFloat("X", _inputActions.BasicMovement.Move.ReadValue<Vector2>().x * 0.3f);
        //    animator.SetFloat("Y", _inputActions.BasicMovement.Move.ReadValue<Vector2>().y * 0.3f);
        //}
        //else
        //{
        //    animator.SetFloat("X", _inputActions.BasicMovement.Move.ReadValue<Vector2>().x);
        //    animator.SetFloat("Y", _inputActions.BasicMovement.Move.ReadValue<Vector2>().y);
        //}
        #endregion
    }
}
