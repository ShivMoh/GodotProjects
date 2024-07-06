using System.Collections.Generic;
using System.Linq;
using Godot;
public partial class OpenWorldStateMachine : StateMachine {



    public override void _EnterTree()
    {
        GD.Print("State machine is machining i suppose");
        this.states.Add(new OpenWorldInitialState());
        this.states.Add(new OpenWorldExploreState());
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
