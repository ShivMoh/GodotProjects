extends Node2D

const ropePiece = preload("res://ropePiece.tscn")
const endPiece = preload("res://RopeEnd.tscn");

# points
@onready var pointA = get_node("Start")
var pointB = null

var positionY = 0
var mark = 167
var positionXForPinJoint = 0

var RopePices  = []

const force: Vector2 = Vector2(200000, -100000)
const angle: int = 45
const lenghtOfRope: int = 9 * 128

var head = null

var origin: Vector2 = Vector2.ZERO
var marPos: Vector2 = Vector2.ZERO

var firstPinJoint : PinJoint2D = null

func _ready():

	positionXForPinJoint = pointA.position.x + 64
	positionY = pointA.position.y 
	
	origin = get_node("Start").position + Vector2(128, 0)
	marPos = origin
	
	spawnRope(7)
#	spawnEndPiece("head")
#	launch(head)

	#mark = pointA.position
	
func _physics_process(delta):
#	if head:
#		if(calDistanceFromOrigin(head)) > mark:
#			spawnRopePiece()
	if head:
		head.rotation_degrees = 0
		
	if Input.is_action_just_pressed("RotateUp"):
		launch(head)
		
	if Input.is_action_just_pressed("RotateDown"):
		firstPinJoint.queue_free()



func calDistanceFromOrigin(object):
	var x = object.position.x - origin.x
	var y = object.position.y - origin.y
	var h = sqrt(x**2 + y**2)

	return h
	
func getPinJoint():
	
		
	var pinJoint: PinJoint2D = PinJoint2D.new()
	pinJoint.name = "pinJoint"
	pinJoint.position.x = positionXForPinJoint
	pinJoint.position.y = positionY
	pinJoint.node_a = pointA.get_path()
	pinJoint.node_b = pointB.get_path()
	
	positionXForPinJoint += 128
	
	# point A is now the previous point B
	pointA = pointB
	
	# pointB is now null again
	pointB = null
	
	add_child(pinJoint)
	
	if (firstPinJoint == null):
		firstPinJoint = pinJoint
	

func spawnRopePiece():
	var ropePiece_instance: RopePiece = ropePiece.instantiate() 
	ropePiece_instance.name = "ropePiece"
	
	# point B becomes the newly spawned ropePiece
	pointB = ropePiece_instance

	ropePiece_instance.position = marPos
#	var test = (head.position.y - marPos.y) / (head.position.x - marPos.x)
#	var pieceAngle = atan(test)
	
	# ropePiece_instance.rotation_degrees = rad_to_deg(pieceAngle)
	mark += 128
	marPos.x = marPos.x + 128
#	marPos = head.position
#	ropePiece_instance.rotation_degrees = -45
	
	ropePiece_instance.freeze = true
	
	RopePices.append(ropePiece_instance)

	add_child(ropePiece_instance)
	
func spawnEndPiece(name: String, angle: int = 0):
	var endPiece_instance: RopeEnd = endPiece.instantiate()
	endPiece_instance.name = name
	
	# point B becomes the newly spawned ropePiece
	pointB = endPiece_instance
	
#	endPiece_instance.position = Vector2(mark, positionY)
	endPiece_instance.position = marPos
	endPiece_instance.gravity_scale = 0.2
#	mark += 128
	endPiece_instance.mass = 0.79
	endPiece_instance.freeze = true
	
	add_child(endPiece_instance)
	
	head = endPiece_instance
	
	#launch(endPiece_instance)
	
	return endPiece_instance

func spawnRope(numberOfPieces):
	for x in range(0, numberOfPieces):
		spawnRopePiece()
		getPinJoint()
	
	spawnEndPiece("head")
	getPinJoint()
#
	# unfreeze the pinjoints
	for ropePiece in RopePices:
		ropePiece.freeze = false

	head.freeze = false
		
	
func _on_timer_timeout():
	pass

func launch(hook):
	hook.apply_central_force(force)

	
func cal_vector_x():
	return cos(deg_to_rad(angle) ) * lenghtOfRope
	
func cal_vector_y():
	return sin(deg_to_rad(angle)) * lenghtOfRope

func cal_force() -> Vector2: 
	return Vector2(cal_vector_x(), cal_vector_y()) * 500

