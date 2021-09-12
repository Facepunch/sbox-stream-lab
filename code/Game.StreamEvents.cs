using Sandbox;
using Sandbox.Streaming;

namespace TwitchLab
{
	public partial class Game
	{
		[Event.Stream.Connected]
		public static void OnStreamConnected()
		{
			Log.Info( $"Connected to stream" );

			StreamClient.JoinChannel( StreamClient.Username );
		}

		[Event.Stream.Message]
		public static void OnStreamMessage( ChatMessage message )
		{
			Log.Info( $"{message.DisplayName}: {message.Message} {message.Color}" );

			Current.OnChatMessage( message.DisplayName, message.Message, message.Color );
		}

		[ClientRpc]
		void OnChatMessage( string name, string message, string color )
		{
			Hud.AddChatEntry( name, message, color );
		}

		[Event.Stream.Join]
		public static void OnStreamJoinCommand( string user )
		{
			Log.Info( $"{user} joined" );
		}

		[Event.Stream.Leave]
		public void OnStreamLeaveCommand( string user )
		{
			Log.Info( $"{user} left" );

			if ( Players.TryGetValue( user, out var player ) )
			{
				Players.Remove( user );
				player?.Delete();
			}
		}

		[Event.Stream.Command( "play" )]
		public void OnStreamPlayCommand( string user )
		{
			if ( !Players.ContainsKey( user ) )
			{
				var player = new Player
				{
					DisplayName = user,
				};

				MoveToSpawnpoint( player );

				player.SetupPhysics();

				Players.Add( user, player );
			}
		}

		[Event.Stream.Command( "quit" )]
		public void OnStreamQuitCommand( string user )
		{
			if ( Players.TryGetValue( user, out var player ) )
			{	
				Players.Remove( user );

				if ( player != null )
				{
					player.Delete();
				}
			}
		}

		[Event.Stream.Command( "jump" )]
		public void OnStreamJumpCommand( string user )
		{
			if ( !Players.TryGetValue( user, out var player ) )
				return;

			player.ApplyAbsoluteImpulse( Vector3.Up * (player.PhysicsGroup.Mass * 40.0f) );
		}

		[Event.Stream.Command( "reset" )]
		public void OnStreamResetCommand( string user )
		{
			if ( !Players.TryGetValue( user, out var player ) )
				return;

			MoveToSpawnpoint( player );
		}

		[Event.Stream.Command( "w" )] public void OnStreamForwardCommand( string user ) => MovePlayer( user, Vector3.Forward );
		[Event.Stream.Command( "a" )] public void OnStreamLeftCommand( string user ) => MovePlayer( user, Vector3.Left );
		[Event.Stream.Command( "s" )] public void OnStreamBackwardCommand( string user ) => MovePlayer( user, Vector3.Backward );
		[Event.Stream.Command( "d" )] public void OnStreamRightCommand( string user ) => MovePlayer( user, Vector3.Right );

		private void MovePlayer( string user, Vector3 direction )
		{
			if ( !Players.TryGetValue( user, out var player ) )
				return;

			var rotation = Rotation.From( LocalClient.Pawn.Rotation.Angles().WithPitch( 0 ) );
			player.ApplyAbsoluteImpulse( rotation * direction * ( player.PhysicsGroup.Mass * 30.0f ) );
		}
	}
}
