using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RagdollCreatures
{
	public class HealthSystem : MonoBehaviour
	{
		[Serializable]
		public class HealthSystemDeathEvent : UnityEvent<GameObject> { }

		#region Settings
		[Range(0.0f, 5000.0f)]
		public float minMeeleForce = 1500.0f;
		#endregion

		#region Events
		public HealthSystemDeathEvent OnDeath;
		#endregion

		#region Internal
		private List<RagdollCreature> ragdollCreatures = new List<RagdollCreature>();
		#endregion

		void Update()
		{
			foreach (RagdollCreature ragdollCreature in FindObjectsOfType<RagdollCreature>())
			{
				if (!ragdollCreatures.Contains(ragdollCreature))
				{
					ragdollCreature.OnRagdollLimbCollisionEnter2D.AddListener(doHitDamage);
					ragdollCreatures.Add(ragdollCreature);
				}
			}
		}

		public void doHitDamage(RagdollLimb limb, Collision2D col)
		{
			RagdollCreature ragdollCreature = limb.GetRootParent();
			if (null != ragdollCreature)
			{
				if (ragdollCreature.isDead)
				{
					return;
				}

				Health health = ragdollCreature.GetComponent<Health>();
				if (null != health)
				{
					IWeapon weapon = col.collider.gameObject.GetComponent<IWeapon>();
					if (null != weapon && weapon.GetWeaponType() != WeaponType.Harmless)
					{
						int newHealth = health.GetHealth() - weapon.GetDamage();
						if (newHealth <= 0)
						{
							health.SetHealth(0);
							ragdollCreature.DeactivateAllMuscles();
							ragdollCreature.isDead = true;
							OnDeath.Invoke(ragdollCreature.gameObject);

							foreach (RagdollLimb ragdollLimb in ragdollCreature.ragdollLimbs)
							{
								Joint2D joint = ragdollLimb.GetComponent<Joint2D>();
								if (null != joint)
								{
									joint.enabled = false;
								}
								ragdollLimb.rigidbody.AddForce(new Vector2(
									UnityEngine.Random.Range(-300, 300),
									UnityEngine.Random.Range(-300, 300)));
							}
						}
						else
						{
							health.SetHealth(newHealth);
						}
					}
				}
			}
		}
	}
}
