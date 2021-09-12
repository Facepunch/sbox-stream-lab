using Sandbox.UI;
using Sandbox.UI.Construct;

namespace TwitchLab
{
	public partial class ChatEntry : Panel
	{
		public Label NameLabel { get; internal set; }
		public Label MessageLabel { get; internal set; }
		public Image BadgeImage { get; internal set; }

		public ChatEntry()
		{
			BadgeImage = Add.Image();
			NameLabel = Add.Label( "Name", "name" );
			MessageLabel = Add.Label( "Message", "message" );
		}
	}
}
