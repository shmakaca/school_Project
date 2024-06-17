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
    [SerializeField] public VisualEffect MuzzelFlash;
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
        ReloadAnimator.enabled = false; 
    }

    protected virtual void Update()
    {
        if ((CurrentAmmo < MagSize && Input.GetKeyDown(KeybindManager.GetKeyCode("Reload"))) || CurrentAmmo == 0)
        {
            StartReload();
        }
    }

    private Coroutine reloadCoroutine;

    protected IEnumerator Reload()
    {
        IsReloading = true;
        ReloadAnimator.enabled = true; 
        ReloadAnimator.SetTrigger("Reload");
        yield return new WaitForSeconds(ReloadAnimation.length);
        IsReloading = false;
        CurrentAmmo = MagSize;
        ReloadAnimator.enabled = false; 
    }

    public void StartReload()
    {
        if (!IsReloading)
        {
            IsReloading = true;
            ReloadAnimator.enabled = true; 
            ReloadAnimator.SetTrigger("Reload");
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
            ReloadAnimator.enabled = false; 
        }
    }

    protected void TriggerMuzzleFlash()
    {
        if (MuzzelFlash != null)
        {
            MuzzelFlash.SendEvent("OnPlay");
        }
    }

    protected void HandleShooting()
    {
        if (CurrentAmmo > 0 && !IsReloading && Time.time >= TimeToShootAgain)
        {
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
        ReloadAnimator.SetFloat("ReloadProgress", 1f);
    }
}
