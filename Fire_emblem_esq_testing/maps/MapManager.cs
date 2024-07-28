using Godot;

public partial class MapManager {

	public static TileMap loadMap(string mapName) {
		PackedScene map = GD.Load<PackedScene>(mapName);
		TileMap instance = map.Instantiate<TileMap>();
		return instance;
	} 
}
