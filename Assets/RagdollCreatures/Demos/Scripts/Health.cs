using UnityEngine;

namespace RagdollCreatures
{
	public class Health : MonoBehaviour
	{
		public int health;

		public int GetHealth()
		{
			return health;
		}

		public void SetHealth(int health)
		{
			this.health = health;
		}
	}
}
