using UnityEngine;

namespace RagdollCreatures
{
	public class SniperBullet : MonoBehaviour, IWeapon
	{
		#region Internal
		private int damage = 0;
		private WeaponType weaponType = WeaponType.Bullet;
		#endregion

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

		void OnCollisionEnter2D(Collision2D collider)
		{
			Destroy(gameObject);
		}
	}
}
