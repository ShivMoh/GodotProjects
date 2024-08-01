using Godot;

public enum Direction
{
	RIGHT,

	LEFT,

	UP,

	DOWN,

	NONE
}
public partial class MapManager : Node {
	
	public static Node mapContainer;
	public static TileMap loadMap(string mapName) {
		
		if (MapEntities.map is not null) {
			MapEntities.map.QueueFree();
		}
		
		PackedScene map = GD.Load<PackedScene>(mapName);
		TileMap instance = map.Instantiate<TileMap>();
		MapManager.mapContainer.AddChild(instance);
		MapEntities.map = instance;

		return instance;
	}

	public static Direction determineBounds(Vector2 GlobalPosition) {
		Rect2I rect = MapEntities.map.GetUsedRect();
		Vector2I currentPosition = MapEntities.map.LocalToMap(GlobalPosition);
		//GD.Print(currentPosition, rect.Position, rect.End);
		//GD.Print(GlobalPosition, MapEntities.map.MapToLocal(rect.Position), MapEntities.map.MapToLocal(rect.End));

		if (currentPosition.X < rect.Position.X) {
			return Direction.LEFT;
		} else if ( currentPosition.Y < rect.Position.Y ) {
			return Direction.UP;
		} else if ( currentPosition.X > rect.End.X)	{
			return Direction.RIGHT;
		} else if (currentPosition.Y > rect.End.Y ) {
			return Direction.DOWN;
		} else {
			return Direction.NONE;
		}
		
	}

}
