using Godot;
using System;

public partial class mage_inherited : Node3D
{
	
	public AnimationPlayer animation;
	public override void _Ready()
	{
		animation = GetNode<AnimationPlayer>("AnimationPlayer");
	}



}
