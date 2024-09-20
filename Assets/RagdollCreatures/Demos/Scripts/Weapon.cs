using UnityEngine;

namespace RagdollCreatures
{
	public enum WeaponType { Harmless, Bullet, Meele }

	public interface IWeapon
	{
		public WeaponType GetWeaponType();

		public void SetWeaponType(WeaponType weaponType);

		public int GetDamage();

		public void SetDamage(int damage);
	}
}
