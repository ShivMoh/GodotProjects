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
				7,
				new Vector2I(1, 0)
		);

		tilemap.SetCell(
				1,
				current,
				7,
				new Vector2I(0, 0)
		);

	}

	public void drawCursor(Vector2I coords) {
		tilemap.SetCell(
			1,
			coords,
			7,
			new Vector2I(0, 0)
		);
	}

	public void eraseCursor(Vector2I coords) {
		tilemap.EraseCell(
			1,
			coords
		);
	}
}
