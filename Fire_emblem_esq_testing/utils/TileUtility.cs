using Godot;

public partial class TileUtility {

	private TileMap tilemap;
	public TileUtility(TileMap tilemap) {
		this.tilemap = tilemap;
	}

	public void setTiles(Vector2I previous, Vector2I current) {

		tilemap.SetCell(
				1,
				previous,
				9,
				new Vector2I(0, 0)
		);

		tilemap.SetCell(
				1,
				current,
				9,
				new Vector2I(1, 0)
		);

	}

	public void highLight(Vector2I coords, Vector2I tile, int source = 9) {
		tilemap.SetCell(
			1,
			coords,
			source, 
			tile
		);
	}

	public void drawCursor(Vector2I coords) {
		tilemap.SetCell(
			1,
			coords,
			9,
			new Vector2I(1, 0)
		);
	}

	public void eraseCursor(Vector2I coords) {
		tilemap.EraseCell(
			1,
			coords
		);
	}
}
