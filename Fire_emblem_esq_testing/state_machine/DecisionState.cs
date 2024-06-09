using Godot;

public partial class DecisionState : State {

	private PlayableCharacter selectedCharacter;

	private int currentCheckedItem = 0;

	public override void enter()
	{
		// this.ShareCharacter += setSelectedCharacter;
		GD.Print("I am deciding things");
	}
	public override void physicsUpdate(double _delta)
	{
		selectedCharacter.actionsMenu.Popup();

		if (Input.IsActionJustPressed("select")) {
			int focusedItem = selectedCharacter.actionsMenu.GetFocusedItem();
			selectedCharacter.actionsMenu.SetItemChecked(focusedItem, true);
			if (selectedCharacter.actionsMenu.IsItemChecked(currentCheckedItem)) {
				selectedCharacter.actionsMenu.SetItemChecked(currentCheckedItem, false);
			}

			currentCheckedItem = focusedItem;
		}
	}

	public void setSelectedCharacter(PlayableCharacter selectedCharacter) {
		this.selectedCharacter = selectedCharacter;
		GD.Print("Selected character is being set");
	}
}
