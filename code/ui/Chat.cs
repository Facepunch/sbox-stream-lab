using Sandbox.UI;
using System.Collections.Generic;

namespace TwitchLab
{
	public partial class Chat : Panel
	{
		public Panel Canvas { get; protected set; }

		private readonly Queue<ChatEntry> Entries = new();
		private ChatEntry LastEntry = null;

		public Chat()
		{
			StyleSheet.Load( "/ui/Chat.scss" );
			Canvas = Add.Panel( "chat_canvas" );
			Canvas.PreferScrollToBottom = true;
		}

		public void AddEntry( string name, string message, string avatar, string color )
		{
			if ( Entries.Count >= 20 )
			{
				Entries.Dequeue()?.Delete( true );
			}

			ChatEntry entry = LastEntry;
			if ( entry == null || entry.NameLabel.Text != name )
			{
				entry = Canvas.AddChild<ChatEntry>();
				entry.MessageLabel.Text = message;

				Entries.Enqueue( entry );
				LastEntry = entry;
			}
			else
			{
				entry.MessageLabel.Text += "\n";
				entry.MessageLabel.Text += message;
			}

			entry.NameLabel.Text = name;
			entry.NameLabel.Style.FontColor = Color.Parse( color );
			entry.BadgeImage.SetTexture( avatar );

			entry.SetClass( "noname", string.IsNullOrEmpty( name ) );
			entry.SetClass( "noavatar", string.IsNullOrEmpty( avatar ) );
		}
	}
}

