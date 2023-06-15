using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    private int maxAmmo;
    public int MaxAmmo{
        get => maxAmmo;
        set
        {
            maxAmmo = value;
            ((InGameManager)GameManager.Instance.SceneController).SetBulletText(currentAmmo,maxAmmo);
        }
    }
    private int currentAmmo;
    public int CurrentAmmo{
        get => currentAmmo;
        set
        {
            currentAmmo = value;
            ((InGameManager)GameManager.Instance.SceneController).SetBulletText(currentAmmo,maxAmmo);
        }
    }
    public int damage;
    private float reloadTime = 3f;
    protected Transform firePos;
    protected Camera mainCam;
    [SerializeField]
    protected float shotDelay = 0.1f;
    [SerializeField]
    protected LayerMask hitLayer;
    [SerializeField]
    protected BulletLine bulletLine;
    [SerializeField]
    protected Transform FirePos;

    private AgentAnimator animator;
    public AgentAnimator Animator{
        get{
            if(animator == null)
            {
                animator = GameObject.Find("OverLayCam/Arm").GetComponent<AgentAnimator>();
            }
            return animator;
        }
    }

    protected bool isFire;
    public virtual void Init()
    {
        isFire = true;
    }
    public abstract bool Fire();
    public abstract void Reload(); 
}
