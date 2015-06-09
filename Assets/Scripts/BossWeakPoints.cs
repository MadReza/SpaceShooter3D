using UnityEngine;
using System.Collections;

public class BossWeakPoints : MonoBehaviour
{
    public int health = 2;
    private Boss bossScript;

    void Start()
    {
        bossScript = transform.parent.gameObject.GetComponent<Boss>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("WeakPoints: " + other.tag + " " + !bossScript.TakeDamage + " " + !bossScript.died);
        if (other.tag == "Bullet" && !bossScript.TakeDamage && !bossScript.died)
        {
            health--;
            if (health <= 0)
            {
                health = 0;
                bossScript.WeakPointDown();
            }
            Destroy(other.gameObject);
        }
    }
}
