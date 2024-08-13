using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class CameraState : State {

	private SetupUtility setupUtility;

	public override void enter()
	{

	}

	public override void physicsUpdate(double _delta)
	{
		Vector2 direction = Input.GetVector("left", "right", "up", "down");

		MapEntities.mapCamera.GlobalPosition += 5.0f * direction;


		if (Input.IsActionJustPressed("zoom_out")) {
			MapEntities.mapCamera.Zoom = MapEntities.mapCamera.Zoom * 2;
			//GD.Print(MapEntities.mapCamera.Enabled);
			//GD.Print(MapEntities.mapCamera.Zoom);
			//GD.Print("Zooming out");
		}

		if (Input.IsActionJustPressed("zoom_in")) {
			MapEntities.mapCamera.Zoom = MapEntities.mapCamera.Zoom / 2;

			////GD.Print(GetViewport().GetCamera2D() as Camera2D is null);
			// //GD.Print(MapEntities.mapCamera.Enabled);
			// //GD.Print(MapEntities.mapCamera.Zoom);
			//GD.Print("Zooming in");
		}
		
		if (Input.IsActionJustPressed("select")) EmitSignal(SignalName.StateChange, this, previousStateName);
	}
}
