using System.Collections.Generic;
using Godot;

public partial class FinalState : State {

	public override void enter()
	{
		MapEntities.selectedCharacter.usedTurn = true;
		MapEntities.selectedCharacter = null;
		MapEntities.count++;
		MapEntities.detectedEnemies.Clear();
		MapEntities.targetedCharacters.Clear();
	}

	public override void physicsUpdate(double _delta)
	{
		EmitSignal(SignalName.StateChange, this, "ExploreState");
	}
}
