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

		[Event.Stream.ChatMessage]
		public static void OnStreamMessage( StreamChatMessage message )
		{
			if ( !Host.IsServer )
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

		[Event.Stream.JoinChat]
		public static void OnStreamJoinEvent( string user )
		{
			if ( !Host.IsServer )
				return;

			Log.Info( $"{user} joined" );
		}

		[Event.Stream.LeaveChat]
		public static void OnStreamLeaveEvent( string user )
		{
			if ( !Host.IsServer )
				return;

			Log.Info( $"{user} left" );

			Current.RemovePlayer( user );
		}

		[OnChatCommand( "!play" )]
		public static void OnStreamPlayCommand()
		{
			if ( !Host.IsServer )
				return;

			Current.AddPlayer( OnChatCommand.User );
		}

		[OnChatCommand( "!quit" )]
		public static void OnStreamQuitCommand()
		{
			if ( !Host.IsServer )
				return;

			Current.RemovePlayer( OnChatCommand.User );
		}

		[OnChatCommand( "jump" )]
		public static void OnStreamJumpCommand()
		{
			if ( !Host.IsServer )
				return;

			Current.JumpPlayer( OnChatCommand.User );
		}

		[OnChatCommand( "reset" )]
		public static void OnStreamResetCommand()
		{
			if ( !Host.IsServer )
				return;

			Current.ResetPlayer( OnChatCommand.User );
		}

		[OnChatCommand( "w" )] public static void OnStreamForwardCommand() => Current.MovePlayer( OnChatCommand.User, Vector3.Forward );
		[OnChatCommand( "a" )] public static void OnStreamLeftCommand() => Current.MovePlayer( OnChatCommand.User, Vector3.Left );
		[OnChatCommand( "s" )] public static void OnStreamBackwardCommand() => Current.MovePlayer( OnChatCommand.User, Vector3.Backward );
		[OnChatCommand( "d" )] public static void OnStreamRightCommand() => Current.MovePlayer( OnChatCommand.User, Vector3.Right );

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
