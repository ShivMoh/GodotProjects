using Godot;
using System;

public partial class Test_Seeker : CharacterBody2D
{
	[Export]
	CharacterBody2D target;

	[Export]
	NavigationAgent2D navigationAgent2D;
	public override void _Ready()
	{
		this.SeekerSetup();
	}


	public override void _PhysicsProcess(double delta)
	{

		if (navigationAgent2D.TargetPosition != target.Position) {
			navigationAgent2D.TargetPosition = target.Position;
		}

		if (navigationAgent2D.IsNavigationFinished()) return;

		var currentAgentPosition = this.Position;

		var nextPathPosition = navigationAgent2D.GetNextPathPosition();

		Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * 200.0f;

		MoveAndSlide();			
	}

	public async void SeekerSetup() {
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		if (target is not null) {
			navigationAgent2D.TargetPosition = target.Position;
		}
	}
}
