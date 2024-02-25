extends State


@onready var point : PinJoint2D = null
var target  = null

var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

var raycast : RayCast2D

var locked_object = null

var pull : bool = false

func Enter():

	raycast = player.get_node("beam")
	point = player.get_child(2) as PinJoint2D
	target = get_node("Target")
#	print(get_parent().get_parent().get_path())
#	point.node_a = get_parent().get_parent().get_path()
#	target = get_parent().get_parent().get_parent().get_node("Target")
#	point.node_b = target.get_path()

func Physics_Update(delta: float):
	
	if Input.is_action_just_pressed("shoot"):
		shoot_rope()
		
	if locked_object:
		pull_up(delta)
#	
	basic_movement(delta)     
	player.move_and_slide()

func pull_up(delta):
	if locked_object:
		if Input.is_action_just_pressed("ui_text_delete"):
			player.velocity = Vector2(1500, -500)
			locked_object = null
		
func shoot_rope():
	var collider = raycast.get_collider()
	if collider is Node and not locked_object:
		if collider.is_in_group("test"):
			connect_rope(collider)

func connect_rope(object):
	print("rope connected")
	point.node_b = object.get_path()
	locked_object = object
		
			
func calVeclocity(player : Vector2, target : Vector2):
	var distanceX = abs(player.x - target.x)
	var velocityX = distanceX / 2
	print(distanceX)
	var distanceY = player.y - target.y
	print(distanceY)
	var velocityY = distanceY / 2
	return Vector2(velocityX, velocityY)
	
func calculate_ray_cast_endpoint(a = 1000):
	var o = tan(deg_to_rad(player.rotation_degrees))
	var x = player.position.x - a
	var y = player.position.y - o
	return Vector2(x, y)
	
func basic_movement(delta):
	# Add the gravity.
	if not player.is_on_floor():
		player.velocity.y += gravity * delta

	# Handle Jump.
	if Input.is_action_just_pressed("ui_accept") and player.is_on_floor():
		player.velocity.y = -400
	
	if Input.is_action_pressed("lookup"):
		player.rotation_degrees += 1
		
	if Input.is_action_pressed("lookdown"):
		player.rotation_degrees -= 1
		
	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction = Input.get_axis("ui_left", "ui_right")
	if direction:
		player.velocity.x = direction * 300
	else:
		player.velocity.x = move_toward(player.velocity.x, 0, 300)

	player.move_and_slide()
	
func detect_target_code():
	var target_position = calculate_ray_cast_endpoint()
	var space_state = player.get_world_2d().direct_space_state
	var query = PhysicsRayQueryParameters2D.create(player.global_position, target_position,
	player.collision_mask, [player])
	var result = space_state.intersect_ray(query)

	if result:
		print(result['collider'].is_in_group("test"))
