using Photon.Pun;
using System.Collections;
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
    [SerializeField] float DamagePerShot = 40;
    int totalBullets = 90;
    int extraBullets = 60;
    int bullets = 30;
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
        if (bullets < 1)
        {
            StartCoroutine(Reload());
        }
        if (bullets < 1) return;
        Fire();
        bullets--;
        GeneralUIManager.Instance.UpdateBullet(bullets);
        pv.RPC("Fire", RpcTarget.All);
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
            //Debug.Log(hit.collider.name);
            switch(hit.collider.tag) {
                case "Sand":
                    PlayParticles(_sandHitEffects, hit);
                    break;
                case "Metal":
                    PlayParticles(_metalHitEffects, hit);
                    break;
                case "Wood":
                    PlayParticles(_woodHitEffects, hit);
                    break;
                case "Enemy":
                    PlayParticles(_bloodHitEffects, hit);
                    break;
                default: //stone
                    PlayParticles(_stoneHitEffects, hit);
                    break;
            }
        }
    }

    IEnumerator Reload()
    {
        GeneralUIManager.Instance.reloadingText.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (totalBullets >= 30)
        {
            extraBullets = totalBullets - 30;
            GeneralUIManager.Instance.UpdateRemainingBullets(extraBullets);
            bullets = 30;
            GeneralUIManager.Instance.UpdateBullet(bullets);
        }
        else if (totalBullets < 30)
        {
            bullets = extraBullets + bullets;
            GeneralUIManager.Instance.UpdateBullet(bullets);
            extraBullets = 0;
            GeneralUIManager.Instance.UpdateRemainingBullets(extraBullets);
        }
        GeneralUIManager.Instance.reloadingText.SetActive(false);

    }

    void PlayParticles(ParticleSystem particle, RaycastHit hit)
    {
        particle.transform.position = hit.point;
        particle.Play();
        IDamagable enemy = hit.transform.GetComponent<IDamagable>();
        if (enemy != null)
        {
            enemy.TakeDamage(DamagePerShot);
        }
    }

    public void OnPlayerDeath()
    {
        _inputActions.BasicMovement.Disable();
    }
}
