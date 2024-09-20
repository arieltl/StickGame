using System;
using System.Collections;
using UnityEngine;

namespace RagdollCreatures
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Sword : MonoBehaviour, IInteractable, IWeapon
	{
		#region Settings
		[Range(0, 100)]
		private int damage = 10;

		[Range(0.0f, 5000.0f)]
		public float hitForce = 60.0f;

		[Range(0f, 3.0f)]
		public float hitDelay = 2.0f;
		private float lastHitTime;
		#endregion

		#region Internal
		private new Rigidbody2D rigidbody;
		private WeaponType weaponType = WeaponType.Harmless;
		#endregion

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody2D>();
		}

		public WeaponType GetWeaponType()
		{
			return weaponType;
		}

		public void SetWeaponType(WeaponType weaponType)
		{
			this.weaponType = weaponType;
		}

		public int GetDamage()
		{
			return damage;
		}

		public void SetDamage(int damage)
		{
			this.damage = damage;
		}

		public void interact(GameObject parent)
		{
			if (Time.time >= lastHitTime + hitDelay)
			{
				Rigidbody2D parentRigidbody = parent.GetComponent<Rigidbody2D>();
				if (null != parentRigidbody)
				{
					float angle = parentRigidbody.rotation % 360;
					if (angle > 180)
					{
						angle = (360 - angle) * -1;
					} else if (angle < -180)
					{
						angle = 360 + angle;
					}

					Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
					parentRigidbody.AddForce(q * Vector2.down * hitForce);
				}
				StartCoroutine(Hit());
				
				lastHitTime = Time.time;
			}
		}

		IEnumerator Hit()
		{
			weaponType = WeaponType.Meele;
			yield return new WaitForSeconds(0.2f);
			weaponType = WeaponType.Harmless;
		}
	}
}
