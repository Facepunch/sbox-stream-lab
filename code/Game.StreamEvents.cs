using Sandbox;

namespace TwitchLab
{
	public partial class Game
	{
		[Stream.OnConnected]
		public static void OnStreamConnected()
		{
			if ( !Host.IsServer )
				return;

			Stream.JoinChannel( Stream.Username );
		}

		[Stream.OnMessage]
		public static void OnStreamMessage( StreamChatMessage message )
		{
			if ( !Host.IsServer )
				return;

			Current.OnChatMessage( message.DisplayName, message.Message, message.Color );
		}

		[Stream.OnJoin]
		public static void OnStreamJoinEvent( string user )
		{
			if ( !Host.IsServer )
				return;

			Log.Info( $"{user} joined" );
		}

		[Stream.OnLeave]
		public static void OnStreamLeaveEvent( string user )
		{
			if ( !Host.IsServer )
				return;

			Log.Info( $"{user} left" );

			Current.RemovePlayer( user );
		}

		[Stream.OnChat( "!play" )]
		public static void OnStreamPlayCommand()
		{
			if ( !Host.IsServer )
				return;

			Current.AddPlayer( Stream.OnChat.User );
		}

		[Stream.OnChat( "!quit" )]
		public static void OnStreamQuitCommand()
		{
			if ( !Host.IsServer )
				return;

			Current.RemovePlayer( Stream.OnChat.User );
		}

		[Stream.OnChat( "jump" )]
		public static void OnStreamJumpCommand()
		{
			if ( !Host.IsServer )
				return;

			Current.JumpPlayer( Stream.OnChat.User );
		}

		[Stream.OnChat( "reset" )]
		public static void OnStreamResetCommand()
		{
			if ( !Host.IsServer )
				return;

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
			if ( !Host.IsServer )
				return;

			if ( !Players.TryGetValue( user, out var player ) )
				return;

			var rotation = Rotation.From( LocalClient.Pawn.Rotation.Angles().WithPitch( 0 ) );
			player.ApplyAbsoluteImpulse( rotation * direction * ( player.PhysicsGroup.Mass * 30.0f ) );
		}
	}
}
