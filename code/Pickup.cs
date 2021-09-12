using Sandbox;
using System;

namespace TwitchLab
{
	public partial class Pickup : ModelEntity
	{
		private ModelEntity VisualEntity;
		private float TimeOffset;

		public Pickup()
		{
			SetupPhysicsFromSphere( PhysicsMotionType.Keyframed, Vector3.Zero, 32.0f );
			CollisionGroup = CollisionGroup.Trigger;
		}

		public override void ClientSpawn()
		{
			base.ClientSpawn();

			VisualEntity = new ModelEntity();
			VisualEntity.SetParent( this );
			VisualEntity.SetModel( "models/citizen_props/coin01.vmdl" );
			VisualEntity.LocalPosition = Vector3.Zero;
			VisualEntity.LocalRotation = Rotation.FromPitch( 90.0f );
			VisualEntity.EnableAllCollisions = false;

			TimeOffset = Rand.Float( 0.0f, 5.0f );
		}

		[Event.Frame]
		protected void OnFrame()
		{
			if ( VisualEntity == null )
				return;

			var time = (Time.Now + TimeOffset) % (MathF.PI * 2.0f);
			var delta = (MathF.Sin( time * 5.0f ) + 1.0f) * 0.5f;

			VisualEntity.LocalPosition = Vector3.Up * 25.0f * delta;
			VisualEntity.LocalRotation = Rotation.From( 90.0f, time.RadianToDegree() * 5.0f, 0.0f );
		}
	}
}
