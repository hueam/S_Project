using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudio : AudioPlayer
{
    public AudioClip ShootBulletclip = null, OutOfBulletClip = null, ReloadClip = null;

    public void PlayShootSound()
    {
        PlayWithVariablePitch(ShootBulletclip);
    }

    public void PlayOutOfBulletSound()
    {
        PlayWithBasePitch(OutOfBulletClip);
    }

    public void PlayReloadSound()
    {
        PlayWithBasePitch(ReloadClip);
    }

}
