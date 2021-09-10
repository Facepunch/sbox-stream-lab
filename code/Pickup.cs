using Sandbox;
using System;

namespace TwitchLab
{
	public partial class Pickup : ModelEntity
	{
		private ModelEntity Visual;
		private float timeOffset;

		public Pickup()
		{
			SetupPhysicsFromSphere( PhysicsMotionType.Keyframed, Vector3.Zero, 32.0f );
			CollisionGroup = CollisionGroup.Trigger;
		}

		public override void ClientSpawn()
		{
			base.ClientSpawn();

			Visual = new ModelEntity();
			Visual.SetParent( this );
			Visual.SetModel( "models/citizen_props/coin01.vmdl" );
			Visual.LocalPosition = Vector3.Zero;
			Visual.LocalRotation = Rotation.FromPitch( 90.0f );

			timeOffset = Rand.Float( 0.0f, 5.0f );
		}

		[Event.Frame]
		protected void OnFrame()
		{
			if ( Visual == null )
				return;

			var time = (Time.Now + timeOffset) % (MathF.PI * 2.0f);
			var delta = (MathF.Sin( time * 5.0f ) + 1.0f) * 0.5f;

			Visual.LocalPosition = Vector3.Up * 25.0f * delta;
			Visual.LocalRotation = Rotation.From( 90.0f, time.RadianToDegree() * 5.0f, 0.0f );
		}
	}
}
