using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Events;

public class Rifle : Gun
{
    public UnityEvent fireEvnet;
    public Transform ZoomFirePos;
    public override bool Fire()
    {
        if (CurrentAmmo > 0)
        {
            if (isFire == true)
            {
                agentInput.AddForce(Random.Range(-recoilX,recoilX),recoilY);
                BulletLine bl = Instantiate(bulletLine, Vector3.zero, Quaternion.identity);
                StartCoroutine(DelayCor(shotDelay));
                Ray ray = Define.MainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 2000f, hitLayer))
                {
                    if (hit.transform.TryGetComponent<OtherPlayer>(out OtherPlayer player))
                    {
                        if(player.HitDamage(damage))
                        {
                            GameManager.Instance.WinPoint++;
                        }
                    }
                }
                else
                {
                    hit.point = (Define.MainCam.transform.forward * 1000f);
                }
                if(GameManager.Instance.SceneEnum == SceneTypes.InGame)
                    fireEvnet?.Invoke();
                ((Client)GameManager.Instance.Managers[Core.Managers.Client]).SendData((int)Events.InGame,(int)InGameTypes.Fire,JsonUtility.ToJson(new Vec3Packet(hit.point)));
                if(Input.GetMouseButton(1))
                {
                    bl.SetLine(ZoomFirePos.position, hit.point);
                }
                else
                    bl.SetLine(FirePos.position, hit.point);
                CurrentAmmo--;
            }
            return true;
        }
        else
        {
            Animator.SetTriggerReload(true);
            Animator.SetBoolReload(true);
            return false;
        }
    }
    
    private IEnumerator DelayCor(float delay)
    {
        isFire = false;
        yield return new WaitForSeconds(delay);
        isFire = true;
    }

    public override void Init(Transform root)
    {
        base.Init(root);
        firePos = transform.Find("firePos");
        CurrentAmmo = MaxAmmo = 30;
    }

    public override void Reload()
    {
        CurrentAmmo = MaxAmmo;
    }
}
