using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    private PlayerInputs _inputActions;
    [SerializeField] Transform _fireOrigin;
    [SerializeField] Transform _fireTarget;
    [SerializeField] ParticleSystem _bloodHitEffects;
    [SerializeField] ParticleSystem _woodHitEffects;
    [SerializeField] ParticleSystem _stoneHitEffects;
    [SerializeField] ParticleSystem _sandHitEffects;
    [SerializeField] ParticleSystem _metalHitEffects;
    [SerializeField] ParticleSystem _muzzleFlash;
    [SerializeField] CinemachineCamera _cam;
    PhotonView pv;

    bool _isScoped = false;

    private void Awake()
    {
        _inputActions = new PlayerInputs();
        _inputActions.BasicMovement.Enable();
        pv = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        _inputActions.BasicMovement.Attack.started += Attack;
        _inputActions.BasicMovement.Scope.started += Scope;
    }

    private void OnDisable()
    {
        _inputActions.BasicMovement.Attack.started -= Attack;
        _inputActions.BasicMovement.Scope.started -= Scope;
    }

    private void Attack(InputAction.CallbackContext callback)
    {
        if (!pv.IsMine) return;
        Fire();
        pv.RPC("Fire", RpcTarget.Others);
    }

    private void Scope(InputAction.CallbackContext context)
    {
        if (!pv.IsMine) return;
        if (_isScoped)
        {
            _cam.Lens.FieldOfView = 60f;
            _isScoped = false;
        }
        else
        {
            _cam.Lens.FieldOfView = 30f;
            _isScoped = true;
        }
    }

    [PunRPC]
    void Fire()
    {
        _muzzleFlash.Play();

        Vector3 fireDirection = _fireTarget.position - _fireOrigin.position;
        Ray fireRay = new Ray(_fireOrigin.position, fireDirection);
        RaycastHit hit;

        if (Physics.Raycast(fireRay, out hit))
        {
            Debug.DrawLine(_fireOrigin.position, hit.point, Color.red, 1f);
            Debug.Log(hit.collider.name);
            switch(hit.collider.tag) {
                case "Sand":
                    _sandHitEffects.transform.position = hit.point;
                    _sandHitEffects.Play();
                    break;
                case "Metal":
                    _metalHitEffects.transform.position = hit.point;
                    _metalHitEffects.Play();
                    break;
                case "Stone":
                    _stoneHitEffects.transform.position = hit.point;
                    _stoneHitEffects.Play();
                    break;
                case "Wood":
                    _woodHitEffects.transform.position = hit.point;
                    _woodHitEffects.Play();
                    break;
                case "Enemy":
                    _bloodHitEffects.transform.position = hit.point;
                    _bloodHitEffects.Play();
                    Destroy(hit.collider.gameObject,1f);
                    break;
                default:
                    break;
            }
        }
    }
}
