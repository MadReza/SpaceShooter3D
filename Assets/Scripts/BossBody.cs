using UnityEngine;
using System.Collections;

public class BossBody : MonoBehaviour
{
    private Boss bossScript;

    void Start()
    {
        bossScript = transform.parent.gameObject.GetComponent<Boss>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet" && bossScript.TakeDamage && !bossScript.died)
        {
            bossScript.GotHit();
            Destroy(other.gameObject);
        }
    }
}
