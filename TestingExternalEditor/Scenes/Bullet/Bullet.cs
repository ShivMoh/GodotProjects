using Godot;
using System;

public partial class Bullet : Area2D
{

	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
	{
		
	}

	public void CollideWithBody(string target) {
		
		this.BodyEntered += (body) => {
			
			if(body.GetGroups().Contains(target)) {
				GD.Print(target);
				health_bar health = body.GetNodeOrNull<health_bar>("HealthBar");
				health.handleDamage();
				this.QueueFree();
			}
		};
	}
}
