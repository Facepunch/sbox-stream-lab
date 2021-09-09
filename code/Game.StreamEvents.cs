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
		}

		[Event.Stream.Message]
		public static void OnStreamMessage( ChatMessage message )
		{
			Log.Info( $"{message.DisplayName}: {message.Message} {message.Color}" );
		}

		[Event.Stream.Join]
		public static void OnStreamJoinCommand( string user )
		{
			Log.Info( $"{user} joined" );
		}

		[Event.Stream.Leave]
		public static void OnStreamLeaveCommand( string user )
		{
			Log.Info( $"{user} left" );
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

			player.ApplyAbsoluteImpulse( Vector3.Up * 500 );
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
			player.ApplyAbsoluteImpulse( rotation * direction * 500.0f );
		}
	}
}
