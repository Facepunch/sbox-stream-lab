using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Tests;

namespace TwitchLab
{
	public partial class StreamList : Panel
	{
		VirtualScrollPanel ScrollPanel;

		public StreamList()
		{
			StyleSheet.Load( "/ui/StreamList.scss" );
			ScrollPanel = AddChild<VirtualScrollPanel>( "streams" );

			ScrollPanel.Layout.ItemSize = new Vector2( 300, 180 );
			ScrollPanel.Layout.AutoColumns = true;
			ScrollPanel.PreferScrollToBottom = true;
			ScrollPanel.OnCreateCell = ( cell, data ) =>
			{
				var entry = (Entry)data;
				cell.AddChild<Image>( "thumbnail" ).SetTexture( entry.Image );
				cell.AddChild<Label>( "stream" ).SetText( entry.Name );
			};

			AddEntries();
		}

		struct Entry
		{
			public string Name { get; set; }
			public string Image { get; set; }
		}

		async void AddEntries()
		{
			var game = await Streamer.GetGame( "rust" );

			Log.Info( $"BoxArtUrl: {game.BoxArtUrl}" );
			Log.Info( $"Id: {game.Id}" );
			Log.Info( $"Name: {game.Name}" );

			var broadcasts = await game.Broadcasts;

			foreach ( var broadcast in broadcasts )
			{
				Log.Info( broadcast.ThumbnailUrl );
				var url = broadcast.ThumbnailUrl;
				url = url.Replace( "{width}", 290.ToString() );
				url = url.Replace( "{height}", 170.ToString() );
				AddEntry( $"{broadcast.Title}\n\n{broadcast.DisplayName}\n\n{broadcast.ViewerCount} viewers", url );
			}
		}

		public void AddEntry( string name, string image )
		{
			ScrollPanel.AddItem( new Entry { Name = name, Image = image } );
		}
	}
}

