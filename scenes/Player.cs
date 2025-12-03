using Godot;


public partial class Player : CharacterBody2D
{
    [Export]private float MoveSpeed = 200f;

    [Export] private Node2D ActionableFinder;

    private AnimatedSprite2D animatedsprite;

    public bool HasKey = false;

    public override void _Ready()
    {
        animatedsprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    private void HandleAnimation(Vector2 direction){

        if(direction == Vector2.Zero)
        {
            animatedsprite.Stop();
            return;
        }

        string anim = "";

        if(direction.X !=0){
            anim=direction.X>0 ? "walkright" : "walkleft";
        }
        else if(direction.Y != 0)
        {
            anim=direction.Y>0? "walkdown" : "walkup";
        }

        if(animatedsprite.Animation != anim){
            animatedsprite.Play(anim);
        }
    }

    private void GetInput()
    {
        Vector2 inputDirection = Vector2.Zero;
        //jobbra
        if (Input.IsActionPressed("ui_right"))
        {
            inputDirection.X += 1;
        }
        //balra
        if (Input.IsActionPressed("ui_left"))
        {
            inputDirection.X -= 1;
        }
       
        //fel
        if (Input.IsActionPressed("ui_up"))
        {
            inputDirection.Y -= 1;
        }
       

        //le
        if (Input.IsActionPressed("ui_down"))
        {
            inputDirection.Y += 1;
        }

        inputDirection = inputDirection.Normalized();
        
        Velocity = inputDirection * MoveSpeed;

        HandleAnimation(inputDirection);
    }


    public void PickupKey(){
        HasKey = true;
        GD.Print("player objektumban lefutott a pickupkey");
    }

    public override void _UnhandledInput(InputEvent @event){
        GetInput();
        if(Input.IsActionJustPressed("ui_accept")){
            var actionables = ((Area2D)ActionableFinder).GetOverlappingAreas();
            if(actionables.Count>0)
            {
                if(actionables[0] is Actionable action){
                    action.Action();
                }
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        
        MoveAndSlide();
    }

}
