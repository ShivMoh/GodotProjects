extends State

const SPEED = 300.0
const JUMP_VELOCITY = -400.0

var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

var double_tap : float = 0.0
var time_passed_since_previous_tap : float = 0.0

var player_sprite : Sprite2D
var player_raycast : RayCast2D

func Enter():
	double_tap = 0
	time_passed_since_previous_tap = 0
	print("I am on start")

	player_sprite = player.get_node("sprite")
	player_raycast = player.get_node("raycast")
	
func Physics_Update(delta: float):
	
	if not player.is_on_floor():
		player.velocity.y += gravity * delta

	if detect_object_type("test"):
		Transitioned.emit(self, "climb")
		
	if double_tap > 1:
		Transitioned.emit(self, "run")

	if Input.is_action_just_pressed("shoot"):
		Transitioned.emit(self, "shoot")
		
	count_double_tap(delta)

	if Input.is_action_just_pressed("ui_accept"):
		Transitioned.emit(self, "jump")

	var direction = Input.get_axis("ui_left", "ui_right")

	if direction:
		player.velocity.x = direction * SPEED

		if direction == 1:
			player_sprite.flip_h = true
			player_raycast.rotation_degrees = 0
		
		if direction == -1:
			player_sprite.flip_h = false
			player_raycast.rotation_degrees = 180
	else:
		player.velocity.x = move_toward(player.velocity.x, 0, SPEED)

	player.move_and_slide()

func count_double_tap(delta) -> void:
	if Input.is_action_just_pressed("ui_right") or Input.is_action_just_pressed("ui_left"):
		double_tap += 1
		time_passed_since_previous_tap = 0
	
	time_passed_since_previous_tap += delta
	
	if time_passed_since_previous_tap > (delta * 10):
		double_tap = 0
		
func detect_object_type(type: String) -> bool:
	# this should check if object is a ladder
	var cast : RayCast2D = player.get_node("raycast")

	if cast.is_colliding:
		var collider = cast.get_collider() 
		if collider != null:
			if collider.is_in_group(type):
				return true

	return false
	
