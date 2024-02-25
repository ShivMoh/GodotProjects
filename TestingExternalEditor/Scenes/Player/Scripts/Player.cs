using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 500.0f;
	public const float JumpVelocity = -400.0f;

	private int damage = 0;

	private PackedScene bullet = GD.Load<PackedScene>("res://Scenes/Bullet/bullet_pack.tscn");

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (Input.IsActionJustPressed("ui_accept")) {
			SpawnBullet();
		}

	
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		if (Input.IsActionPressed("turn_right")) {
			this.RotatePlayer(1);
		}

		if(Input.IsActionPressed("turn_left")) {
			this.RotatePlayer(-1);
		}

		if (direction != Vector2.Zero)
		{
			velocity = direction * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void SpawnBullet() {
		Bullet shot = bullet.Instantiate<Bullet>();
		Node2D bulletPack = GetNode<Node2D>("BulletPack");
		bulletPack.AddChild(shot);
		shot.CollideWithBody("Enemy");
		FireBullet(shot);
	}

	public void FireBullet(Bullet bullet) {
		Tween tween = GetTree().CreateTween();
		Tween tween2 = GetTree().CreateTween();

		tween.TweenProperty(bullet, "position", bullet.Position + new Vector2(500, 0), 0.5f);
		tween2.TweenProperty(bullet, "modulate:a", 0, 0.5f);
		tween.TweenCallback(Callable.From(bullet.QueueFree));
	}

	public void RotatePlayer(int direction) {
		var degree = direction * 1;
		this.Rotate(Mathf.DegToRad(degree));
	}
}
