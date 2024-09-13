extends RigidBody2D


# Called when the node enters the scene tree for the first time.
func _ready():
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta):
	apply_central_force(Vector2(-2000, -980))
			
func _on_detector_body_entered(body):
	if body is PlayerStateMachine:
		
		var direction : Vector2 = Vector2.ZERO

		if body.position.x < self.position.x:
			direction.x = 1
		else:
			direction.x = -1

		if body.position.y < self.position.y:
			direction.y = 1
		else:
			direction.y = -1
		body.current.state_change.emit("wind.gd", null, direction)
