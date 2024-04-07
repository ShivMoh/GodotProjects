using Godot;
using System;

public partial class mage : CharacterBody3D
{

	float speed;
	float gravity;

	Vector3 velocity;

	public override void _Ready()
	{
		speed = 5.0f;
		gravity = 400.0f;
	}

	public override void _PhysicsProcess(double delta)
	{

		velocity = Velocity;
		Vector2 dir = Input.GetVector("ui_right", "ui_left", "ui_down", "ui_up");
		Vector3 direction = (Transform.Basis * new Vector3(dir.X, 0, dir.Y)).Normalized();
		if (direction != Vector3.Zero) {
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
		} else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event is InputEventMouseButton mouseEvent) {
			GD.Print("Position", GlobalPosition);
			GD.Print("Mouse Click Position", mouseEvent.Position.Normalized());
			// GlobalPosition = new Vector3(mouseEvent.Position.X, Position.Y, mouseEvent.Position.Y);
		}
	}

}
