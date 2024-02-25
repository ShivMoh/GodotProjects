extends State

const SPEED = 300.0
const JUMP_VELOCITY = -400.0

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

var double_tap : int = 0.0
var time_passed_since_previous_tap : float = 0.0

func Enter():
	double_tap = 0
	time_passed_since_previous_tap = 0
	print("I am on Start")
	
func Physics_Update(delta: float):
	
	# Add the gravity.
	if not player.is_on_floor():
		player.velocity.y += gravity * delta
		
	if player.is_on_wall():
		Transitioned.emit(self, "climb")
		
	if double_tap > 1:
		Transitioned.emit(self, "run")
		
	if Input.is_action_just_pressed("shoot"):
		Transitioned.emit(self, "shoot")
		
	count_double_tap(delta)

	# Handle Jump.
	if Input.is_action_just_pressed("ui_accept") and player.is_on_floor():
		player.velocity.y = JUMP_VELOCITY

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction = Input.get_axis("ui_left", "ui_right")
	if direction:
		player.velocity.x = direction * SPEED
	else:
		player.velocity.x = move_toward(player.velocity.x, 0, SPEED)

	player.move_and_slide()

func count_double_tap(delta):
	if Input.is_action_just_pressed("ui_right") or Input.is_action_just_pressed("ui_left"):
		double_tap += 1
		time_passed_since_previous_tap = 0
	
	time_passed_since_previous_tap += delta
	
	if time_passed_since_previous_tap > (delta * 10):
		double_tap = 0
		
