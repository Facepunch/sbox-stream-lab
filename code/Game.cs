using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitchLab
{
	public partial class Game : GameBase
	{
		public static Game Current { get; private set; }
		public static Client LocalClient { get; private set; }
		public Hud Hud { get; private set; }

		private readonly Dictionary<string, Player> Players = new();

		public Game()
		{
			Current = this;
			Transmit = TransmitType.Always;

			if ( IsClient )
			{
				Hud = new Hud();
				Local.Hud = Hud;
			}

			if ( IsServer )
			{
				Stream.JoinChannel( Stream.Username );
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

		public static void MoveToSpawnpoint( Entity pawn )
		{
			var spawnpoint = All.OfType<SpawnPoint>()
				.OrderBy( x => Guid.NewGuid() )
				.FirstOrDefault();

			if ( spawnpoint == null )
			{
				Log.Warning( $"Couldn't find spawnpoint for {pawn}!" );
				return;
			}

			pawn.ResetInterpolation();
			pawn.Transform = spawnpoint.Transform;
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

		public override void PostLevelLoaded()
		{
		}

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
		}

		public override bool CanHearPlayerVoice( Client source, Client dest )
		{
			return false;
		}

		public override void PostCameraSetup( ref CameraSetup camSetup )
		{
		}

		public override void OnVoicePlayed( ulong steamId, float level )
		{
		}
	}
}
