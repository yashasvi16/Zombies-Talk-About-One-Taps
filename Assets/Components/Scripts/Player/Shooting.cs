using UnityEngine;
using UnityEngine.AI;

public class Shooting : MonoBehaviour
{
    [SerializeField] float _damage;
    [SerializeField] float _timeBetweenShooting, _spread, _range, _reloadTime, _timeBetweenShots;
    [SerializeField] int _magazineSize, _bulletsPerTap;
    [SerializeField] bool _holdAllowed;
    [SerializeField] LayerMask _enemy;
    [SerializeField] Camera _fpsCam;
    [SerializeField] ShakeCinemachine _shakeCinemachine;

    private int m_bulletsLeft, m_bulletsShot;
    private bool m_shooting, m_readyToShoot, m_reloading;

    Animator m_animator;
    PlayerInput m_playerInput;
    RaycastHit _rayHit;
    AudioSource m_audioSource;

    /*public GameObject _muzzleFlash, _bulletHoleGraphic;*/
    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_playerInput = GetComponent<PlayerInput>();
        m_audioSource = GetComponent<AudioSource>();

        m_bulletsLeft = _magazineSize;
        m_readyToShoot = true;
    }
    private void FixedUpdate()
    {
        MyInput();
    }
    private void MyInput()
    {
        m_shooting = m_playerInput.shoot;

        if (((m_playerInput.reload && m_bulletsLeft < _magazineSize) || m_bulletsLeft <= 0) && !m_reloading) Reload();

        if (m_readyToShoot && m_shooting && !m_reloading && m_bulletsLeft > 0)
        {
            m_bulletsShot = _bulletsPerTap;
            Shoot();
            m_animator.SetTrigger("Shoot");
            PlayGunAudio();
        }
    }
    private void Shoot()
    {
        m_readyToShoot = false;

        float x = Random.Range(-_spread / 2, _spread / 2);
        float y = Random.Range(0, _spread * 3);

        Vector3 direction = _fpsCam.transform.forward + new Vector3(x, y, 0);

        if (Physics.Raycast(_fpsCam.transform.position, direction, out _rayHit, _range, _enemy))
        {
            if(_rayHit.collider.gameObject.CompareTag("EnemyHead"))
            {
                _rayHit.collider.gameObject.GetComponentInParent<Enemy>().DeadEnemy();
                GameManager.Instance.UpdateScore();
            }
        }

        m_bulletsLeft--;
        m_bulletsShot--;

        Invoke("ResetShot", _timeBetweenShooting);

        if (m_bulletsShot > 0 && m_bulletsLeft > 0 && _holdAllowed)
        {
            Invoke("Shoot", _timeBetweenShots);
        }

    }
    private void ResetShot()
    {
        m_readyToShoot = true;
    }
    private void Reload()
    {
        m_reloading = true;
        Invoke("ReloadFinished", _reloadTime);
    }
    private void ReloadFinished()
    {
        m_bulletsLeft = _magazineSize;
        m_reloading = false;
    }

    private void PlayGunAudio()
    {
        m_audioSource.Play();
    }
}
