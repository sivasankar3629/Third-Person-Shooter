using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimaition : MonoBehaviour
{
    [SerializeField] Animator animator;
    PlayerInputs _inputActions;

    private void Awake()
    {
        _inputActions = new PlayerInputs();
        _inputActions.BasicMovement.Enable();
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        if (_inputActions.BasicMovement.Run.IsPressed())
        {
            animator.SetFloat("X", _inputActions.BasicMovement.Move.ReadValue<Vector2>().x);
            animator.SetFloat("Y", _inputActions.BasicMovement.Move.ReadValue<Vector2>().y);
        }
        else
        {
            animator.SetFloat("X", _inputActions.BasicMovement.Move.ReadValue<Vector2>().x * 0.3f);
            animator.SetFloat("Y", _inputActions.BasicMovement.Move.ReadValue<Vector2>().y * 0.3f);
        }
    }
}
