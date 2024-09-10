using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Godot;

public partial class EnemySelectionUtility {
	private List<PlayableCharacter> playableCharacters;

	private EnemyCharacter enemyCharacter;

	private TileMap tileMap;

	private List<AttackMeta> availableAttacks;

	private Dictionary<string, List<Vector2I>> spots;

	private PathUtility pathUtility;

	public EnemySelectionUtility(
		EnemyCharacter enemyCharacter,
		TileMap tileMap
	) {
		this.enemyCharacter = enemyCharacter;
		this.tileMap = tileMap;
		this.pathUtility = new PathUtility(tileMap);
		this.pathUtility.setUpAStarGrid(0);
	}

	/*
		["Fire ball", "Ice ball"]
		[
			["this guy", "that guy", "other guy"],
			["this guy", "that guy"]
		]

		// get all targetable for fireball

		fireballindex = indexof(fireball)
		list[fireballindex]
	 */

	public List<AttackMeta> getAvailableAttacks() {
		return this.availableAttacks;
	}

	/* 
		TODO - this function doesn't account for blocked paths. Will need to rewrite it or utlize another function
		for determining paths that incorporate objects blocking path
	*/

	public List<List<Character>> findTargetsWithinRange(List<PlayableCharacter> characters) {

		Vector2I enemyTileLocation = tileMap.LocalToMap(enemyCharacter.Position);
		this.availableAttacks = AttackSelectionUtility.determineAvailableAttacks(enemyCharacter);
		//.OrderByDescending(attack => attack.attackTargetMeta.range);
		List<List<Character>> targetCandidates = new List<List<Character>>();
		Dictionary<string, List<Vector2I>> targetSpotCandidates = new Dictionary<string, List<Vector2I>>();

		if (availableAttacks.Count() == 0) return targetCandidates; 

		foreach (AttackMeta attack in availableAttacks) {
			List<Character> targetableCandidatesForAttack = new List<Character>();
			List<Vector2I> spots = new List<Vector2I>();

			foreach (PlayableCharacter playableCharacter in characters) {
				Vector2I playableTileLocation = tileMap.LocalToMap(playableCharacter.Position);
				spots = this.findTargetSpotCandidates(enemyTileLocation, playableTileLocation, attack.attackTargetMeta.range);

				if (spots.Count > 0) {
					targetableCandidatesForAttack.Add(playableCharacter);
				}

				targetSpotCandidates.Add(playableCharacter.Name + attack.attackTargetMeta.range.ToString(), spots);
				
			}          
			
			targetCandidates.Add(targetableCandidatesForAttack);

		}

		this.spots = targetSpotCandidates;
		return targetCandidates;
	}

	public Dictionary<string, List<Vector2I>> getSpots() {
		return this.spots;
	}

	public List<Vector2I> findTargetSpotCandidates(Vector2I current, Vector2I target, int range) {

		List<Vector2I> spots = new List<Vector2I>();

		Vector2I negativeX = new Vector2I(target.X - range, target.Y);
		List<Vector2I> negativeXs = new List<Vector2I>(); 
		for(int i = negativeX.X; i < target.X; i++) {
			if (this.isSpotSolid(new Vector2I(i, negativeX.Y))) {
				negativeXs.Clear();
				continue;
			}
			negativeXs.Add(new Vector2I(i, negativeX.Y));
		}

		Vector2I positiveX = new Vector2I(target.X + range, target.Y);
		List<Vector2I> positiveXs = new List<Vector2I>();

		for(int i = positiveX.X; i > target.X; i--) {
			if (this.isSpotSolid(new Vector2I(i, positiveX.Y))) {
				positiveXs.Clear();
				continue;
			}
			positiveXs.Add(new Vector2I(i, positiveX.Y));
		}

		Vector2I negativeY = new Vector2I(target.X, target.Y - range);
		List<Vector2I> negativeYs = new List<Vector2I>();

		for(int i = negativeY.Y; i < target.Y; i++) {
			if (this.isSpotSolid(new Vector2I(negativeY.X, i))) {
				negativeYs.Clear();
				continue;
			}
			negativeYs.Add(new Vector2I(negativeY.X, i));
		}

		Vector2I positiveY = new Vector2I(target.X, target.Y + range);
		List<Vector2I> positiveYs = new List<Vector2I>();

		for(int i = positiveY.Y; i > target.Y; i--) {
			if (this.isSpotSolid(new Vector2I(positiveY.X, i))) {
				positiveYs.Clear();
				continue;
			}
			positiveYs.Add(new Vector2I(positiveY.X, i));
		}

		Vector2I q1 = new Vector2I(target.X + range, target.Y + range);
		List<Vector2I> q1s = new List<Vector2I>();

		for(Vector2I i = q1; i != target; i+=new Vector2I(-1, -1)) {
			if (this.isSpotSolid(i)) {
				q1s.Clear();
				continue;
			}
			q1s.Add(i);
		}
		
		Vector2I q2 = new Vector2I(target.X + range, target.Y - range);
		List<Vector2I> q2s = new List<Vector2I>();

		for(Vector2I i = q2; i != target; i += new Vector2I(-1, 1)) {
			if (this.isSpotSolid(i)) {
				q2s.Clear();
				continue;
			}
			q2s.Add(i);
		}
		
		Vector2I q3 = new Vector2I(target.X - range, target.Y - range);
		List<Vector2I> q3s = new List<Vector2I>();

		for(Vector2I i = q3; i != target; i+= new Vector2I(1, 1)) {
			if (this.isSpotSolid(i)) {
				q3s.Clear();
				continue;
			}
			q3s.Add(i);
		}
		
		Vector2I q4 = new Vector2I(target.X - range, target.Y + range);
		List<Vector2I> q4s = new List<Vector2I>();

		for(Vector2I i = q4; i != target; i+= new Vector2I(1, -1)) {
			if (this.isSpotSolid(i)) {
				q4s.Clear();
				continue;
			}
			q4s.Add(i);
		}


		spots.AddRange(negativeXs);
		spots.AddRange(positiveXs);
		spots.AddRange(negativeYs);
		spots.AddRange(positiveYs);
		spots.AddRange(q1s);
		spots.AddRange(q2s);
		spots.AddRange(q3s);
		spots.AddRange(q4s);
		  
	   
		// we could prolly remove this loop and do the move checks in the individual loops but...
		// eh...this is more readable to me. i'll change it if i have to
		
		List<Vector2I> spotsCopy = new List<Vector2I>(spots);

		foreach (Vector2I spot in spotsCopy)
		{
			// we can use the a star grid path finding instead
			// but i fear that would be too computationally expensive so...
			// let's just estimate ig
			 
			List<Vector2I> path =  this.pathUtility.generatePath(tileMap.MapToLocal(current), tileMap.MapToLocal(target));
			// int numberOfStepsTo = Mathf.Abs(current.Y - target.Y) + Mathf.Abs(current.X - target.X) - (range + 1);
			int numberOfStepsTo = path.Count(); 
			if (numberOfStepsTo > this.enemyCharacter.moveSteps) {
				spots.Remove(spot);
			}          

			
		}
		
		return spots;

	}	

