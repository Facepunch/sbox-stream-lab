using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitchLab
{
	public partial class Game : Sandbox.Game
	{
		public static new Game Current { get; private set; }
		public static Client LocalClient { get; private set; }
		public Hud Hud { get; private set; }

		private readonly Dictionary<string, Player> Players = new();

		public Game()
		{
			Current = this;

			if ( IsClient )
			{
				Hud = new Hud();
				Local.Hud = Hud;
			}
		}

		public override void Shutdown()
		{
			if ( Current == this )
			{
				Current = null;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if ( Local.Hud == Hud )
			{
				Local.Hud = null;
			}

			Hud?.Delete();
			Hud = null;
		}

		public override void ClientJoined( Client cl )
		{
			if ( LocalClient == null )
			{
				LocalClient = cl;
			}

			cl.Pawn = new Pawn();
			MoveToSpawnpoint( cl.Pawn );
			cl.Pawn.Position += Vector3.Up * 50;
		}

		public override void Simulate( Client cl )
		{
			cl.Pawn?.Simulate( cl );
		}

		public override void FrameSimulate( Client cl )
		{
			Host.AssertClient();

			cl.Pawn?.FrameSimulate( cl );
		}

		public override void BuildInput( InputBuilder input )
		{
			var driving = input.Down( InputButton.Attack2 );
			Hud.SetClass( "driving", driving );

			if ( !driving )
				return;

			Local.Pawn?.BuildInput( input );
		}

		public override CameraSetup BuildCamera( CameraSetup camSetup )
		{
			camSetup.Rotation = Rotation.Identity;
			camSetup.Position = Vector3.Zero;
			camSetup.FieldOfView = 80;
			camSetup.Ortho = false;
			camSetup.Viewer = null;

			if ( Local.Client.Pawn != null )
			{
				camSetup.Rotation = Local.Client.Pawn.EyeRot;
				camSetup.Position = Local.Client.Pawn.EyePos;
				camSetup.Viewer = Local.Client.Pawn;
			}

			return camSetup;
		}

		public void ResetPlayers()
		{
			foreach ( var player in Players.Values )
			{
				MoveToSpawnpoint( player );
			}
		}

		[ClientRpc]
		void OnChatMessage( string name, string message, string color )
		{
			Hud.AddChatEntry( name, message, color );
		}
	}
}
