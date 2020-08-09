using Assets.Scripts.WeaponScripts.RangedWeaponClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class RangedWeaponController : MonoBehaviour {
    public GameObject Target;
    public float damping = 1f;

    [Space]
    [Header("Rotation Properties")]
    [Range(0.0f, 0.5f)]
    [SerializeField]
    private float YawMax;
    public Quaternion CurrentYaw;
    [Range(0.0f, 0.2f)]
    [SerializeField]
    private float PitchMax;
    public Quaternion CurrentPitch;

    [Space]
    [Header("Ammunition Properties")]
    [Tooltip("The game object that will spawn directly infront of the weapon barrel.")]
    [SerializeField]
    private GameObject Projectile;
    public int AmmunitionCount;
    public int AmmoUsedPerShot;

    [Space]
    public ParticleSystem FireParticleEffect;

    private Transform mount;
    private Transform gimble;
    private Transform weapon;
    private Transform muzzle;

    private Animator anim;
    private AudioSource fireSound;

    // Use this for initialization
    void Start ()
    {
        // Set components
        anim = GetComponent<Animator>();
        fireSound = GetComponent<AudioSource>();
    }
	
    void Awake ()
    {
        // Locate Pieces
        mount = transform.Find("Mount"); // Mount never rotates
        gimble = mount.transform.Find("Gimble"); // Gimble Rotates only by Y
        weapon = gimble.transform.Find("Weapon"); // Weapon Rotates only by Z
        muzzle = weapon.transform.Find("MuzzlePoint");
    }

    void FixedUpdate()
    {
        // Button Actions
        if (Input.GetButtonDown("Fire1"))
        {
            FiringEvent();
        }
    }

	void Update ()
    {
        if (AmmunitionCount < AmmoUsedPerShot) return;
        if (Target == null) return;
        PerformRotation();


    }

    private void PerformRotation()
    {
        RotateToFace.FaceTarget(Target, gimble, true, false, true);
        weapon.LookAt(Target.transform);

        gimble.localRotation = new Quaternion(gimble.localRotation.x, Mathf.Clamp(gimble.localRotation.y, YawMax * -1, YawMax), gimble.localRotation.z, gimble.localRotation.w);
        weapon.localRotation = new Quaternion(Mathf.Clamp(weapon.localRotation.x, PitchMax * -1, PitchMax), weapon.localRotation.y, weapon.localRotation.z, weapon.localRotation.w);

        // Just display rotational values
        CurrentYaw = gimble.localRotation;
        CurrentPitch = weapon.localRotation;
    }

    public void FiringEvent()
    {
        fireSound.Play();
        var muz = transform.Find("Mount").Find("Gimble").Find("Weapon").Find("MuzzlePoint").transform;
        if (Projectile != null) Instantiate(Projectile, muz.position, muz.rotation);
        if (FireParticleEffect != null) Instantiate(FireParticleEffect, muz.position, muz.rotation, gameObject.transform);
        AmmunitionCount -= AmmoUsedPerShot;

        // if (!anim.GetBool("IsFired"))
        // {
        //     fireSound.Play();
        //     anim.SetBool("IsFired", true);
        //     var muz = transform.Find("Mount").Find("Gimble").Find("Weapon").Find("MuzzlePoint").transform;
        //     if (Projectile != null) Instantiate(Projectile, muz.position, muz.rotation);
        //     if (FireParticleEffect != null) Instantiate(FireParticleEffect, muz.position, muz.rotation, gameObject.transform);
        //     AmmunitionCount -= AmmoUsedPerShot;
        // }
             
    }

    // public void FiredEvent()
    // {
    //     anim.SetBool("IsFired", false);
    // }


}
