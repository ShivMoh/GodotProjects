using System.Collections.Generic;
using System.Linq;
using Godot;
public partial class OpenWorldStateMachine : StateMachine {



    public override void _EnterTree()
    {
        GD.Print(nameof(CombatStateMachine), " is running");
        this.states.Add(new OpenWorldInitialState());
        this.states.Add(new OpenWorldExploreState());
        this.states.Add(new OpenWorldFinalState());
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
