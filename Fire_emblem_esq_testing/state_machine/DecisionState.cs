using Godot;

public partial class DecisionState : State {


	private int currentCheckedItem = 0;

	public override void enter()
	{
		GD.Print("I am deciding things");
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
			this.processSelectedChoice(choice);
			MapEntities.selectedCharacter.actionsMenu.Hide();
		}
	}


	private void processSelectedChoice(string option) {
		switch (option)
		{
			case "Attack":
				EmitSignal(SignalName.StateChange, this, 1);
			break;
			default:
				return;
		}
	}
}
