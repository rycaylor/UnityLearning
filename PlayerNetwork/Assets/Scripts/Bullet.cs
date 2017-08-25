using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public bool power;
	public void OnCollisionEnter(Collision collision)
	{
		var hit = collision.gameObject;
		var health = hit.GetComponent<Health>();
		var dmg =10;
		if (power) {
			dmg = 90;
		}
		if (health  != null)
		{
			health.TakeDamage(dmg);
		}

		Destroy(gameObject);
	}

}
