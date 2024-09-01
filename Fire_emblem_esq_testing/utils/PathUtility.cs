using System.Collections.Generic;
using Godot;

public partial class PathUtility {

    public TileMap tileMap;

    public AStarGrid2D aStarGrid2D;

    public List<Vector2I> path;

    public PathUtility(
        TileMap tileMap
    ) {
        this.tileMap = tileMap;
        this.aStarGrid2D = new AStarGrid2D();
        this.path = new List<Vector2I>();
     
    }

    public void setUpAStarGrid(int layer) {
        this.aStarGrid2D.Region = this.tileMap.GetUsedRect();
        this.aStarGrid2D.CellSize = new Vector2(64, 64);
        this.aStarGrid2D.DefaultComputeHeuristic = AStarGrid2D.Heuristic.Manhattan;
        this.aStarGrid2D.DefaultEstimateHeuristic = AStarGrid2D.Heuristic.Manhattan;
        this.aStarGrid2D.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;

        this.update();

        foreach(Vector2I cell in this.tileMap.GetUsedCells(layer)) {
            bool solid = isSpotSolid(cell);
            if (solid) {
                this.aStarGrid2D.SetPointSolid(cell, true);
            }
        }

    }

    public List<Vector2I> generatePath(
            Vector2 currentPosition, 
            Vector2 targetPosition,
            int count = 20,
            int layer = 1
        ) {

        this.tileMap.ClearLayer(layer);
        this.path.Clear(); 

        foreach(Vector2I coord in aStarGrid2D.GetIdPath(tileMap.LocalToMap(currentPosition), tileMap.LocalToMap(targetPosition))) {
			this.path.Add(coord);
		}
        
        if (this.path.Count > count) {
            this.path.Clear();
        }

        return this.path;
    }

    public void showPath(int layer = 1, int source = 0, int tileX = 2, int tileY = 1) {
        foreach (Vector2I cell in this.path)
        {
            tileMap.SetCell(layer, cell, source, new Vector2I(tileX,tileY));
        }
    }

    public void update() {
        this.aStarGrid2D.Update();
    }


	private bool isSpotSolid(Vector2I spot) {
		return (bool) this.tileMap.GetCellTileData(0, spot).GetCustomData("isSolid");
	}
}
