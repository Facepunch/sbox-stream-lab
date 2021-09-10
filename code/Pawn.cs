using Sandbox;

namespace TwitchLab
{
	public partial class Pawn : ModelEntity
	{
		public override void Spawn()
		{
			base.Spawn();

			EnableHideInFirstPerson = true;

			Tags.Add( "pawn" );

			SetModel( "models/light_arrow.vmdl" );
		}

		public override void BuildInput( InputBuilder input )
		{
			Host.AssertClient();

			input.ViewAngles += input.AnalogLook;
			input.ViewAngles = input.ViewAngles.Normal;
			input.ViewAngles.pitch = input.ViewAngles.pitch.Clamp( -90, 90 );
			input.ViewAngles.roll = 0;
			input.InputDirection = input.AnalogMove;
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			Rotation = Input.Rotation;
			EyeRot = Rotation;

			var maxSpeed = 500;
			if ( Input.Down( InputButton.Run ) ) maxSpeed = 1000;

			Velocity += Input.Rotation * new Vector3( Input.Forward, Input.Left, Input.Up ) * maxSpeed * 5 * Time.Delta;
			if ( Velocity.Length > maxSpeed ) Velocity = Velocity.Normal * maxSpeed;

			Velocity = Velocity.Approach( 0, Time.Delta * maxSpeed * 3 );

			Position += Velocity * Time.Delta;

			EyePos = Position;

			if ( IsServer && Input.Pressed( InputButton.Attack1 ) )
			{
				var tr = Trace.Ray( Input.Cursor.Origin, Input.Cursor.Origin + Input.Cursor.Direction * 5000 )
					.WorldOnly()
					.Run();

				if ( tr.Hit )
				{
					new Pickup
					{
						Position = tr.EndPos
					};
				}
			}
		}
	}
}
