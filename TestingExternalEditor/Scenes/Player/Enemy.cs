using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Vector2 = Godot.Vector2;

public partial class Enemy : CharacterBody2D
{

	private Vector2 enemyLastNodePosition = new Vector2();

	private Vector2 enemyTargetPosition = Vector2.Zero;

	private float enemyWalkDistance = 200;

	private int currentIndex = 0;

	private bool playerDetected = false;

	private Timer timer;

	private PackedScene bullet = GD.Load<PackedScene>("res://Scenes/Bullet/bullet_pack.tscn");
	
	public override void _Ready()
	{
		this.enemyLastNodePosition = this.Position;
		this.Velocity = PerimeterWalk(currentIndex);
		currentIndex++;
		TurnRight();

		timer = GetNode<Timer>("Timer");
		timer.WaitTime = GenerateRandomNumber();
		timer.Timeout += OnTimerTimeout;
	}

	
	public override void _PhysicsProcess(double delta)
	{
		
		if (!playerDetected) {
			if(enemyLastNodePosition.DistanceTo(this.Position) >= this.enemyWalkDistance) {
				this.Velocity = PerimeterWalk(currentIndex, enemyWalkDistance);
				this.currentIndex = currentIndex == 3 ? 0 : currentIndex + 1;
				this.enemyLastNodePosition = this.Position;
			}
		}

		PerformRayCast(delta);

		MoveAndSlide();
	}

	public int GenerateRandomNumber() {
		var randomNumberGeneder = new Random();
		var randomTime = randomNumberGeneder.Next(1, 10);
		return randomTime;
	}

	public void PerformRayCast(double delta) {

		Vector2 raycastEndPoint = calculateRayCastEndPoint();
		var spaceState = GetWorld2D().DirectSpaceState;
		var query = PhysicsRayQueryParameters2D.Create(Transform.Origin, raycastEndPoint, CollisionMask, new Godot.Collections.Array<Rid> { GetRid() });

		var result = spaceState.IntersectRay(query);
		
		if (this.Position == this.enemyTargetPosition && this.enemyTargetPosition != Vector2.Zero) { 
			if(result.Count > 0) {
				this.enemyTargetPosition = (Vector2) result["position"];
				Mathf.MoveToward(this.Position.X, enemyTargetPosition.X, delta);
				Mathf.MoveToward(this.Position.Y, enemyTargetPosition.Y, delta);
				this.playerDetected = true;
			}
		}
	
	}

	private Vector2 PerimeterWalk(int index = 0, float speed = 200) {
		Vector2 velocity = new Vector2(speed, speed);

		List<Vector2> pivotPoints = new List<Vector2>() {
			new Vector2(0, 1),
			new Vector2(1, 0),
			new Vector2(0, -1),
			new Vector2(-1, 0)
		};

		velocity *= pivotPoints[index];

		return velocity;
	}

	private Vector2 calculateRayCastEndPoint(int a = 1000) {
		var o = Mathf.Tan(Mathf.DegToRad(this.RotationDegrees)) * a;
		var x = this.Position.X - a;
		var y = this.Position.Y - o;
		return new Vector2(x, y);
	}

	private void TurnRight() {
		var turnRightTween = GetTree().CreateTween();
		turnRightTween.TweenProperty(this, "rotation_degrees", 30, 1);
		turnRightTween.TweenCallback(Callable.From(TurnLeft));
	}

	private void TurnLeft() {
		var turnLeftTween = GetTree().CreateTween();
		turnLeftTween.TweenProperty(this, "rotation_degrees", -30, 2);
		turnLeftTween.TweenCallback(Callable.From(TurnRight));
	}

	private void OnTimerTimeout() {
		this.SpawnBullet();
		this.timer.WaitTime = GenerateRandomNumber();
	}

	public void SpawnBullet() {
		Bullet shot = bullet.Instantiate<Bullet>();
		Node2D bulletPack = GetNode<Node2D>("BulletPack");
		bulletPack.AddChild(shot);
		shot.CollideWithBody("Player");
		FireBullet(shot);
	}

	public void FireBullet(Bullet bullet) {
		Tween tween = GetTree().CreateTween();
		Tween tween2 = GetTree().CreateTween();

		tween.TweenProperty(bullet, "position", bullet.Position + new Vector2(500, 0), 0.5f);
		tween2.TweenProperty(bullet, "modulate:a", 0, 0.5f);
		tween.TweenCallback(Callable.From(bullet.QueueFree));
	}

}
