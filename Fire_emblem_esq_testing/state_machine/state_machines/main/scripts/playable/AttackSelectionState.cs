using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class AttackSelectionState : State {

	private int currentCheckedItem = 0;
	private List<string> popupMenuOptions = new List<string>();

	private PlayableCharacter selectedCharacter;
	public override void enter()
	{
		//GD.Print("I am on attack selection state");
		//GD.Print("Stats", MapEntities.characters.Count(), MapEntities.playableCharacters.Count(), MapEntities.enemyCharacters.Count());

		this.selectedCharacter = MapEntities.selectedCharacter as PlayableCharacter;
		
		popupMenuOptions.Clear();
		foreach (AttackMeta attack in MapEntities.attackMetas)
		{
			popupMenuOptions.Add(attack.name);
		}

		popupMenuOptions.Add("Back");
		
		this.selectedCharacter.actionsMenu.Clear();
		this.selectedCharacter.addPopupMenuItem(popupMenuOptions);

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

			if (choice == "Back") {
				// MapEntities.map.ClearLayer(1);
				EmitSignal(SignalName.StateChange, this, nameof(DecisionState));
			}

			AttackMeta chosenAttack = MapEntities.attackMetas.ElementAt(popupMenuOptions.IndexOf(choice));
			MapEntities.chosenAttack = chosenAttack;

			this.selectedCharacter.actionsMenu.Hide();
			this.selectedCharacter.actionsMenu.Clear();

			EmitSignal(SignalName.StateChange, this, typeof(TargetSelectionState).ToString());

		}

		if (Input.IsActionJustPressed("cancel")) {
			// MapEntities.map.ClearLayer(1);
			EmitSignal(SignalName.StateChange, this, nameof(DecisionState));
		}
	}
}
