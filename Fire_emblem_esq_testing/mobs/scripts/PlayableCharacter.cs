using System.Collections.Generic;
using Godot;

public partial class PlayableCharacter : Character{


	public PopupMenu actionsMenu;

	public List<AttackMeta> attacks;
	
	public override void _Ready()
	{
		actionsMenu = this.GetNode<PopupMenu>("PopupMenu");
		actionsMenu.Position = new Vector2I(20, 20);
		targetPosition = this.GlobalPosition;

		this.animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		this.ZIndex = 10;

	
	}

	public void addPopupMenuItem(List<string> items) {
		foreach (string item in items)
		{
			actionsMenu.AddRadioCheckItem(item);
		}
	}

	public void setAttacks(List<AttackMeta> attacks) {
		this.attacks = attacks;
	}

}
