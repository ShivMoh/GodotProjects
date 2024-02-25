using Godot;
using System;

public partial class health_bar : TextureProgressBar
{

	[Export]
	public CharacterBody2D body;
	private int health = 100;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
	
	}

	public void handleDamage() {
		this.Value -= 10;

		if(this.Value == 0) {
			this.EnemyDeath();
			var data = GetNode<data>("/root/Data");	
			data.setScore(
				data.getScore() + 1
			);
		}
	}

	private void EnemyDeath() {
	
		this.body.QueueFree();
		this.QueueFree();
	}

	


	
}
