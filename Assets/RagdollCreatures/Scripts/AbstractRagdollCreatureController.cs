using UnityEngine;
using UnityEngine.InputSystem;

namespace RagdollCreatures
{
	public class AbstractRagdollCreatureController : IRagdollCreatureController
	{
		#region Internal
		private RagdollCreature ragdollCreature;
		private RagdollCreatureMovement movement;
		private float lastJumpTime;
		private Vector2 moveVector;
		#endregion

		public AbstractRagdollCreatureController(RagdollCreature ragdollCreature, RagdollCreatureMovement movement)
		{
			this.ragdollCreature = ragdollCreature;
			this.movement = movement;
			if (null == movement)
			{
				this.movement = ScriptableObject.CreateInstance(typeof(RagdollCreatureMovement)) as RagdollCreatureMovement;
			}
			this.movement.useNewInputSystem = InputSystemSwitcher.UseNewInputSystem;
		}

		public void Update()
		{
			if (ragdollCreature.isDead)
			{
				return;
			}

			// Animations
			UpdateAnimations();
		}

		public void FixedUpdate()
		{
			if (ragdollCreature.isDead)
			{
				foreach (RagdollLimb limb in ragdollCreature.ragdollLimbs)
				{
					limb.rigidbody.gravityScale = 1;
				}
				return;
			}

			Vector2 horizontalMove = GetHorizontalMovementInput();
			float speed = ragdollCreature.isGrounded ? movement.movementSpeed : movement.jumpMovementSpeed;

			Rigidbody2D centerOfMass = ragdollCreature.centerOfMass?.rigidbody;
			if (null != centerOfMass)
			{
				// Reduce Y velocity of limbs for smoother walking
				// To achieve a similar effect you could also reduce the friction from the floor.
				// You could also adjust the groundGravityScale.
				if (movement.isSmootherWalking && ragdollCreature.isGrounded)
				{
					foreach (RagdollLimb limb in ragdollCreature.ragdollLimbs)
					{
						if (limb.isMuscleActive && limb.isActiveGroundDetection)
						{
							Rigidbody2D limbRigidbody = limb.rigidbody;
							float velocityY = 0.0f;
							float smoothY = Mathf.SmoothDamp(limbRigidbody.velocity.y, 0, ref velocityY, Time.fixedDeltaTime);
							limbRigidbody.velocity = new Vector2(limbRigidbody.velocity.x, smoothY);
						}
					}
				}

				// Adjust gravity scaling.
				// Jump with normal gravity (gravityScale == 1) is more real but less fun :)
				foreach (RagdollLimb limb in ragdollCreature.ragdollLimbs)
				{
					if (!ragdollCreature.isGrounded)
					{
						if (limb.rigidbody.velocity.y < 0)
						{
							limb.rigidbody.gravityScale = movement.fallGravityScale;
						}
						else if (limb.rigidbody.velocity.y > 0)
						{
							limb.rigidbody.gravityScale = movement.jumpGravityScale;
						}
					}
					else
					{
						limb.rigidbody.gravityScale = movement.groundGravityScale;
					}
				}

				bool directionSwitch = centerOfMass.velocity.x > 0 && horizontalMove.x < 0
					|| centerOfMass.velocity.x < 0 && horizontalMove.x > 0;

				if (movement.isRemoveXVelocityBeforeDirectionSwitch && directionSwitch)
				{
					foreach (RagdollLimb limb in ragdollCreature.ragdollLimbs)
					{
						limb.rigidbody.velocity = new Vector2(0, centerOfMass.velocity.y);
					}
				}

				// Move
				centerOfMass.velocity = Vector2.Lerp(
					centerOfMass.velocity,
					new Vector2(horizontalMove.x * speed, centerOfMass.velocity.y),
					Time.deltaTime * movement.movementLerpFactor);

				if (directionSwitch && ragdollCreature.isGrounded)
				{
					// TODO: PlayWalkEffect();
				}

				// Only jump if Creature is grounded and the jump delay is over

				float jumpMovement = GetJumpMovementInput();

				if (jumpMovement > 0 && ragdollCreature.isGrounded
					&& Time.time >= lastJumpTime + movement.jumpDelay)
				{
					if (movement.isRemoveYVelocityBeforeJumping)
					{
						foreach (RagdollLimb limb in ragdollCreature.ragdollLimbs)
						{
							limb.rigidbody.velocity = new Vector2(centerOfMass.velocity.x, 0);
						}
					}

					// The actual jump
					centerOfMass.AddForce(new Vector2(horizontalMove.x, Vector2.up.y) * movement.jumpForce, ForceMode2D.Impulse);
					lastJumpTime = Time.time;
					// TODO: PlayJumpEffect();
				}
			}
		}

		protected virtual float GetJumpMovementInput()
		{
			float jumpMovement;
			if (movement.useNewInputSystem)
			{
				jumpMovement = moveVector.y;
			}
			else
			{
				jumpMovement = Input.GetAxis("Vertical");
			}

			return jumpMovement;
		}

		protected virtual Vector2 GetMovementInput()
		{
			Vector2 move;
			if (movement.useNewInputSystem)
			{
				move = moveVector;
			}
			else
			{
				move = new Vector2(
					Input.GetAxis("Horizontal"),
					Input.GetAxis("Vertical"));
			}

			return move;
		}

		protected virtual Vector2 GetHorizontalMovementInput()
		{
			return new Vector2(GetMovementInput().x, 0);
		}

		protected virtual Vector2 GetVertivalMovementInput()
		{
			return new Vector2(0, GetMovementInput().y);
		}

		void UpdateAnimations()
		{
			Vector2 move = GetMovementInput();
			ragdollCreature.PlayWalkAnimation(move);
		}

		//void PlayWalkEffect(RagdollCreatureMovement movement)
		//{
		//	if (null != movement.walkParticleSystem && playWalkEffect && !walkParticleSystem.isPlaying)
		//	{
		//		walkParticleSystem.Play();
		//	}
		//}

		//void PlayJumpEffect()
		//{
		//	if (null != jumpParticleSystem && playJumpEffect && !jumpParticleSystem.isPlaying)
		//	{
		//		jumpParticleSystem.Play();
		//	}
		//}

		/// <summary>
		/// Get the move vector from the InputActions.
		/// </summary>
		/// <param name="context"></param>
		public void OnMove(InputAction.CallbackContext context)
		{
			moveVector = context.ReadValue<Vector2>();
		}

		public void OnRagdollLimbCollisionEnter2D(object sender, Collision2D col)
		{
			//Debug.Log("OnRagdollLimbCollisionEnter2D: " + col.ToString());
		}

		public void OnRagdollLimbCollisionExit2D(object sender, Collision2D col)
		{
			//Debug.Log("OnRagdollLimbCollisionExit2D: " + col.ToString());
		}

		public void OnTriggerEnter2D(Collider2D col)
		{
			//Debug.Log("OnTriggerEnter2D: " + col.ToString());
		}

		public void OnTriggerExit2D(Collider2D col)
		{
			//Debug.Log("OnTriggerExit2D: " + col.ToString());
		}

		public bool UseNewInputSystem()
		{
			return movement.useNewInputSystem;
		}

		public void SetUseNewInputSystem(bool useNewInputSystem)
		{
			movement.useNewInputSystem = useNewInputSystem;
		}
	}
}
