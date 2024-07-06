using Godot;

public partial class StateMachineManager : Node {
	
	[Export]
	TileMap tilemap;
	
	public override void _Ready()
	{
		MapEntities.map = tilemap;
		AddChild(new OpenWorldStateMachine());
	}

}
