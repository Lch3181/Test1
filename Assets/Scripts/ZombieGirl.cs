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
    }

    //override Die to remove audio
    protected override void Die()
    {
        base.Die();
        audioActive.Stop();
        audioPassive.Stop();
    }

    //override attack end to add audio
    protected override void AttackEnd()
    {
        audioActive.clip = null;
        base.AttackEnd();
        if(InRange())
        {
            audioActive.PlayOneShot(audioClipActive[0]);
        }
            
    }
}
