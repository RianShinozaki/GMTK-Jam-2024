extends CharacterBody2D

@export var chefItemDictionary : ChefItemDictionary

var ladderLevel : int

@onready var sprite : Sprite2D = $ItemSprite
@onready var trigger = $TriggerShape

func _drop_item(chef : Chef, food : String, level:int):
	sprite.texture = load(chefItemDictionary.dictionary[food])
	trigger.StatusEffects[food] = .2
	ladderLevel = level
	
	chef.drop = self

func _destroy():
	position.y += 100000000
	queue_free()

func _physics_process(delta):
	# Add the gravity.
	if not is_on_floor():
		velocity += get_gravity() * delta

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	move_and_slide()
