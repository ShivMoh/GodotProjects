using Godot;
using System;

public partial class newPlayer : CharacterBody3D
{
	public const float Speed = 15.0f;
	public const float JumpVelocity = 4.5f;

	public const float gravity = 5.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		rotateCharacterY();
		
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void rotateCharacterY() 
	{
		if (Input.IsActionPressed("ui_left"))
		{
			this.RotationDegrees = this.RotationDegrees + new Vector3(0, 1, 0);
		}

		if (Input.IsActionPressed("ui_right"))
		{
			this.RotationDegrees = this.RotationDegrees + new Vector3(0, -1, 0);
		}	
	}
}
