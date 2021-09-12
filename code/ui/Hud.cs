using Sandbox.UI;

namespace TwitchLab
{
	public class Hud : RootPanel
	{
		public NameTags NameTags { get; private set; }
		public Chat Chat { get; private set; }

		public Hud()
		{
			SetTemplate( "/ui/Hud.html" );
		}

		public void AddChatEntry( string name, string message, string color )
		{
			Chat.AddEntry( name, message, null, color );
		}
	}
}
