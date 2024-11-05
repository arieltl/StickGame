using UnityEngine;

namespace RagdollCreatures
{
	public class Sniper : MonoBehaviour, IInteractable
	{
		#region Settings
		public Transform startPosition;
		public GameObject bulletPrefab;

		[Range(0, 100)]
		public int damage = 25;

		[Range(10.0f, 200.0f)]
		public float bulletSpeed = 60.0f;

		[Range(0.0f, 10000.0f)]
		public float recoil = 0.0f;

		[Range(0f, 3.0f)]
		public float shootDelay = 2.0f;
		private float lastShootTime;
		#endregion

		#region SoundEffects
		public AudioSource audioSource;
		public AudioClip shootSound;
		public AudioClip reloadSound;
		#endregion

		public void Start()
		{
			if (audioSource == null)
			{
				audioSource = GetComponent<AudioSource>();
			}
		}

		public void interact(GameObject parent)
		{
			if (Time.time >= lastShootTime + shootDelay)
			{
				Vector2 dir = startPosition.position - transform.position;
				float rotation = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
				audioSource.PlayOneShot(shootSound);
				
				Debug.DrawRay(startPosition.position, dir);

				GameObject bullet = Instantiate(bulletPrefab);
				Bullet bulletScript = bullet.GetComponent<Bullet>();
				if (null != bulletScript)
				{
					bulletScript.SetDamage(damage);
				}
				bullet.transform.position = startPosition.position;
				bullet.transform.rotation = startPosition.rotation;
				dir.Normalize();
				bullet.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;

				Rigidbody2D rigidbody = parent.GetComponent<Rigidbody2D>();
				if (null != rigidbody)
				{
					rigidbody.AddForce(-dir * recoil);
				}
				lastShootTime = Time.time;
			}
		}
	}
}
