using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] public float Damage;
    [SerializeField] public int MagSize;
    [SerializeField] public float Firerate;
    [SerializeField] public int CurrentAmmo;
    [SerializeField] public float Range;
    [SerializeField] public bool IsReloading = false;
    [SerializeField] public VisualEffect MuzzleFlash;
    [SerializeField] public GameObject ImpactHitEffect;
    [SerializeField] public AnimationClip ReloadAnimation;
    [SerializeField] public Animator ReloadAnimator;
    private KeybindManager KeybindManager;
    private WeaponSway WeaponSway;

    public float TimeToShootAgain;

    protected virtual void Start()
    {
        CurrentAmmo = MagSize;
        KeybindManager = FindObjectOfType<KeybindManager>();
        WeaponSway = FindObjectOfType<WeaponSway>();

        if (ReloadAnimator == null)
        {
            Debug.LogError("ReloadAnimator not assigned!");
        }
        else
        {
            ReloadAnimator.enabled = false;
        }
    }

    protected virtual void Update()
    {
        if ((CurrentAmmo < MagSize && Input.GetKeyDown(KeybindManager.GetKeyCode("Reload"))) || (CurrentAmmo == 0 && Input.GetKeyDown(KeybindManager.GetKeyCode("Shoot"))))
        {
            StartReload();
        }

        if (!IsReloading)
        {
            SetAnimatorState(false);
        }
    }

    private Coroutine reloadCoroutine;

    protected IEnumerator Reload()
    {
        SetAnimatorState(true);
        IsReloading = true;
        if (ReloadAnimator != null)
        {
            ReloadAnimator.SetTrigger("Reload");
        }

        yield return new WaitForSeconds(ReloadAnimation.length - 0.05f);

        IsReloading = false;
        CurrentAmmo = MagSize;
        SetAnimatorState(false);
    }

    public void StartReload()
    {
        if (!IsReloading)
        {
            if (ReloadAnimator != null)
            {
                SetAnimatorState(true);
                ReloadAnimator.SetTrigger("Reload");
            }
            else
            {
                Debug.LogError("ReloadAnimator is null during StartReload.");
            }
            reloadCoroutine = StartCoroutine(Reload());
        }
    }

    public void StopReload()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            IsReloading = false;
            reloadCoroutine = null;
            SetAnimatorState(false);
        }
    }

    protected void TriggerMuzzleFlash()
    {
        if (MuzzleFlash != null)
        {
            MuzzleFlash.SendEvent("OnPlay");
        }
    }

    protected void HandleShooting()
    {
        if (CurrentAmmo > 0 && !IsReloading && Time.time >= TimeToShootAgain)
        {
            Debug.Log("Shooting...");
            TriggerMuzzleFlash();
            TimeToShootAgain = Time.time + 1f / Firerate;
            Shoot();
            WeaponSway.TriggerRecoil();
            CurrentAmmo--;
        }
    }

    protected abstract void Shoot();

    public void OnReloadComplete()
    {
        if (ReloadAnimator != null)
        {
            ReloadAnimator.SetFloat("ReloadProgress", 1f);
        }
    }

    private void SetAnimatorState(bool state)
    {
        if (ReloadAnimator != null)
        {
            ReloadAnimator.enabled = state;
        }
    }
}
