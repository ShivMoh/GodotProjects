extends State

const SPEED = 300.0
const JUMP_VELOCITY = -400.0

var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

var double_tap : float = 0.0
var time_passed_since_previous_tap : float = 0.0

func Enter():
	print("I am on jump")

func Physics_Update(delta: float) -> void:
	if player.is_on_floor():
		player.velocity.y = JUMP_VELOCITY
		
	Transitioned.emit(self, "start")	
	player.move_and_slide()
