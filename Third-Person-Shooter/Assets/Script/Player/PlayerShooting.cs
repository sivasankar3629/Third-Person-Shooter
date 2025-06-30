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
    float score = 0;
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
        bullets--;
        GeneralUIManager.Instance.UpdateBullet(bullets);

        // Firing
        Vector3 fireDirection = _fireTarget.position - _fireOrigin.position;
        Ray fireRay = new Ray(_fireOrigin.position, fireDirection);

        if (Physics.Raycast(fireRay, out RaycastHit hit))
        {
            Debug.DrawLine(_fireOrigin.position, hit.point, Color.red, 1f);
            string hitTag = hit.collider.tag;

            IDamagable damagable = hit.transform.GetComponent<IDamagable>();
            if (damagable != null)
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                float approxHealth = enemy.Health;
                damagable.TakeDamage(DamagePerShot);

                if (approxHealth <= DamagePerShot)
                {
                    score++;
                    GeneralUIManager.Instance.UpdateScoreText(score);
                }
            }
            pv.RPC(nameof(FireRPC), RpcTarget.All, hit.point, hit.normal, hitTag);
        }

    }

    private void Scope(InputAction.CallbackContext context)
    {
        if (!pv.IsMine) return;
        _cam.Lens.FieldOfView = _isScoped ? 60f : 30f;
        _isScoped = !_isScoped;

    }

    [PunRPC]
    void FireRPC(Vector3 point, Vector3 normal, string tag)
    {
        _muzzleFlash.Play();
        switch(tag) {
            case "Sand":
                PlayParticles(_sandHitEffects, point, normal);
                break;
            case "Metal":
                PlayParticles(_metalHitEffects, point, normal);
                break;
            case "Wood":
                PlayParticles(_woodHitEffects, point, normal);
                break;
            case "Enemy":
                PlayParticles(_bloodHitEffects, point, normal);
                 break;
            default: //stone
                PlayParticles(_stoneHitEffects, point, normal);
                break;
        }
        
    }

    void PlayParticles(ParticleSystem particle, Vector3 point, Vector3 normal)
    {
        if (particle == null)
        {
            Debug.Log("Particle system is null.");
            return;
        }
        particle.transform.position = point;
        particle.transform.rotation = Quaternion.LookRotation(normal);
        particle.Play();

    }

    void StopInput()
    {
        if (!pv.IsMine) return;
        _inputActions.BasicMovement.Disable();
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
    public void OnPlayerDeath()
    {
        StopInput();
        GeneralUIManager.Instance.UpdatePlayerDeath();
        GameOverUIManager.Instance.UpdateScoreText(score);
    }
}
