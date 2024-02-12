using Godot;
using System;

public partial class start : state
{
	public override void Enter()
	{
		GD.Print("I am on the start node");
		EmitSignal(SignalName.Transitioned, this, "run");
	}
}
