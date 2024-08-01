using System;
using System.Linq;
using System.Numerics;
using Godot;
using Vector2 = Godot.Vector2;

public partial class MobCharacter : Character {

    public bool agro = false;

    public float theta = 90.0f;

    Vector2 endPoint = Vector2.Zero;

    public override void _Process(double delta)
    {
    }

    public override void _PhysicsProcess(double delta)
	{
		if (!open) {
			Vector2 velocity = this.Velocity;

			velocity = this.moveTo(velocity);

			this.Velocity = velocity;
			
			MoveAndSlide();
		}

        detectEnemyPresence();
	}

    protected bool detectEnemyPresence() {
        var spaceState = GetWorld2D().DirectSpaceState;
        Vector2 endPoint = calculateVectorDistance(300.0f, theta);

        this.endPoint = endPoint;
       
        var query = PhysicsRayQueryParameters2D.Create(
                                                        this.GlobalPosition, 
                                                        this.GlobalPosition + endPoint,
                                                        CollisionMask,
                                                        new Godot.Collections.Array<Rid> {GetRid()}
                                                    );

        var result = spaceState.IntersectRay(query);

        if (result.Count > 0) {
            if (((Node) (result["collider"])).GetType().Name is nameof(PlayableCharacter)) {
                //GD.Print("FDFSD", this.GlobalPosition);
                //GD.Print("fsfdf", this.GlobalPosition + endPoint);
                //GD.Print("collider detected", result["collider"]);
            }
        }

   
        return true;

    }

    protected Vector2 calculateVectorDistance(float radius, float theta) {
        float thetaInRadians = Mathf.DegToRad(theta);
        Vector2 endPoint;

        if (Mathf.Abs(theta) == 90.0f) {
            endPoint = new Vector2(0.0f, radius * Mathf.Sin(thetaInRadians));
        }
        else if (Mathf.Abs(theta) == 180.0f) {
            endPoint = new Vector2(radius * Mathf.Cos(thetaInRadians), 0.0f);
        } else {
            endPoint = new Vector2(radius * Mathf.Cos(thetaInRadians), radius * Mathf.Sin(theta));
        }

        return endPoint;
    }

}


