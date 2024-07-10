using System.Collections.Generic;
using System.Linq;
using Godot;
public partial class CombatStateMachine : StateMachine {


    public override void _EnterTree()
    {
		GD.Print(nameof(CombatStateMachine), " is running");

        InitialState firstState = new InitialState();
		FinalState finalState =	new FinalState();

		states.Add(firstState as State);
		states.Add(finalState as State);

		states.AddRange(
			new List<State>() {		
				new ExploreState(),
				new DecisionState(),
				new AttackSelectionState(),
				new TargetSelectionState(),
				new AttackState()
			}
		);

		states.AddRange(
			new List<State>() {
				new EnemyTargetSelectionState(),
				new EnemyAttackSelectionState(),
				new EnemyMoveState(),
				new EnemyAttackState()
			}
		);
		
		
    }

    // public override void _Ready()
	// {
	
	// 	// MapEntities.map = tilemap;
		
	// 	if (initialState is not null) {
	// 		initialState.enter();
	// 		currentState = initialState;
	// 	}
	// }


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
