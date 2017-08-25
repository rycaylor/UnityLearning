using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public bool power = false;

	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdFire();
		}
		if (power) 
		{
			GetComponent<MeshRenderer> ().material.color = Color.green;

		}
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Power"))
		{
			other.gameObject.SetActive(false);
		
			power = true;

		}
		if (other.gameObject.CompareTag("Pack"))
		{
			other.gameObject.SetActive(false);

			var health = GetComponent<Health>();
			health.currentHealth += 50;

		}
	}
	[Command]
	void CmdFire()
	{
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		if (power) {
			bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * 25;
			bullet.GetComponent<Bullet> ().power = true; 
			power = false;
			GetComponent<MeshRenderer>().material.color = Color.blue;
			Debug.Log ("fuck you");
		} else {
			bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * 6;
		}
		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);        
	}
	public override void OnStartLocalPlayer()
	{
		GetComponent<MeshRenderer>().material.color = Color.blue;
	}
}
