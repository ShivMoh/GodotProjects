using Godot;
using System;

public partial class HealthBar : TextureProgressBar
{
	[Export]
	public Character character;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (this.character is Character) {
			// //GD.Print("is character");
			this.Value = this.character.getCharacterStats().health;
			this.MaxValue = this.character.getCharacterStats().health;
			this.Size = new Vector2((float) this.Value, this.Size.Y);
			// //GD.Print(this.character.getCharacterStats().health);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		// this.Value = this.initalValue - this.damage;
	}

	public void takeDamage(int damage) {
		// this.damage += damage;
		this.Value -= damage;
	}

	public void setUpParameters(int health) {
		  this.Value = health;
		  this.MaxValue = health;
		  this.Size = new Vector2((float) this.Value, this.Size.Y);
	}

	
}
