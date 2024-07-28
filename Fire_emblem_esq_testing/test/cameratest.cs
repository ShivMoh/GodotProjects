using Godot;
using System;

public partial class cameratest : Node2D
{
	// Called when the node enters the scene tree for the first time.

	Camera2D camera2D;
	public override void _Ready()
	{
		this.camera2D = GetViewport().GetCamera2D();
		// this.camera2D.Zoom = new Vector2(0.5f, 0.5f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("zoom_out")) {
			this.camera2D.Zoom = this.camera2D.Zoom * 2;
		}

		if (Input.IsActionJustPressed("zoom_in")) {
			this.camera2D.Zoom = this.camera2D.Zoom / 2;
		}
	}
}
