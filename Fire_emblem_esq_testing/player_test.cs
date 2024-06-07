using Godot;
using System;
using System.Numerics;

using Vector2 = Godot.Vector2;
public partial class player_test : CharacterBody2D
{
	public const float Speed = 100.0f;

	public Vector2 target_position;
	public bool move = false;
	
	private AnimatedSprite2D animatedSprite;
	public override void _Ready()
	{
		target_position = this.GlobalPosition;
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		velocity = this.moveTo(velocity);

		Velocity = velocity;
		MoveAndSlide();
	}

	private Vector2 moveTo(Vector2 velocity) {
		
		if (!move) {
			playAnimation(Vector2.Zero);
			return Vector2.Zero;
		}

		Vector2 v = velocity;

		Vector2 direction = calculateDirection(); 

		playAnimation(direction);

		v.X = direction.X * Speed;
		v.Y = direction.Y * Speed;

		return v;
	}

	private Vector2 calculateDirection() {
		float directionX =  Mathf.Round(target_position.X - this.GlobalPosition.X);
		float directionY = Mathf.Round(target_position.Y - this.GlobalPosition.Y);

		Vector2 direction = new Vector2(directionX, directionY).Normalized();
		return direction;
	}
 
	private void playAnimation(Vector2 direction) {
		if (direction.X > 0) {
			animatedSprite.Play("right");
		}

		if (direction.X < 0) {
			animatedSprite.Play("left");
		}

		if (direction.Y < 0) {
			animatedSprite.Play("back");
		}

		if (direction.Y > 0) {
			animatedSprite.Play("front");
		}

		if (direction == Vector2.Zero) {
			animatedSprite.Play("idle");
		}
	}
}