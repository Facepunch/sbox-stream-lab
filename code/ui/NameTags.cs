using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace TwitchLab
{
	public class NameTag : Panel
	{
		private readonly Player player;
		private Image image;

		public NameTag( Player player )
		{
			this.player = player;

			Add.Label( player.DisplayName );
			image = Add.Image( "ui/twitch.jpg" );
		}

		public void SetImageTexture( string texture )
		{
			image.SetTexture( texture );
		}

		public override void Tick()
		{
			base.Tick();

			if ( !player.IsValid() )
				return;

			var labelPos = player.WorldSpaceBounds.Center + Vector3.Up * 10;
			var lookDir = (labelPos - CurrentView.Position).Normal;

			if ( CurrentView.Rotation.Forward.Dot( lookDir ) < 0.5f )
			{
				Style.Opacity = 0.0f;
				return;
			}

			var screenPos = labelPos.ToScreen();

			Style.Left = Length.Fraction( screenPos.x );
			Style.Top = Length.Fraction( screenPos.y );

			var transform = new PanelTransform();
			transform.AddTranslateY( Length.Fraction( -1.0f ) );
			transform.AddTranslateX( Length.Fraction( -0.5f ) );

			Style.Opacity = 1.0f;
			Style.Transform = transform;
			Style.Dirty();
		}
	}

	public class NameTags : Panel
	{
		public NameTags()
		{
			StyleSheet.Load( "/ui/NameTags.scss" );
		}

		public NameTag AddNameTag( Player player )
		{
			return new NameTag( player )
			{
				Parent = this
			};
		}
	}
}
