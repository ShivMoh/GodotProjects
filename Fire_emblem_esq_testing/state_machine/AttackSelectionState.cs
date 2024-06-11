using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class AttackSelectionState : State {

	private int currentCheckedItem = 0;
	private List<string> popupMenuOptions = new List<string>();

	public override void enter()
	{
		GD.Print("I am on attack selection state");
		popupMenuOptions.Clear();
		foreach (AttackMeta attack in MapEntities.attackMetas)
		{
			GD.Print(attack.name);
			popupMenuOptions.Add(attack.name);
		}
		
		MapEntities.selectedCharacter.actionsMenu.Clear();
		MapEntities.selectedCharacter.addPopupMenuItem(popupMenuOptions);
	}

	public override void physicsUpdate(double _delta)
	{
		MapEntities.selectedCharacter.actionsMenu.Popup();

		if (Input.IsActionJustPressed("select")) {
			int focusedItem = MapEntities.selectedCharacter.actionsMenu.GetFocusedItem();
			MapEntities.selectedCharacter.actionsMenu.SetItemChecked(focusedItem, true);
			if (MapEntities.selectedCharacter.actionsMenu.IsItemChecked(currentCheckedItem)) {
				MapEntities.selectedCharacter.actionsMenu.SetItemChecked(currentCheckedItem, false);
			}

			currentCheckedItem = focusedItem;
			string choice = MapEntities.selectedCharacter.actionsMenu.GetItemText(currentCheckedItem);
			AttackMeta chosenAttack = MapEntities.attackMetas.ElementAt(popupMenuOptions.IndexOf(choice));
			MapEntities.chosenAttack = chosenAttack;

			MapEntities.selectedCharacter.actionsMenu.Hide();
			MapEntities.selectedCharacter.actionsMenu.Clear();

			EmitSignal(SignalName.StateChange, this, "TargetSelectionState");

			// this.processSelectedChoice(choice);

		}
	}


	private void processSelectedChoice(string option) {
		switch (option)
		{
			case "Attack":
				EmitSignal(SignalName.StateChange, this, 3);
			break;
			default:
				return;
		}
	}
}
