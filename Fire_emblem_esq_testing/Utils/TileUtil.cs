using Godot;

public partial class TileUtil {
    public static void setTiles(TileMap tilemap, Vector2I previous, Vector2I current) {

		tilemap.SetCell(
				3,
				previous,
				7,
				new Vector2I(1, 0)
		);

		tilemap.SetCell(
				3,
				current,
				7,
				new Vector2I(0, 0)
		);

	}

    public static void drawCursor(TileMap tilemap, Vector2I coords) {
        tilemap.SetCell(
            3,
            coords,
            7,
            new Vector2I(0, 0)
        );
    }

    public static void eraseCursor(TileMap tilemap, Vector2I coords) {
        tilemap.EraseCell(
            3,
            coords
        );
    }
}