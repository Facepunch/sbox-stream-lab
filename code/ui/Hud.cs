using Sandbox.UI;

namespace TwitchLab
{
	public class Hud : RootPanel
	{
		public NameTags NameTags { get; private set; }

		public Hud()
		{
			SetTemplate( "/ui/Hud.html" );

			NameTags = AddChild<NameTags>();
		}
	}
}
