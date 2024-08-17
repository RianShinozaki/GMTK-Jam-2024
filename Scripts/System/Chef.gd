class_name Chef extends CharacterBody2D

enum ChefStates #we can have a manager that grabs all the neccesary objects the chef refers to
{
	Rest,
	GetPot,
	GetWater,
	GetVegetables,
	GetMeat,
	Cook,
	Serve
}

enum ChefStatus
{
	Idle, #Momentarily Idle before doing next task
	Move, #Moving to next task
	Stun, 
	Aggro, #Dealing with bot
	ActiveTask #Currently doing the task
}

@onready var chefSprite : Sprite2D = $CharacterSprite
@onready var displayAction : Sprite2D = $WantedAction
@onready var debugLabel : Label = $Label

@export var chefTasks : Array[ChefStates]
var targetLocation : Vector2

var chefProgress : int
var findTaskString : String
var startTask : bool

var currentChefStatus : ChefStatus
var progressTimer : float

var currentChefSpeed : float

var moveTween : Tween

const GRAVITY = 200.0

# Called when the node enters the scene tree for the first time.
func _ready():
	pass 

func _physics_process(delta):
	velocity.y += delta * GRAVITY

	var motion = velocity * delta
	move_and_slide()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	_display_current_action()
	progressTimer -= delta
	
	if progressTimer < 0:
		_chef_status_state_tree()
	
	if Input.is_action_just_pressed("ui_accept"):
		
		_get_target()
		position = targetLocation
		CameraEffects._focus_camera(1, targetLocation, 2.)
		CameraEffects._screen_shake(Vector2(0,20), 20)
		chefProgress += 1
	pass

func _chef_status_state_tree():
	match currentChefStatus:
		ChefStatus.Idle:
			currentChefStatus = ChefStatus.Move
		ChefStatus.Move:
			if startTask:
				currentChefStatus = ChefStatus.ActiveTask
				_start_active_task()
		ChefStatus.ActiveTask:
			_next_task()
		ChefStatus.Stun:
			currentChefStatus = ChefStatus.Aggro
			progressTimer = 5
		ChefStatus.Aggro:
			currentChefStatus = ChefStatus.Idle

func _display_current_action():
	match currentChefStatus:
		ChefStatus.Stun:
			debugLabel.text = "Ouch!"
		ChefStatus.Aggro:
			debugLabel.text = "I want to destroy the robtotjh"
		_:
			debugLabel.text = "I want to " + str(chefTasks[chefProgress])

func _start_active_task():
	#anim stuff not sure what to put yet
	progressTimer = 5.
	pass

func _next_task():
	_get_target()
	position = targetLocation
	currentChefStatus = ChefStatus.Idle
	progressTimer = 2.
	

func _get_target():
	match chefTasks[chefProgress]:
		ChefStates.Rest:
			pass
		ChefStates.GetPot:
			targetLocation = ChefManager.potLocation.position
			findTaskString = "Pot"
		ChefStates.GetWater:
			targetLocation = ChefManager.waterLocation.position
			findTaskString = "Water"
		ChefStates.GetVegetables:
			targetLocation = ChefManager.vegetableLocation.position
			findTaskString = "Vegetable"
		ChefStates.GetMeat:
			targetLocation = ChefManager.meatLocation.position
			findTaskString = "Meat"
		ChefStates.Cook:
			targetLocation = ChefManager.cookLocation.position
			findTaskString = "Cook"
		ChefStates.Serve:
			targetLocation = ChefManager.serveLocation.position
			findTaskString = "Serve"


func _on_hurt_box_on_damage_recieved(statusEffects): #Put hurt Chef
	
	if statusEffects.has("Hurt"):
		currentChefStatus = ChefStatus.Stun
		progressTimer = statusEffects["Hurt"]
		startTask = false
	
	if statusEffects.has(findTaskString):
		startTask = true
		progressTimer = 1
	
	
	pass # Replace with function body.
