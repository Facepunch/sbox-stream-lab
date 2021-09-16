using Sandbox;

namespace TwitchLab
{
	public partial class Game
	{
		[Stream.OnChat( "north" )]
		public static void OnNorth()
		{
			Log.Info( "NORTH!" );
		}

		[Event.Stream.Connected]
		public static void OnStreamConnected()
		{
			Stream.JoinChannel( Stream.Username );
		}

		[Event.Stream.Message]
		public static void OnStreamMessage( StreamChatMessage message )
		{
			Current.OnChatMessage( message.DisplayName, message.Message, message.Color );
		}

		[ClientRpc]
		void OnChatMessage( string name, string message, string color )
		{
			Hud.AddChatEntry( name, message, color );
		}

		[Event.Stream.Join]
		public static void OnStreamJoinEvent()
		{
			Log.Info( $"{Stream.OnChat.User} joined" );
		}

		[Event.Stream.Leave]
		public static void OnStreamLeaveEvent()
		{
			Log.Info( $"{Stream.OnChat.User} left" );

			Current.RemovePlayer( Stream.OnChat.User );
		}

		[Stream.OnChat( "!play" )]
		public static void OnStreamPlayCommand()
		{
			Current.AddPlayer( Stream.OnChat.User );
		}

		[Stream.OnChat( "!quit" )]
		public static void OnStreamQuitCommand()
		{
			Current.RemovePlayer( Stream.OnChat.User );
		}

		[Stream.OnChat( "jump" )]
		public static void OnStreamJumpCommand()
		{
			Current.JumpPlayer( Stream.OnChat.User );
		}

		[Stream.OnChat( "reset" )]
		public static void OnStreamResetCommand()
		{
			Current.ResetPlayer( Stream.OnChat.User );
		}

		[Stream.OnChat( "w" )] public static void OnStreamForwardCommand() => Current.MovePlayer( Stream.OnChat.User, Vector3.Forward );
		[Stream.OnChat( "a" )] public static void OnStreamLeftCommand() => Current.MovePlayer( Stream.OnChat.User, Vector3.Left );
		[Stream.OnChat( "s" )] public static void OnStreamBackwardCommand() => Current.MovePlayer( Stream.OnChat.User, Vector3.Backward );
		[Stream.OnChat( "d" )] public static void OnStreamRightCommand() => Current.MovePlayer( Stream.OnChat.User, Vector3.Right );

		private void AddPlayer( string user )
		{
			if ( !Players.ContainsKey( user ) )
			{
				var player = new Player
				{
					DisplayName = user,
				};

				player.RequestAvatar();

				MoveToSpawnpoint( player );

				player.SetupPhysics();

				Players.Add( user, player );
			}
		}

		private void RemovePlayer( string user )
		{
			if ( Players.TryGetValue( user, out var player ) )
			{
				Players.Remove( user );
				player?.Delete();
			}
		}

		private void ResetPlayer( string user )
		{
			if ( !Players.TryGetValue( user, out var player ) )
				return;

			MoveToSpawnpoint( player );
		}

		private void JumpPlayer( string user )
		{
			if ( !Players.TryGetValue( user, out var player ) )
				return;

			player.ApplyAbsoluteImpulse( Vector3.Up * (player.PhysicsGroup.Mass * 40.0f) );
		}

		private void MovePlayer( string user, Vector3 direction )
		{
			if ( !Players.TryGetValue( user, out var player ) )
				return;

			var rotation = Rotation.From( LocalClient.Pawn.Rotation.Angles().WithPitch( 0 ) );
			player.ApplyAbsoluteImpulse( rotation * direction * ( player.PhysicsGroup.Mass * 30.0f ) );
		}
	}
}
