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
		
		
		private Vector3 aimDirection = new Vector3(1, -1, 0);

		public Color color;
		public int playerId = 0;
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
		public RagdollCreature creature;

		public GameObject root;
		public GameObject IK;
		GameManager gameMangager;
		private bool shouldRespawn = false;

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
			creature.OnRagdollLimbCollisionEnter2D.AddListener(OnRagdollLimbCollisionEnter2D);
			anim.Rebind();
			anim.Update(0f);
		}

		void Start() {
			gameMangager = FindObjectOfType<GameManager>();
			setControlls();
		}

		void OnDestroy() { }

		public void Update()
		{
			controller.Update();
			if (shouldRespawn)
			{
				Respawn();
				shouldRespawn = false;
			}
		}

		public void FixedUpdate()
		{
			controller.FixedUpdate();
		}

		public void OnStartgame() {
			if (!gameMangager.isGameRunning){
				gameMangager.StartGame();
			} else {
				var gameManager = FindObjectOfType<GameManager>();
				gameManager.ApplyDamage(playerId, 100);
			}


		}

		public void Respawn() {
			
			hasRespawned = true;
			creature.ActivateAllMuscles();			
			
			gameMangager.players[playerId].health = 100;

			var currentBody = transform.Find("PlayerBody").gameObject;
			
			DestroyImmediate(currentBody);
			creature.resetEvents();

			var newBody = Instantiate(playerPrefab, transform.position, transform.rotation,transform);

			newBody.name = "PlayerBody";
			
			
			CreateController();
			creature.isDead = false;
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
	        aimAction.performed += (InputAction.CallbackContext context) => {
		        var vec = context.ReadValue<Vector2>().normalized;
		        aimDirection = new Vector3(vec.x, vec.y, 0);
	        };

			var shootAction = playerInput.actions.FindAction("Shoot");
			shootAction.performed += interactScript.OnAttack;

			var placeAction = playerInput.actions.FindAction("Place");
			placeAction.performed += OnPlace;

			var pauseScript = FindFirstObjectByType<PauseMenu>();
			var pauseAction = playerInput.actions.FindAction("Pause");
			pauseAction.performed += pauseScript.onPause;
        }

		void OnPlace(InputAction.CallbackContext context) {
			var placeable = gameMangager.players[playerId].collectedItem;
			Debug.Log("Place " + placeable);
			if (placeable == null) return;
			Debug.Log("Place not null");
			var body = transform.Find("PlayerBody").gameObject;
			var root = body.transform.Find("Root").gameObject;
			var hip = root.transform.Find("Hip");
			Instantiate(placeable.Value.prefab, hip.position + aimDirection, Quaternion.identity);
			gameMangager.players[playerId].collectedItem = null;
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			controller.OnMove(context);
		}

		public void OnRagdollLimbCollisionEnter2D(object sender, Collision2D col)
		{
		}

		public void OnRagdollLimbCollisionExit2D(object sender, Collision2D col)
		{
		}

		public void OnTriggerEnter2D(Collider2D col)
		{
		}

		public void OnTriggerExit2D(Collider2D col)
		{
		}

		public bool UseNewInputSystem()
		{
			return controller.UseNewInputSystem();
		}

		public void SetUseNewInputSystem(bool useNewInputSystem)
		{
			controller.SetUseNewInputSystem(useNewInputSystem);
		}

		public void DelayRespawn()
		{
			shouldRespawn = true;
		}
	}
}
