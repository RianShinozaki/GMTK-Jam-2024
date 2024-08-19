class_name Chef extends CharacterBody2D

enum ChefStates #we can have a manager that grabs all the neccesary objects the chef refers to
{
	Rest,
	GetPot,
	GetAlcohol,
	GetSeasoning,
	GetVegetables,
	GetMeat,
	Chop,
	Stove,
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
@onready var chefAnimation : AnimationPlayer = $CharacterSprite/AnimationPlayer
@onready var displayAction : Sprite2D = $WantedAction
@onready var debugLabel : Label = $Label

@onready var holdingItemSprite : Sprite2D = $HeldItem

@export var chefTasks : Array[ChefPhase]


var targetLocation : ChefStation
var ladderTarget : Ladder
var canClimb : bool

var isActive : bool

var phase : int
var phaseTimer : float
var phaseActive : bool

var chefProgress : int
var findTaskString : String
var startTask : bool

var currentChefStatus : ChefStatus
var progressTimer : float

@export var chefItemSprites : Dictionary
var currentHeldItem : String

var goalObject : String

@export var currentLadderLevel : int

@export var currentChefSpeed : float

@export var ui : ChefUI

var moveTween : Tween

var potInventory : Array[String]

# Called when the node enters the scene tree for the first time.
func _ready():
	pass 

func _physics_process(delta):
	velocity += get_gravity() * delta
	if currentChefStatus == ChefStatus.Move and progressTimer < 0:
		var actualTarget = ladderTarget if currentLadderLevel != targetLocation.ladderLevel else targetLocation
		
		velocity.x = currentChefSpeed * sign(actualTarget.position.x - position.x)
		chefSprite.flip_h = 0 > sign(actualTarget.position.x - position.x)
	else:
		velocity.x = 0
	
	#if !moveTween or !moveTween.is_running():
	move_and_slide()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	_display_current_action()
	
	if isActive:
		progressTimer -= delta
	if phaseActive:
		phaseTimer -= delta
		if chefProgress >= chefTasks[phase].taskList.size() and phaseTimer < 0:
			phase += 1
			_start_first_task()
	
	if progressTimer < 0:
		_chef_status_state_tree()
	
	if Input.is_action_just_pressed("ui_accept"):
		
		_start_first_task()
	pass

func _chef_status_state_tree():
	
	match currentChefStatus:
		ChefStatus.Idle:
			currentChefStatus = ChefStatus.Move
			chefAnimation.play("walk")
		ChefStatus.Move:
			if startTask:
				currentChefStatus = ChefStatus.ActiveTask
				targetLocation._chef_use_station(self, chefTasks[phase].taskList[chefProgress].itemName)
				holdingItemSprite.visible = false
				chefAnimation.play("idle")
		ChefStatus.ActiveTask:
			if goalObject == "AddPot":
				potInventory.append(currentHeldItem)
				currentHeldItem = ""
				if !phaseActive:
					phaseActive = true
					phaseTimer = chefTasks[phase].phaseLength
			else:
				currentHeldItem = goalObject
			
			holdingItemSprite.visible = true
			if chefItemSprites.has(currentHeldItem):
				holdingItemSprite.texture = load(chefItemSprites[currentHeldItem])
			else:
				holdingItemSprite.texture = null
			
			_next_task()
			chefAnimation.play("idle")
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
			if chefProgress >= chefTasks[phase].taskList.size():
				debugLabel.text = "Im done"
			else:
				debugLabel.text = "I want to " + str(chefTasks[phase].taskList[chefProgress].itemName)

func _start_first_task():
	isActive = true
	canClimb = true
	phaseActive = false
	startTask = false
	ui._new_phase(chefTasks[phase].phaseName, chefTasks[phase].phaseDescription)
	chefProgress = 0
	_get_target()
	currentChefStatus = ChefStatus.Idle
	progressTimer = 2.

func _next_task():
	chefProgress += 1
	startTask = false
	_get_target()
	currentChefStatus = ChefStatus.Idle
	progressTimer = 2.
	

func _get_target():
	if chefProgress >= chefTasks[phase].taskList.size():
		if phaseActive and phaseTimer < 0:
			phase += 1
			_start_first_task()
		
		print("Done with tasks")
		return
	
	match chefTasks[phase].taskList[chefProgress].station:
		ChefStates.Rest:
			pass
		ChefStates.GetPot:
			targetLocation = ChefManager.potLocation
			findTaskString = "Pot"
		ChefStates.GetAlcohol:
			targetLocation = ChefManager.alcoholLocation
			findTaskString = "Alcohol"
		ChefStates.GetSeasoning:
			targetLocation = ChefManager.seasoningLocation
			findTaskString = "Seasoning"
		ChefStates.GetVegetables:
			targetLocation = ChefManager.vegetableLocation
			findTaskString = "Vegetable"
		ChefStates.GetMeat:
			targetLocation = ChefManager.meatLocation
			findTaskString = "Meat"
		ChefStates.Chop:
			targetLocation = ChefManager.chopLocation
			findTaskString = "Chop"
		ChefStates.Stove:
			targetLocation = ChefManager.stoveLocation
			findTaskString = "Stove"
		ChefStates.Serve:
			targetLocation = ChefManager.serveLocation
			findTaskString = "Serve"
	
	if targetLocation.ladderLevel != 0 and currentLadderLevel != targetLocation.ladderLevel:
		ladderTarget = ChefManager.ladderLocations[targetLocation.ladderLevel]

func _start_active_task(timeRequired : float, stationType : ChefStation.ChefStationType, goal : String):
	progressTimer = timeRequired
	
	goalObject = goal
	
	#add animation here
	match stationType:
		ChefStation.ChefStationType.Grab:
			pass
		ChefStation.ChefStationType.Stir:
			pass
		ChefStation.ChefStationType.Cook:
			pass
		ChefStation.ChefStationType.Cut:
			pass

func _climb_ladder(level : int, snap : Vector2 ,goal : Vector2, duration : float):
	if moveTween:
		moveTween.kill()
	moveTween = create_tween().set_trans(Tween.TRANS_LINEAR)
	moveTween.tween_property(self, "global_position", snap, .05)
	canClimb = false
	await moveTween.finished
	moveTween.kill()
	moveTween = create_tween().set_trans(Tween.TRANS_LINEAR)
	moveTween.tween_property(self, "global_position", goal, duration)
	chefAnimation.play("climb")
	await moveTween.finished
	currentLadderLevel = level
	canClimb = true
	chefAnimation.play("walk")

func _on_hurt_box_on_damage_recieved(statusEffects): #Put hurt Chef
	
	if statusEffects.has("Ladder"):
		if targetLocation.ladderLevel == statusEffects["Level"] or (statusEffects["Level"] == 0 and targetLocation.ladderLevel != currentLadderLevel and canClimb):
			print("Climbing!")
			ChefManager.ladderLocations[statusEffects["Ladder"]]._request_climb_ladder(self, statusEffects["Level"])
	
	if statusEffects.has("Hurt"):
		currentChefStatus = ChefStatus.Stun
		progressTimer = statusEffects["Hurt"]
		startTask = false
	
	print(statusEffects)
	print(findTaskString)
	
	if statusEffects.has(findTaskString):
		startTask = true
		
		progressTimer = statusEffects[findTaskString] #Time to actually start the task
		
	
	
	pass # Replace with function body.
