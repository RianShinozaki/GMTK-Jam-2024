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

@onready var holdItemDisplace : Node = $Displace
@onready var holdingItemSprite : Sprite2D = $Displace/HeldItem

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

@export var chefItemSprites : ChefItemDictionary
var currentHeldItem : String

var droppedItem : bool
@onready var droppedItemPrefab = preload("res://ChefDroppedItem.tscn")
var drop
@onready var chefParent = %ChefParent

var goalObject : String

@export var currentLadderLevel : int

var hasSavedLadder : bool
var savedLadder : int
var savedLevel : int

@export var currentChefSpeed : float

@onready var music = %MusicManager
@export var ui : ChefUI

var moveTween : Tween

var potInventory : Array[String]

# Called when the node enters the scene tree for the first time.
func _ready():
	music.Resume()
	pass 

func _physics_process(delta):
	velocity += get_gravity() * delta
	if currentChefStatus != ChefStatus.Stun:
		if currentChefStatus == ChefStatus.Move and progressTimer < 0:
			var actualTarget = ladderTarget if currentLadderLevel != targetLocation.ladderLevel else targetLocation
			if droppedItem:
				actualTarget = drop
			
			velocity.x = currentChefSpeed * sign(actualTarget.global_position.x - global_position.x)
			chefSprite.flip_h = 0 > sign(actualTarget.global_position.x - global_position.x)
			holdItemDisplace.scale.x = sign(actualTarget.global_position.x - global_position.x)
			
			if progressTimer < 0 and fmod(abs(progressTimer),1.) < .5 and abs(actualTarget.global_position.x - global_position.x) < 15:
				velocity.x *= -1
			
			
		elif currentChefStatus == ChefStatus.Aggro:
			velocity.x *= .9
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
			chefAnimation.play("walk_hold" if chefItemSprites.dictionary.has(currentHeldItem) else "walk")
		ChefStatus.Move:
			if startTask:
				currentChefStatus = ChefStatus.ActiveTask
				targetLocation._chef_use_station(self, chefTasks[phase].taskList[chefProgress].itemName)
				holdingItemSprite.visible = false
				chefAnimation.play("hold_idle" if chefItemSprites.dictionary.has(currentHeldItem) else "idle")
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
			if chefItemSprites.dictionary.has(currentHeldItem):
				holdingItemSprite.texture = load(chefItemSprites.dictionary[currentHeldItem])
			else:
				holdingItemSprite.texture = null
			
			_next_task()
			chefAnimation.play("hold_idle" if chefItemSprites.dictionary.has(currentHeldItem) else "idle")
		ChefStatus.Stun:
			pass
			#currentChefStatus = ChefStatus.Aggro
			#progressTimer = 5
		ChefStatus.Aggro:
			currentChefStatus = ChefStatus.Idle
			holdingItemSprite.visible = true

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
	
	music.PlayTrack(chefTasks[phase].trackType)
	music.SetTrack(chefTasks[phase].trackType, chefTasks[phase].trackPart)
	music.SetVolume(chefTasks[phase].trackType, chefTasks[phase].trackVolume)
	
	
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
	holdingItemSprite.visible = false
	
	await moveTween.finished
	moveTween.kill()
	moveTween = create_tween().set_trans(Tween.TRANS_LINEAR)
	moveTween.tween_property(self, "global_position", goal, duration)
	
	chefAnimation.play("climb")
	await moveTween.finished
	holdingItemSprite.visible = true
	currentLadderLevel = level
	canClimb = true
	chefAnimation.play("walk_hold" if chefItemSprites.dictionary.has(currentHeldItem) else "walk")

func _on_hurt_box_on_damage_recieved(statusEffects): #Put hurt Chef
	
	if statusEffects.has("Ladder") and currentChefStatus != ChefStatus.Stun and currentChefStatus != ChefStatus.Aggro:
		if currentLadderLevel == statusEffects["Level"]:
			currentLadderLevel = 0
		
		if targetLocation.ladderLevel == statusEffects["Level"] or (statusEffects["Level"] == 0 and targetLocation.ladderLevel != currentLadderLevel and canClimb):
			#print("Climbing!")
			ChefManager.ladderLocations[statusEffects["Ladder"]]._request_climb_ladder(self, statusEffects["Level"])
	
	if statusEffects.has("Hurt"):
		
		
		if chefItemSprites.dictionary.has(currentHeldItem):
			drop = droppedItemPrefab.instantiate()
			#drop = d
			chefParent.add_child(drop)
			
			print(drop)
			drop._drop_item(self, currentHeldItem, currentLadderLevel)
			droppedItem = true
			
			holdingItemSprite.texture = null
			
			drop.global_position = global_position
			findTaskString = currentHeldItem
			currentHeldItem = ""
			
		
		holdingItemSprite.visible = false
		chefAnimation.play("hurt")
		currentChefStatus = ChefStatus.Stun
		progressTimer = .5
		startTask = false
		velocity = statusEffects["Power"] * Vector2(sign(targetLocation.global_position.x - global_position.x),-1)
	
	#print(statusEffects)
	#print(findTaskString)
	
	if statusEffects.has(findTaskString) and currentChefStatus != ChefStatus.Stun and currentChefStatus != ChefStatus.Aggro:
		
		if statusEffects.has("Dropped"):
			print("pick up")
			currentHeldItem = goalObject
			droppedItem = false
			
			drop.queue_free()
			drop._destroy()
			drop = null
			_get_target()
			
			chefAnimation.play("walk_hold")
			holdingItemSprite.visible = true
			if chefItemSprites.dictionary.has(currentHeldItem):
				holdingItemSprite.texture = load(chefItemSprites.dictionary[currentHeldItem])
			else:
				holdingItemSprite.texture = null
		elif !droppedItem:
			startTask = true
			
			progressTimer = statusEffects[findTaskString] #Time to actually start the task
		
	
	
	if statusEffects.has("Remove") and ChefManager.chefCanRemove:
		ChefManager.chefRemove.queue_free()
		ChefManager.chefCanRemove = false

func _reset_progress():
	for i in range(chefProgress):
		if chefTasks[phase].taskList[chefProgress-1]:
			pass

func _on_ground_body_entered(body):
	
	if currentChefStatus == ChefStatus.Stun and progressTimer < 0:
		#print("touch")
		if global_position.y > -100:
			currentLadderLevel = 0
		if drop and currentLadderLevel != drop.ladderLevel:
			droppedItem = false
		
		CameraEffects._screen_shake(Vector2(0,10), 15)
		progressTimer = 3
		currentChefStatus = ChefStatus.Aggro
		chefAnimation.play("hurt_hit")
		await get_tree().create_timer(1).timeout
		if currentChefStatus == ChefStatus.Stun:
			return
		chefAnimation.play("hurt_get_up")
		await get_tree().create_timer(.8).timeout
		if currentChefStatus == ChefStatus.Stun:
			return
		chefSprite.flip_h = !chefSprite.flip_h
		for i in range(3):
			await get_tree().create_timer(.2).timeout
			if currentChefStatus == ChefStatus.Stun:
				return
			chefSprite.flip_h = !chefSprite.flip_h
