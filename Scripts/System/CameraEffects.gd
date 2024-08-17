extends Node

var camera : Camera2D
var center : Vector2
var cameraFocus : float

var screenShakePower : Vector2
var maxTimer : int
var timer : int

var moveTween : Tween
var scaleTween : Tween

# Called when the node enters the scene tree for the first time.
func _ready():
	camera = get_viewport().get_camera_2d()
	center = camera.position
	cameraFocus = camera.zoom.x
# Called every frame. 'delta' is the elapsed time since the previous frame.

func _physics_process(delta):
	if timer > 0:
		camera.position = Vector2(randf_range(-screenShakePower.x,screenShakePower.x),randf_range(-screenShakePower.y,screenShakePower.y))*(float(timer)/maxTimer)
		timer -= 1
		

#Just call the method with duration only to reset the position
func _focus_camera(duration : float, pos : Vector2 = center, zoomScale : float = cameraFocus):
	if moveTween:
		moveTween.kill()
	moveTween = create_tween().set_ease(Tween.EASE_IN_OUT).set_trans(Tween.TRANS_BACK)
	moveTween.tween_property(camera, "offset", pos, duration)
	
	if scaleTween:
		scaleTween.kill()
	scaleTween = create_tween().set_ease(Tween.EASE_OUT).set_trans(Tween.TRANS_SINE)
	scaleTween.tween_property(camera, "zoom", Vector2(zoomScale,zoomScale), duration)

func _screen_shake(power : Vector2, time : int):
	screenShakePower = power
	maxTimer = time
	timer = time
