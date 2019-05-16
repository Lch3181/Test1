using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    public float Speed;
    public float maxDist;
    private float lifeTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += 1 * Time.deltaTime;

        if (lifeTime >= maxDist)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().takeDamage(Damage);
        }
        Destroy(this.gameObject);
    }
}
