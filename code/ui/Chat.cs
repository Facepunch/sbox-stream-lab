using Sandbox.UI;
using System.Collections.Generic;

namespace TwitchLab
{
	public partial class Chat : Panel
	{
		public Panel Canvas { get; protected set; }

		readonly Queue<ChatEntry> Entries = new();

		public Chat()
		{
			StyleSheet.Load( "/ui/Chat.scss" );
			Canvas = Add.Panel( "chat_canvas" );
			Canvas.PreferScrollToBottom = true;
		}

		public void AddEntry( string name, string message, string avatar, string color )
		{
			var e = Canvas.AddChild<ChatEntry>();
			e.MessageLabel.Text = message;
			e.NameLabel.Text = name;
			e.NameLabel.Style.FontColor = Color.Parse( color );
			e.BadgeImage.SetTexture( avatar );

			e.SetClass( "noname", string.IsNullOrEmpty( name ) );
			e.SetClass( "noavatar", string.IsNullOrEmpty( avatar ) );

			if ( Entries.Count >= 20 )
			{
				Entries.Dequeue()?.Delete( true );
			}

			Entries.Enqueue( e );
		}
	}
}

