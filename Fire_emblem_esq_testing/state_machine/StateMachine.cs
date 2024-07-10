using System.Collections.Generic;
using System.Linq;
using Godot;
public partial class StateMachine : Node2D {

	public State currentState;
	public List<State> states = new List<State>();
	public State initialState;

	[Signal]
	public delegate void StateMachineChangeEventHandler(StateMachine currentStateMachine, string stateMachineName);
	
	public override void _Ready()
	{
		

		// foreach (var child in this.GetChildren())
		// {   
		// 	if (child.GetChildCount() != 0) {
		// 		foreach (var subChild in child.GetChildren()) {
		// 			if (subChild is State) {
		// 				states.Add(subChild as State);

		// 				(subChild as State).StateChange += onChildTransition;
		// 				// (child as State).ShareAttack += setAttack;
		// 				// (child as State).ShareAttack += setAttacks;
		// 			}
		// 		}
		// 	} else {
		// 		if (child is State) {
		// 			states.Add(child as State);
		// 			(child as State).StateChange += onChildTransition;
		// 			// (child as State).ShareAttack += setAttack;
		// 			// (child as State).ShareAttack += setAttacks;
		// 		}
		// 	}
		
		// }
		initialState = states.First();

		foreach (State state in this.states)
		{
			state.StateChange += onChildTransition;
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
		if (currentState.switchStateMachine) { 
			EmitSignal(SignalName.StateMachineChange, this, this.GetType().Name);
		} else if (currentState is not null) {
			currentState.physicsUpdate(delta);
		} 
	}

	public virtual void onChildTransition(State state, string stateName) {

		if (state != currentState) return;

		State newState = states.FirstOrDefault(state => state.GetType().Name == stateName, null);

		if (newState is null) return;

		if (currentState is not null) currentState.exit();

		newState.previousStateName = state.GetType().Name;
		
		// this.processStateDecisions(newState);
		newState.enter();

		currentState = newState;
	}

	// public void setSelectedCharacter(PlayableCharacter selectedCharacter) {
	// 	this.selectedCharacter = selectedCharacter;
	// }

	// private void processStateDecisions(State state) {
	// 	switch (state.GetType().Name)
	// 	{
	// 		case "DecisionState":
	// 			if (selectedCharacter is not null) {
	// 				(state as DecisionState).setSelectedCharacter(selectedCharacter);
	// 			}
	// 			GD.Print("selected character", selectedCharacter);
	// 			break;
	// 		default:
	// 			GD.Print("Nothing to do here");
	// 			break;
	// 	}
	// }
}
