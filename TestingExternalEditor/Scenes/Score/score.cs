using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class score : Node2D
{

	Label playerScore;
	data data;
	public override void _Ready()
	{
		this.playerScore = this.GetChild<Label>(0);
		this.data = GetNode<data>("/root/Data");
		GD.Print(data.score);
	}


	public override void _Process(double delta)
	{
		this.playerScore.Text = this.data.getScore().ToString();
	}

}
