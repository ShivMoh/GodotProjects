using Godot;
using System;

public partial class knight : Node3D
{
	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
		AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("1H_Melee_Attack_Chop");
	}
}
