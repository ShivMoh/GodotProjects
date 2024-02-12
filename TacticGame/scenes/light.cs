using Godot;
using System;

public partial class light : DirectionalLight3D
{
	[Export] public newPlayer player;

	public override void _PhysicsProcess(double delta)
	{
		// base._PhysicsProcess(delta);

		// this.Position = new Vector3(player.Position.X, player.Position.Y + 20, player.Position.Z);
	}

}
