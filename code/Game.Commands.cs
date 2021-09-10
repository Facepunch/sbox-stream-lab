using Sandbox.Streaming;

namespace TwitchLab
{
	public partial class Game
	{
		[AdminCmd( "stream_clear" )]
		public static void StreamClearCommand()
		{
			StreamClient.ClearChat();
		}

		[AdminCmd( "stream_say" )]
		public static void StreamSayCommand( string message )
		{
			StreamClient.SendMessage( message );
		}

		[AdminCmd( "stream_ban" )]
		public static void StreamBanCommand( string username, string reason = null )
		{
			StreamClient.BanUser( username, reason );
		}

		[AdminCmd( "stream_unban" )]
		public static void StreamUnbanCommand( string username )
		{
			StreamClient.UnbanUser( username );
		}

		[AdminCmd( "stream_timeout" )]
		public static void StreamTimeoutCommand( string username, int duration, string reason = null )
		{
			StreamClient.TimeoutUser( username, duration, reason );
		}

		[AdminCmd( "stream_joinchannel" )]
		public static void StreamJoinChannelCommand( string channel )
		{
			StreamClient.JoinChannel( channel );
		}

		[AdminCmd( "stream_leavechannel" )]
		public static void StreamLeaveChannelCommand( string channel )
		{
			StreamClient.LeaveChannel( channel );
		}

		[AdminCmd( "stream_resetplayers" )]
		public static void StreamResetPlayersCommand()
		{
			Current.ResetPlayers();
		}
	}
}
