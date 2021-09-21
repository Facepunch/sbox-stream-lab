using Sandbox;
using System;
using System.Linq;

namespace TwitchLab
{
	public partial class Game
	{
		/// <summary>
		/// Event called when a chat command comes in
		/// </summary>
		public class OnChatCommand : LibraryMethod
		{
			public string TargetName { get; set; }

			public static string User { get; internal set; }

			public OnChatCommand( string targetName )
			{
				TargetName = targetName;
			}
		}

		[Event.Streamer.ChatMessage]
		public static void OnStreamMessage( StreamChatMessage message )
		{
			if ( !Host.IsClient )
				return;

			var splits = message.Message.Split( ' ', StringSplitOptions.RemoveEmptyEntries );
			if ( splits.Length > 0 )
			{
				var attribute = Library.GetAttributes<OnChatCommand>()
					.Where( x => string.Equals( x.TargetName, splits[0], StringComparison.OrdinalIgnoreCase ) )
					.FirstOrDefault();

				OnChatCommand.User = message.Username;
				attribute?.InvokeStatic();
				OnChatCommand.User = null;
			}

			Current.OnChatMessage( message.DisplayName, message.Message, message.Color );
		}

		[Event.Streamer.JoinChat]
		public static void OnStreamJoinEvent( string user )
		{
			if ( !Host.IsClient )
				return;

			Log.Info( $"{user} joined" );
		}

		[Event.Streamer.LeaveChat]
		public static void OnStreamLeaveEvent( string user )
		{
			if ( !Host.IsClient )
				return;

			Log.Info( $"{user} left" );

			RemovePlayerCommand( user );
		}

		[OnChatCommand( "!play" )]
		public static void OnStreamPlayCommand()
		{
			if ( !Host.IsClient )
				return;

			AddPlayerCommand( OnChatCommand.User );
		}

		[OnChatCommand( "!quit" )]
		public static void OnStreamQuitCommand()
		{
			if ( !Host.IsClient )
				return;

			RemovePlayerCommand( OnChatCommand.User );
		}

		[OnChatCommand( "jump" )]
		public static void OnStreamJumpCommand()
		{
			if ( !Host.IsClient )
				return;

			JumpPlayerCommand( OnChatCommand.User );
		}

		[OnChatCommand( "reset" )]
		public static void OnStreamResetCommand()
		{
			if ( !Host.IsClient )
				return;

			ResetPlayerCommand( OnChatCommand.User );
		}

		[OnChatCommand( "w" )] public static void OnStreamForwardCommand() => MovePlayerCommand( OnChatCommand.User, Vector3.Forward );
		[OnChatCommand( "a" )] public static void OnStreamLeftCommand() => MovePlayerCommand( OnChatCommand.User, Vector3.Left );
		[OnChatCommand( "s" )] public static void OnStreamBackwardCommand() => MovePlayerCommand( OnChatCommand.User, Vector3.Backward );
		[OnChatCommand( "d" )] public static void OnStreamRightCommand() => MovePlayerCommand( OnChatCommand.User, Vector3.Right );

		[ServerCmd( "stream_addplayer" )]
		public static void AddPlayerCommand( string user )
		{
			Host.AssertServer();
			Current.AddPlayer( user );
		}

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

		[ServerCmd( "stream_removeplayer" )]
		public static void RemovePlayerCommand( string user )
		{
			Host.AssertServer();
			Current.RemovePlayer( user );
		}

		private void RemovePlayer( string user )
		{
			if ( Players.TryGetValue( user, out var player ) )
			{
				Players.Remove( user );
				player?.Delete();
			}
		}

		[ServerCmd( "stream_resetplayer" )]
		public static void ResetPlayerCommand( string user )
		{
			Host.AssertServer();
			Current.ResetPlayer( user );
		}

		private void ResetPlayer( string user )
		{
			if ( !Players.TryGetValue( user, out var player ) )
				return;

			MoveToSpawnpoint( player );
		}

		[ServerCmd( "stream_jumpplayer" )]
		public static void JumpPlayerCommand( string user )
		{
			Host.AssertServer();
			Current.JumpPlayer( user );
		}

		private void JumpPlayer( string user )
		{
			if ( !Players.TryGetValue( user, out var player ) )
				return;

			player.ApplyAbsoluteImpulse( Vector3.Up * (player.PhysicsGroup.Mass * 20.0f) );
		}

		[ServerCmd( "stream_moveplayer" )]
		public static void MovePlayerCommand( string user, Vector3 direction )
		{
			Host.AssertServer();
			Current.MovePlayer( user, direction );
		}

		private void MovePlayer( string user, Vector3 direction )
		{
			if ( !Players.TryGetValue( user, out var player ) )
				return;

			var rotation = Rotation.From( LocalClient.Pawn.Rotation.Angles().WithPitch( 0 ) );
			player.ApplyAbsoluteImpulse( rotation * direction * (player.PhysicsGroup.Mass * 30.0f) );
		}
	}
}
