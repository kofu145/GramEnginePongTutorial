using System.Numerics;
using GramEngine.Core;
using GramEngine.ECS;
using GramEngine.ECS.Components;

namespace GramPong.Components;

public class Ball : Component
{
    private Vector3 velocity;
    private float speed;
    private float radius;
    private Entity[] paddles;
    private Sound collisionSound;
    private Sound scoreSound;
    
    public Ball(float radius, float speed, Sound collisionSound, Sound scoreSound)
    {
        this.radius = radius;
        this.speed = speed;
        this.collisionSound = collisionSound;
        this.scoreSound = scoreSound;
    }
    
    public override void Initialize()
    {
        base.Initialize();
        paddles = ParentScene.FindEntitiesWithTag("paddle").ToArray();
        Serve(Side.Right);

    }
    
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Transform.Position += velocity * gameTime.DeltaTime;

    
        // Whenever the ball hits the ceiling or floor, make it bounce. 
        // the if statements are split up so we can just set it to a position where it can't get stuck.
        if (Transform.Position.Y <= radius)
        {
            Transform.Position.Y = radius;
            velocity.Y = -velocity.Y;
            collisionSound.Play();
        }

        if (Transform.Position.Y >= GameStateManager.Window.settings.BaseWindowHeight - radius)
        {
            Transform.Position.Y = GameStateManager.Window.settings.BaseWindowHeight - radius;
            velocity.Y = -velocity.Y;
            collisionSound.Play();
        }
        
        if (Transform.Position.X <= radius)
        {
            Serve(Side.Left);
            paddles[1].GetComponent<Paddle>().Score++;
            scoreSound.Play();
        }
        else if (Transform.Position.X >= GameStateManager.Window.settings.BaseWindowWidth - radius)
        {
            Serve(Side.Right);
            paddles[0].GetComponent<Paddle>().Score++;
            scoreSound.Play();
        }
        
        foreach (var entity in paddles)
        {
            // Could also store components as well to avoid constant getcomponent calls?
            var paddle = entity.GetComponent<Paddle>();
            var paddleX = entity.Transform.Position.X;
            var paddleY = entity.Transform.Position.Y;
            if (CheckCollision(
                    paddleX - paddle.PaddleSize.X / 2 - radius,
                    paddleX + paddle.PaddleSize.X / 2 + radius, 
                    paddleY - paddle.PaddleSize.Y / 2 - radius,
                    paddleY + paddle.PaddleSize.Y / 2 + radius))
            {
                // If the ball has collided with either paddle, make it bounce.
                velocity.X = -velocity.X;
                collisionSound.Play();
            }
        }
    
    }
    
    private bool CheckCollision(float left, float right, float bottom, float top)
    {
        // Shorthands so that our conditional isn't too wordy
        var x = Transform.Position.X;
        var y = Transform.Position.Y;
        return x >= left && x <= right && y >= bottom && y <= top;
    }
    
    private void Serve(Side side)
    {
        Transform.Position = new Vector3(300f, 200f, 1f);
        if (side == Side.Left)
            velocity = new Vector3(-speed, speed, 0f);
        if (side == Side.Right)
            velocity = new Vector3(speed, speed, 0f);
    }
}