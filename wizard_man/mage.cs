using Godot;
using System;

public partial class mage : CharacterBody3D
{

	float speed;
	float gravity;

	private const float RayLength = 1000.0f;

	[Export]
	public Camera3D camera;
	
	Vector3 velocity;

	public bool move = false;

	public Vector3 toPosition = Vector3.Zero;

	private int moves = 0;

	private int maxMoves = 2000;

	
	public override void _Ready()
	{
		speed = 10.0f;
		gravity = 400.0f;

	}

	public override void _PhysicsProcess(double delta)
	{

		velocity = Velocity;
		
		MoveTo();
		Velocity = velocity;
		MoveAndSlide();
	}

	public void MoveTo() {
		if (!move) return;
		if (toPosition == GlobalPosition) return;
		if (moves >= maxMoves) {
			velocity = Vector3.Zero;
			return;
		}
		Vector3 direction = toPosition - GlobalPosition;
		velocity.X = direction.X * speed;
		velocity.Z = direction.Z * speed;

	}

	public void setToPosition(Vector3 to) {
		toPosition = to;
		moves+=1;
	}

	// public override void _Input(InputEvent @event)
	// {
	// 	base._Input(@event);
	// 	// if (@event is InputEventMouseButton mouseEvent) {
	// 	// 	GD.Print("Position", GlobalPosition);
	// 	// 	GD.Print("Mouse Click Position", mouseEvent.Position);
			
	// 	// 	GD.Print("Mouse Click Position Viewport", GetViewport().GetMousePosition());
	// 	// 	// GlobalPosition = new Vector3(mouseEvent.Position.X, Position.Y, mouseEvent.Position.Y);
	// 	// }

	// 	if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.Left) {

	// 		var from = camera.ProjectRayOrigin(eventMouseButton.Position);
	// 		var to = from + camera.ProjectRayNormal(eventMouseButton.Position) * RayLength;

	// 		GD.Print(GlobalPosition);
	// 		GD.Print(from, to);
	// 		GD.Print(eventMouseButton.Position);
	// 	}
	// }

}
