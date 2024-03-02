extends CharacterBody2D


const SPEED = 100.0
const JUMP_VELOCITY = -400.0

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")


func _physics_process(delta):
	
	if not is_on_floor():
		velocity.y += gravity * delta

	var direction = if_colliding()

	if direction:
		print("hello here?")
		velocity.x = direction * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
	
	move_and_slide()

func if_colliding() -> int:
	var raycast: RayCast2D = self.get_node("raycast")
	
	if raycast.is_colliding:
		var object = raycast.get_collider()

		if object != Player and object != null:
			
			
			if raycast.global_position.x > self.global_position.x:
				return -1
			else: 
				return 1
			
	return -1
