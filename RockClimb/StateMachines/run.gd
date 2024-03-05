extends State

const SPEED = 700.0
const JUMP_VELOCITY = -500

var delta_time = 0

func Enter(_args = []):
	delta_time = 0
	print("I am on Run")

func Physics_Update(delta: float):
	
	var direction = Input.get_axis("ui_left", "ui_right")

	player.velocity.x = direction * SPEED
	
	if delta_time > (delta * 30):
		Transitioned.emit(self, "start")
	
	if Input.is_action_just_released("ui_right") or Input.is_action_just_released("ui_left"):
		Transitioned.emit(self, "start")
		
	if Input.is_action_just_pressed("ui_accept"):
		player.velocity.y = JUMP_VELOCITY
		Transitioned.emit(self, "start")
	
	delta_time += delta
		
	player.move_and_slide()
