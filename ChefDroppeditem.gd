extends CharacterBody2D

@export var chefItemDictionary : ChefItemDictionary

@onready var sprite : Sprite2D = $ItemSprite
@onready var trigger = $TriggerShape

func _drop_item(food : String):
	sprite.texture = load(chefItemDictionary.dictionary[food])
	trigger.StatusEffects[food] = .2

func _physics_process(delta):
	# Add the gravity.
	if not is_on_floor():
		velocity += get_gravity() * delta

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	move_and_slide()
