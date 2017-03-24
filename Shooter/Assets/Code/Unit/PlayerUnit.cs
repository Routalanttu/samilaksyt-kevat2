using System;
using TAMKShooter.Data;
using UnityEngine;
using TAMKShooter.Configs;

namespace TAMKShooter
{
	public class PlayerUnit : UnitBase
	{
		public enum UnitType
		{
			None = 0,
			Fast = 1,
			Balanced = 2,
			Heavy = 3
		}

		[SerializeField] private UnitType _type;

		public UnitType Type { get { return _type; } }
		public PlayerData Data { get; private set; }
		public Vector3 Spawnpoint { get; private set; }

		private float _invulnerabilityCooldown;
		private MeshRenderer _visiblePart;

		public override int ProjectileLayer
		{
			get
			{
				return LayerMask.NameToLayer ( Config.PlayerProjectileLayerName );
			}
		}

		public void Init( PlayerData playerData )
		{
			Spawnpoint = transform.position;
			InitRequiredComponents();
			Data = playerData;
		}

		protected override void Die ()
		{
			// TODO:
			// Instantiate explosion effect
			// Play sound
			if (Data.Lives > 1) {
				Data.Lives--;
				// Restore health to 100 (as the minimum is clamped to 0, no deviation can happen):
				TakeDamage (-100);
				transform.position = Spawnpoint;
				_invulnerabilityCooldown = 0.75f;
			} else {
				gameObject.SetActive (false);
				base.Die ();
			}
		}

		public void HandleInput ( Vector3 input, bool shoot )
		{
			Mover.MoveToDirection ( input );
			if(shoot)
			{
				Weapons.Shoot (ProjectileLayer);
			}
		}

		void OnTriggerEnter (Collider collie) {
			if (_invulnerabilityCooldown <= 0f) {
				// Make the unit invulnerable for a bit even after a non-lethal hit for added effect:
				_invulnerabilityCooldown = 0.15f;
				TakeDamage (50);
			}

			IHealth damageReceiver = collie.gameObject.GetComponentInChildren<Health> ();

			if (null != damageReceiver) {
				damageReceiver.TakeDamage (10);
			}
		}

		void Awake () {
			_visiblePart = GetComponent<MeshRenderer> ();
		}

		void Update () {
			if (_invulnerabilityCooldown >= 0f) {
				// Flashing:
				if (_invulnerabilityCooldown % 0.1f > 0.05f) {
					_visiblePart.enabled = false;
				} else {
					_visiblePart.enabled = true;
				}

				_invulnerabilityCooldown -= Time.deltaTime;
			} else {
				_visiblePart.enabled = true;
			}
		}
	}
		
}
