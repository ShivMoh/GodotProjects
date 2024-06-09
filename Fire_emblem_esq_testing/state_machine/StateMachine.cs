using System.Collections.Generic;
using System.Linq;
using Godot;
public partial class StateMachine : Node2D {

	public State currentState;
	public List<State> states = new List<State>();

	[Export]
	public State initialState;
	public override void _Ready()
	{
		foreach (var child in this.GetChildren())
		{   
			if (child is State) {
				states.Add(child as State);

				(child as State).StateChange += onChildTransition;
			}
		}

		if (initialState is not null) {
			initialState.enter();
			currentState = initialState;
		}
	}

	public override void _Process(double delta)
	{
		if (currentState is not null) currentState.update(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (currentState is not null) currentState.physicsUpdate(delta);
	}

	public void onChildTransition(State state, int stateID) {

		if (state != currentState) return;

		State newState = states.FirstOrDefault(state => states.IndexOf(state) == stateID, null);

		if (newState is null) return;

		if (currentState is not null) currentState.exit();

		newState.enter();

		currentState = newState;
	}
}
