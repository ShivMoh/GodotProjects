using System.Collections.Generic;
using System.Linq;
using Godot;

using Vector2 = Godot.Vector2;
public partial class Character : CharacterBody2D
{
	public CharacterStat characterStat;
	public const float Speed = 200.0f;

	public Vector2 targetGlobalPosition;

	public int moveSteps;
	public bool move = false;

	public bool usedTurn = false;

	public bool open = false;
	public AnimatedSprite2D animatedSprite;

	public List<AttackMeta> attacks;

	public AttackMeta equipedAttack;

	[Export]
	public HealthBar healthBar;
	public override void _Ready()
	{
		targetGlobalPosition = this.GlobalPosition;
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		this.ZIndex = 10;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!open) {
			Vector2 velocity = this.Velocity;

			velocity = this.moveTo(velocity);

			this.Velocity = velocity;
			
			MoveAndSlide();
		}
	}

	public bool isMoving() {
		return move;
	}

	public static Character instantiate(Vector2 GlobalPosition, string path) {
		PackedScene characterScene = GD.Load<PackedScene>(path);
		Character instance = characterScene.Instantiate<Character>();
		instance.GlobalPosition = GlobalPosition;
		return instance;
	}

	public void setCharacterStats(CharacterStat characterStat) {
		this.characterStat = characterStat;
	}

	public CharacterStat getCharacterStats() {
		return characterStat;
	}

	public void setAttacks(List<AttackMeta> attacks) {
		this.attacks = attacks;
		this.equipedAttack = attacks.First();
	}


	protected Vector2 moveTo(Vector2 velocity) {
		
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

	protected Vector2 calculateDirection() {
		float directionX =  Mathf.Round(targetGlobalPosition.X - this.GlobalPosition.X);
		float directionY = Mathf.Round(targetGlobalPosition.Y - this.GlobalPosition.Y);

		Vector2 direction = new Vector2(directionX, directionY).Normalized();
		return direction;
	}
 
	public void playAnimation(Vector2 direction) {
		if (direction.X > 0) animatedSprite.Play("right");
	
		if (direction.X < 0) animatedSprite.Play("left");
	
		if (direction.Y < 0) animatedSprite.Play("back");
	
		if (direction.Y > 0) animatedSprite.Play("front");
		
		if (direction == Vector2.Zero) animatedSprite.Play("idle");
	}

}
   
