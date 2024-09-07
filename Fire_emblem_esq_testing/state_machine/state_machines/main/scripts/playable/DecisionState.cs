using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class DecisionState : State {


	private int currentCheckedItem = 0;

	private PlayableCharacter selectedCharacter;
	public override void enter()
	{
		//GD.Print("I am on decision state");
		this.selectedCharacter = MapEntities.selectedCharacter as PlayableCharacter;
		this.selectedCharacter.actionsMenu.Clear();
		this.selectedCharacter.addPopupMenuItem(new List<string>() {"Attack", "Ability", "Use", "End Turn"});
		//GD.Print("Stats", MapEntities.characters.Count(), MapEntities.playableCharacters.Count(), MapEntities.enemyCharacters.Count());

	}
	public override void physicsUpdate(double _delta)
	{
		this.selectedCharacter.actionsMenu.Popup();

		if (Input.IsActionJustPressed("select")) {
			int focusedItem = this.selectedCharacter.actionsMenu.GetFocusedItem();
			this.selectedCharacter.actionsMenu.SetItemChecked(focusedItem, true);

			if (this.selectedCharacter.actionsMenu.IsItemChecked(currentCheckedItem)) {
				this.selectedCharacter.actionsMenu.SetItemChecked(currentCheckedItem, false);
			}

			currentCheckedItem = focusedItem;
			string choice = this.selectedCharacter.actionsMenu.GetItemText(currentCheckedItem);
			this.processSelectedChoice(choice);
			this.selectedCharacter.actionsMenu.Hide();
			// this.selectedCharacter.usedTurn = true;
		}
	}


	private void processSelectedChoice(string option) {
		switch (option)
		{
			case "Attack":
				MapEntities.attackMetas = this.selectedCharacter.attacks;
				EmitSignal(SignalName.StateChange, this, nameof(AttackSelectionState));
				break;
			case "End Turn":
				EmitSignal(SignalName.StateChange, this, nameof(FinalState));
				break;
			default:
				return;
		}
	}
}
