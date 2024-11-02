using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RagdollCreatures
{
	/// <summary>
	/// Generic controller for RagdollLimbs.
	/// 
	/// Contains many adjustment possibilities to cover as many use cases as possible.
	/// Play around with the settings in the inspector to get a feeling for what they do.
	/// For a more detailed description look at the documentation, code comments or video tutorials.
	/// </summary>
	[RequireComponent(typeof(RagdollCreature))]
	public class RagdollCreatureController : MonoBehaviour, IRagdollCreatureController
	{
		bool hasRespawned = false;

		public Color color;
		
		public GameObject playerPrefab;
		#region Movement
		public RagdollCreatureMovement movement;
		public IRagdollCreatureController controller;
		#endregion

		#region Effects
		[Header("Effects")]
		public bool playWalkEffect = false;
		public ParticleSystem walkParticleSystem;

		public bool playJumpEffect = false;
		public ParticleSystem jumpParticleSystem;
		#endregion

		#region Internal
		private RagdollCreature creature;

		public GameObject root;
		public GameObject IK;
		SpawnManager spawnMangager;

		#endregion

		void Awake() {
			CreateController();
			DontDestroyOnLoad(this.gameObject);

		}
		
		public void setColor() {
			foreach (SpriteRenderer renderer in creature.GetComponentsInChildren<SpriteRenderer>())
			{
				if (null != renderer && null != renderer.GetComponent<RagdollLimb>())
				{
					renderer.color = color;
				}
			}
		}

		void CreateController() {
			creature = GetComponent<RagdollCreature>();
			var anim = GetComponentInChildren<Animator>();
			creature.animator = anim;
			creature.Initialize();
			controller = new AbstractRagdollCreatureController(creature, movement);
			
			anim.Rebind();
			anim.Update(0f);
		}

		void Start() {
			spawnMangager = FindObjectOfType<SpawnManager>();
		}

		void OnDestroy() { }

		public void Update()
		{
			controller.Update();
		}

		public void FixedUpdate()
		{
			controller.FixedUpdate();
		}

		public void OnStartgame()
		{
			// if (spawnMangager != null) {
			// 	spawnMangager.StartGame();
			// 	spawnMangager = null;
			// }
			
			
			resetControlls();
			// if (hasRespawned) {
			// 	return;
			// }
			hasRespawned = true;
			creature = GetComponent<RagdollCreature>();
			

			var currentBody = transform.Find("PlayerBody").gameObject;
			
			DestroyImmediate(currentBody);
			creature.resetEvents();

			var newBody = Instantiate(playerPrefab, transform.position, transform.rotation,transform);

			newBody.name = "PlayerBody";
			
			
			CreateController();
			var disableColliders = GetComponent<DisableCollider2D>();
			disableColliders.resetColliders();
			setControlls();
			setColor();

			
		}
        void resetControlls(){
            var playerInput = GetComponent<PlayerInput>();
            var interactAction = playerInput.actions.FindAction("Interact");
            var interactScript = GetComponentInChildren<Interact>();
            interactAction.performed -= interactScript.OnInteract;
            
            var aimAction = playerInput.actions.FindAction("Aim");
            var followMouse = GetComponentInChildren<FollowMouse>();
            // aimAction.performed -= followMouse.OnMouseMove;
            aimAction.performed -= interactScript.OnAim;
        }

        void setControlls() {
	        var playerInput = GetComponent<PlayerInput>();
	        var interactAction = playerInput.actions.FindAction("Interact");
	        var interactScript = GetComponentInChildren<Interact>();
	        interactScript.root = gameObject;
	        interactAction.performed += interactScript.OnInteract;
	        
	        var aimAction = playerInput.actions.FindAction("Aim");
	        var followMouse = GetComponentsInChildren<FollowMouse>();
	        Debug.Log(followMouse.Length);
	        aimAction.performed += followMouse[0].OnMouseMove;
	        aimAction.performed += interactScript.OnAim;
	        
        }

		public void OnMove(InputAction.CallbackContext context)
		{
			controller.OnMove(context);
		}

		public void OnRagdollLimbCollisionEnter2D(object sender, Collision2D col)
		{
			OnRagdollLimbCollisionEnter2D(sender, col);
		}

		public void OnRagdollLimbCollisionExit2D(object sender, Collision2D col)
		{
			OnRagdollLimbCollisionExit2D(sender, col);
		}

		public void OnTriggerEnter2D(Collider2D col)
		{
			OnTriggerEnter2D(col);
		}

		public void OnTriggerExit2D(Collider2D col)
		{
			OnTriggerExit2D(col);
		}

		public bool UseNewInputSystem()
		{
			return controller.UseNewInputSystem();
		}

		public void SetUseNewInputSystem(bool useNewInputSystem)
		{
			controller.SetUseNewInputSystem(useNewInputSystem);
		}
	}
}
