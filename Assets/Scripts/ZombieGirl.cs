using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGirl : Enemy
{
    public AudioClip []audioClipActive;
    public AudioClip []audioClipPassive;
    public AudioSource audioActive;
    public AudioSource audioPassive;
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
    }

    //override Animation to add audio
    protected override void Animation()
    {
        if(animator.IsInTransition(0))
        {
            audioActive.Stop();
            audioPassive.Stop();
        }
        base.Animation();
        if (velocity>0 && !audioActive.isPlaying)
        {
            audioActive.clip = audioClipActive[1];
            audioActive.Play();
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            audioActive.pitch = 1F;
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            audioActive.pitch = 1.8F;
        }
    }

    //override attack end to add audio
    protected override void AttackEnd()
    {
        audioActive.clip = null;
        base.AttackEnd();
        if(inRange())
        {
            audioActive.PlayOneShot(audioClipActive[0]);
        }
            
    }
}
