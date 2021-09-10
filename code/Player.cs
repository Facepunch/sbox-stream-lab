using Sandbox;
using System.Runtime.Serialization;

namespace TwitchLab
{
	public partial class Player : ModelEntity
	{
		[Net] public string DisplayName { get; set; }
		private NameTag NameTag;

		public Player()
		{
			Tags.Add( "player" );
		}

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "models/citizen_props/crate01.vmdl" );
		}

		public override void ClientSpawn()
		{
			base.ClientSpawn();

			NameTag = Game.Current.Hud.NameTags.AddNameTag( this );
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			NameTag?.Delete();
			NameTag = null;
		}

		public void SetupPhysics()
		{
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		}

		public override void StartTouch( Entity other )
		{
			if ( !IsServer ) return;

			if ( other is Pickup pickup )
			{
				Scale = (Scale + 0.1f).Clamp( 1.0f, 5.0f );

				pickup.Delete();

				return;
			}
		}
	}
}