	private bool isSpotSolid(Vector2I spot) {
		// return false;
		return (bool) this.tileMap.GetCellTileData(0, spot).GetCustomData("isSolid");
	}

	public List<Character> findTargetsWithinCloseRange(List<PlayableCharacter> characters) {
		List<Character> targetCandidates = new List<Character>();
		Vector2I enemyTileLocation = tileMap.LocalToMap(enemyCharacter.Position);

		foreach (PlayableCharacter playableCharacter in characters) {
			Vector2I playableTileLocation = tileMap.LocalToMap(playableCharacter.Position);

			int numberOfStepsTo = Mathf.Abs(playableTileLocation.Y - enemyTileLocation.Y) + 
			Mathf.Abs(playableTileLocation.X - enemyTileLocation.X) - 1;
			

			if (numberOfStepsTo <= enemyCharacter.moveSteps) {
				targetCandidates.Add(playableCharacter);
			}        
		}  

		return targetCandidates;
	} 

	
	// this is the circle function 
	public Vector2 findAvailableSpotForTarget(Character character, List<EnemyCharacter> entities, int range) {

		Vector2I centerPosition = this.tileMap.LocalToMap(character.Position);
		Vector2I pointPosition = Vector2I.Zero;
		bool occupied = false;
		float increment = 0.0f;
		
		for (int i = 0; i < 12; i++)
		{
			pointPosition = new Vector2I(
											Mathf.RoundToInt(centerPosition.X + range * Mathf.Cos(Mathf.DegToRad(increment))),
											Mathf.RoundToInt(centerPosition.Y + range * Mathf.Sin(Mathf.DegToRad(increment)))
										);
			
			foreach (Character entity in entities)
			{
				// //GD.Print("ENEMY Position", entity.Position);
				Vector2I entityGlobalPosition = this.tileMap.LocalToMap(entity.Position);
	
				if (pointPosition == entityGlobalPosition) {
					// //GD.Print(centerPosition.X + range * Mathf.Cos(Mathf.DegToRad(increment)));
					// //GD.Print(centerPosition.Y + range * Mathf.Sin(Mathf.DegToRad(increment)));
					// //GD.Print(entityGlobalPosition, pointPosition);
					occupied = true;
				} 

			}
			
			increment+=30;

			if (occupied == false) {
				//GD.Print("Does this run");
				return this.tileMap.MapToLocal(pointPosition); 
			} else {
				occupied = false;
			}
		}
		
		return Vector2.Zero;

	}	



 

	public Character findWeakestDefenceTarget(List<Character> characters) {
		if (enemyCharacter.characterStat.strenth > enemyCharacter.characterStat.magic) {
			return characters.MinBy(character => character.characterStat.physicalDefence);
		} else {
			return characters.MinBy(character => character.characterStat.magicalDefence);
		}
	}

	public void findStrongestTarget() {}
	public void findLowestHealthTarget() {}

	// the area where a given multi attack will have the most effect
	public void findLargestCharacterPool() {}
	
	// like a target u can attack from a distance
	// or without getting hurt back
	public void findSafestToAttackTarget() {}
}



// float distanceBetween = Mathf.Sqrt(
// 	Mathf.Pow(playableCharacter.Position.X - enemyCharacter.Position.X, 2) +                 
// 	Mathf.Pow(playableCharacter.Position.Y - enemyCharacter.Position.Y, 2) 
// );
