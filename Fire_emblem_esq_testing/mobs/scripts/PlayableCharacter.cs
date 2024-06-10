using Godot;

public partial class PlayableCharacter : Character{

	public PopupMenu actionsMenu;
	public override void _Ready()
	{
		actionsMenu = this.GetNode<PopupMenu>("PopupMenu");
		// actionsMenu.Show();
		actionsMenu.Position = new Vector2I(20, 20);

		// actionsMenu.Popup();

		targetPosition = this.GlobalPosition;

		this.animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		this.ZIndex = 10;
	}


}
