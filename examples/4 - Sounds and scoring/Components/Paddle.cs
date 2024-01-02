using System.Numerics;
using GramEngine.Core;
using GramEngine.ECS;
using GramEngine.Core.Input;
using GramEngine.ECS.Components;

namespace GramPong.Components;

public enum Side
{
    Left,
    Right
}

public class Paddle : Component
{
    public Side PaddleSide;
    private float speed;
    private float velocity;
    public Vector2 PaddleSize;
    public int Score;

    // The set keybinds to use to control either side.
    private Keys[] leftPlayerInputs = { Keys.W, Keys.S};
    private Keys[] rightPlayerInputs = { Keys.Up, Keys.Down };
    public Paddle(Side side, float speed)
    {
        PaddleSide = side;
        this.speed = speed;
        Score = 0;
    }
    
    public override void Initialize()
    {
        base.Initialize();
        // We store the paddle size so that we can use it when checking collisions in the ball component
        // (otherwise, we'd have to call getcomponent<sprite>() per frame)
        var paddleWidth = ParentEntity.GetComponent<Sprite>().Width;
        var paddleHeight = ParentEntity.GetComponent<Sprite>().Height;

        // We also want to adjust the paddle size for any scaling we might have done
        PaddleSize = new Vector2(paddleWidth * Transform.Scale.X, 
            paddleHeight * Transform.Scale.Y);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    
        // Basic switch statements that moves each paddle to its accordingly bound keys, clamped to screen bounds.
        velocity = 0f;

        switch (PaddleSide)
        {
            case Side.Left:
                if (InputManager.GetKeyPressed(leftPlayerInputs[0]))
                    velocity = -speed;
                if (InputManager.GetKeyPressed(leftPlayerInputs[1]))
                    velocity = speed;
                break;
        
            case Side.Right:
                if (InputManager.GetKeyPressed(rightPlayerInputs[0]))
                    velocity = -speed;
                if (InputManager.GetKeyPressed(rightPlayerInputs[1]))
                    velocity = speed;
                break;
        
            default:
                break;
        }

        Transform.Position.Y += velocity * gameTime.DeltaTime;

        var yOffset = PaddleSize.Y / 2;
        Transform.Position.Y = Math.Clamp(Transform.Position.Y, yOffset, 
            GameStateManager.Window.settings.BaseWindowHeight - yOffset);

    }
    
}