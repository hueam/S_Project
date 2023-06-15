using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class Rifle : Gun
{
    public override bool Fire()
    {
        if (CurrentAmmo > 0)
        {
            if (isFire == true)
            {
                BulletLine bl = Instantiate(bulletLine, Vector3.zero, Quaternion.identity);
                StartCoroutine(DelayCor(shotDelay));
                Ray ray = Define.MainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 2000f, hitLayer))
                {
                    if (hit.transform.TryGetComponent<OtherPlayer>(out OtherPlayer player))
                    {
                        player.PlayerHealthCompo.HitDamage(damage);
                        ((Client)GameManager.Instance.Managers[Managers.Client]).SendData((int)Events.InGame, (int)InGameTypes.Hit, JsonUtility.ToJson(new DamagePacket(player.socketID, damage)));
                    }
                }
                bl.SetLine(firePos.position, hit.point);
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

    public override void Init()
    {
        base.Init();
        firePos = transform.Find("firePos");
        CurrentAmmo = MaxAmmo = 30;
    }

    public override void Reload()
    {
        CurrentAmmo = MaxAmmo;
    }
}
