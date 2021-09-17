using Sandbox;

namespace TwitchLab
{
	public partial class Game
	{
		[AdminCmd( "stream_clear" )]
		public static void StreamClearCommand()
		{
			Stream.ClearChat();
		}

		[AdminCmd( "stream_say" )]
		public static void StreamSayCommand( string message )
		{
			Stream.SendMessage( message );
		}

		[AdminCmd( "stream_ban" )]
		public static void StreamBanCommand( string username, string reason = null )
		{
			Stream.BanUser( username, reason );
		}

		[AdminCmd( "stream_unban" )]
		public static void StreamUnbanCommand( string username )
		{
			Stream.UnbanUser( username );
		}

		[AdminCmd( "stream_timeout" )]
		public static void StreamTimeoutCommand( string username, int duration, string reason = null )
		{
			Stream.BanUser( username, reason, duration );
		}

		[AdminCmd( "stream_joinchannel" )]
		public static void StreamJoinChannelCommand( string channel )
		{
			Stream.JoinChannel( channel );
		}

		[AdminCmd( "stream_leavechannel" )]
		public static void StreamLeaveChannelCommand( string channel )
		{
			Stream.LeaveChannel( channel );
		}

		[AdminCmd( "stream_resetplayers" )]
		public static void StreamResetPlayersCommand()
		{
			Current.ResetPlayers();
		}

		[AdminCmd( "stream_channel_game" )]
		public static void StreamChannelGameCommand( string gameId )
		{
			Stream.Game = gameId;
		}

		[AdminCmd( "stream_channel_language" )]
		public static void StreamChannelLanguageCommand( string languageId )
		{
			Stream.Language = languageId;
		}

		[AdminCmd( "stream_channel_title" )]
		public static void StreamChannelTitleCommand( string title )
		{
			Stream.Title = title;
		}

		[AdminCmd( "stream_channel_delay" )]
		public static void StreamChannelDelayCommand( int delay )
		{
			Stream.Delay = delay;
		}

		[AdminCmd( "stream_followers" )]
		public static async void StreamFollowersCommand()
		{
			Log.Info( "Followers" );

			var user = await Stream.GetUser();
			var follows = await user.Followers;

			foreach ( var follow in follows )
			{
				Log.Info( $"UserId: {follow.UserId}" );
				Log.Info( $"Username: {follow.Username}" );
				Log.Info( $"DisplayName: {follow.DisplayName}" );
				Log.Info( $"FollowedAt: {follow.CreatedAt}" );
			}
		}

		[AdminCmd( "stream_following" )]
		public static async void StreamFollowingCommand()
		{
			Log.Info( "Following" );

			var user = await Stream.GetUser();
			var follows = await user.Following;

			foreach ( var follow in follows )
			{
				Log.Info( $"UserId: {follow.UserId}" );
				Log.Info( $"Username: {follow.Username}" );
				Log.Info( $"DisplayName: {follow.DisplayName}" );
				Log.Info( $"FollowedAt: {follow.CreatedAt}" );
			}
		}

		[AdminCmd( "stream_game" )]
		public static async void StreamGameCommand( string gameName )
		{
			Log.Info( $"Game: {gameName}" );

			var game = await Stream.GetGame( gameName );

			Log.Info( $"BoxArtUrl: {game.BoxArtUrl}" );
			Log.Info( $"Id: {game.Id}" );
			Log.Info( $"Name: {game.Name}" );

			var broadcasts = await Stream.GetBroadcasts( game.Id );

			foreach ( var broadcast in broadcasts )
			{
				Log.Info( $"DisplayName: {broadcast.DisplayName}" );
				Log.Info( $"ViewerCount: {broadcast.ViewerCount}" );
				Log.Info( $"ThumbnailUrl: {broadcast.ThumbnailUrl}" );
				Log.Info( $"Title: {broadcast.Title}" );
			}
		}
	}
}
