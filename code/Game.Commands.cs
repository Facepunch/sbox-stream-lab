using Sandbox;

namespace TwitchLab
{
	public partial class Game
	{
		[ClientCmd( "stream_clear" )]
		public static void StreamClearCommand()
		{
			Streamer.ClearChat();
		}

		[ClientCmd( "stream_say" )]
		public static void StreamSayCommand( string message )
		{
			Streamer.SendMessage( message );
		}

		[ClientCmd( "stream_ban" )]
		public static void StreamBanCommand( string username, string reason = null )
		{
			Streamer.BanUser( username, reason );
		}

		[ClientCmd( "stream_unban" )]
		public static void StreamUnbanCommand( string username )
		{
			Streamer.UnbanUser( username );
		}

		[ClientCmd( "stream_timeout" )]
		public static void StreamTimeoutCommand( string username, int duration, string reason = null )
		{
			Streamer.BanUser( username, reason, duration );
		}

		[ClientCmd( "stream_resetplayers" )]
		public static void StreamResetPlayersCommand()
		{
			Current.ResetPlayers();
		}

		[ClientCmd( "stream_channel_game" )]
		public static void StreamChannelGameCommand( string gameId )
		{
			Streamer.Game = gameId;
		}

		[ClientCmd( "stream_channel_language" )]
		public static void StreamChannelLanguageCommand( string languageId )
		{
			Streamer.Language = languageId;
		}

		[ClientCmd( "stream_channel_title" )]
		public static void StreamChannelTitleCommand( string title )
		{
			Streamer.Title = title;
		}

		[ClientCmd( "stream_channel_delay" )]
		public static void StreamChannelDelayCommand( int delay )
		{
			Streamer.Delay = delay;
		}

		[ClientCmd( "stream_followers" )]
		public static async void StreamFollowersCommand()
		{
			Log.Info( "Followers" );

			var user = await Streamer.GetUser();
			var follows = await user.Followers;

			foreach ( var follow in follows )
			{
				Log.Info( $"UserId: {follow.UserId}" );
				Log.Info( $"Username: {follow.Username}" );
				Log.Info( $"DisplayName: {follow.DisplayName}" );
				Log.Info( $"FollowedAt: {follow.CreatedAt}" );
			}
		}

		[ClientCmd( "stream_following" )]
		public static async void StreamFollowingCommand()
		{
			Log.Info( "Following" );

			var user = await Streamer.GetUser();
			var follows = await user.Following;

			foreach ( var follow in follows )
			{
				Log.Info( $"UserId: {follow.UserId}" );
				Log.Info( $"Username: {follow.Username}" );
				Log.Info( $"DisplayName: {follow.DisplayName}" );
				Log.Info( $"FollowedAt: {follow.CreatedAt}" );
			}
		}
	}
}
