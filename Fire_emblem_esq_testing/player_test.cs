using Godot;
using System;

public partial class player_test : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public Vector2 target_position;
	
	public override void _Ready()
	{
		target_position = this.GlobalPosition;
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		this.moveTo(velocity);

		Velocity = velocity;
		MoveAndSlide();
	}

	private void moveTo(Vector2 velocity) {
		Vector2 direction = target_position - this.GlobalPosition;

		velocity.X = direction.X * Speed;
		velocity.Y = direction.Y * Speed;
	}
}
