extends State

const SPEED = 300.0
const JUMP_VELOCITY = -400.0

var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

var double_tap : float = 0.0
var time_passed_since_previous_tap : float = 0.0

var player_sprite : Sprite2D
var player_raycast : RayCast2D

var direction : float = 1.0

func Enter(_args = []):
	double_tap = 0
	time_passed_since_previous_tap = 0
	player_sprite = player.get_node("sprite")
	player.detect_object_type("test")

func Physics_Update(delta: float):
	
	if not player.is_on_floor():
		player.velocity.y += gravity * delta

	if Input.is_action_just_pressed("lookup"):
		print("I should be rotating")
		rotate_player(1)

	if Input.is_action_just_pressed("lookdown"):
		print("I should be rotating")
		rotate_player(-1)

	if player.is_detecting():
		Transitioned.emit(self, "climb")
		
	if double_tap > 1:
		Transitioned.emit(self, "run")

	if Input.is_action_just_pressed("shoot"):
		Transitioned.emit(self, "shoot", [player_sprite.flip_h])
		
	count_double_tap(delta)

	if Input.is_action_just_pressed("ui_accept"):
		Transitioned.emit(self, "jump")


	if direction:
		player.velocity.x = direction * SPEED

		if direction == 1:
			player_sprite.flip_h = true
		
		if direction == -1:
			player_sprite.flip_h = false
	else:
		player.velocity.x = move_toward(player.velocity.x, 0, SPEED)
	
	direction = Input.get_axis("ui_left", "ui_right")

	player.move_and_slide()

func count_double_tap(delta) -> void:
	if Input.is_action_just_pressed("ui_right") or Input.is_action_just_pressed("ui_left"):
		double_tap += 1
		time_passed_since_previous_tap = 0
	
	time_passed_since_previous_tap += delta
	
	if time_passed_since_previous_tap > (delta * 10):
		double_tap = 0
		
func rotate_player(_direction) -> void:
	self.rotation_degrees += _direction
