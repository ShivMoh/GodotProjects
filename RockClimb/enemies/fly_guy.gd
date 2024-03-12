extends Enemy

@export var distance : Vector2 = Vector2(200.0, 0)
var initial_position : Vector2

func _ready():
	super()
	gravity = 0
	set_target()
	# print(initial_position)
	# print(global_position)

func _physics_process(delta):
	if not is_on_floor():
		velocity.y += gravity * delta

	direction = if_distance()
	# print(direction)
	# print(global_position)
	if direction:
		velocity.x = direction * SPEED
	else:	
		velocity.x = move_toward(velocity.x, 0, SPEED)

	move_and_slide()

func if_distance():
	if abs(initial_position - global_position) >= distance:
		return set_target()

	return direction
	
func set_target():
	if direction == -1:
		initial_position = global_position 
		return 1
	else:
		initial_position = global_position 
		return -1
