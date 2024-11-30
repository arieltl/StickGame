using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace RagdollCreatures
{
	public class PlayerInputPosition : MonoBehaviour
	{
		public Transform[] spawnPositions;
		public Color[] colors;
		public GameObject TargetGroupManager;

		public void OnPlayerJoined(PlayerInput playerInput)
        {
            SetPlayerSpawnPoint(playerInput);
            FixPlayerColor(playerInput);
			AddPlayerProxyToTargetGroup(playerInput);
        }

        private void FixPlayerColor(PlayerInput playerInput)
        {
            if (SceneManager.GetActiveScene().name == "StartGame")
            {
                RagdollCreature ragdollCreature = playerInput.gameObject.GetComponent<RagdollCreature>();
                if (ragdollCreature != null)
                {
                    int colorIndex = Random.Range(0, colors.Length);
                    Color newColor = colors[colorIndex];
                    var ragdollCreatureController = playerInput.gameObject.GetComponent<RagdollCreatureController>();
                    ragdollCreatureController.color = newColor;
                    foreach (SpriteRenderer renderer in ragdollCreature.GetComponentsInChildren<SpriteRenderer>())
                    {
                        if (null != renderer && null != renderer.GetComponent<RagdollLimb>())
                        {
                            renderer.color = newColor;
                        }
                    }
                }
            }
        }

		private void AddPlayerProxyToTargetGroup(PlayerInput playerInput)
		{		
			if (TargetGroupManager != null) 
			{
				Transform playerProxy = playerInput.transform.Find("PlayerLocationProxy");
				TargetGroupManager.GetComponent<DynamicTargetGroup>().AddPlayer(playerProxy.gameObject);
				Debug.Log("Added Proxy to Target Group");
			}
		}

        private void SetPlayerSpawnPoint(PlayerInput playerInput)
        {
            int index = Random.Range(0, spawnPositions.Length);
            playerInput.gameObject.transform.position = spawnPositions[index].position;
            Rigidbody2D rigidbody2D = playerInput.gameObject.GetComponent<Rigidbody2D>();
            if (null != rigidbody2D)
            {
                rigidbody2D.position = spawnPositions[index].position;
            }
        }
    }
}
