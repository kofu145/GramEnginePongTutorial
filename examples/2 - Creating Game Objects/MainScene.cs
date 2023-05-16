using System.Numerics;
using GramEngine.Core;
using GramEngine.ECS;
using GramEngine.ECS.Components;

namespace GramPong;

public class MainScene : GameState
{
    
    public const float PaddleSize = 7f;
    public const float BallSize = 5f;
    
    private Entity ball;
    private Entity leftPlayer;
    private Entity rightPlayer;

    public override void Initialize()  
    {  
        base.Initialize();
        ball = new Entity();  
        leftPlayer = new Entity();  
        rightPlayer = new Entity();  
        
        ball.AddComponent(new Sprite("Content/ball.png"));
        leftPlayer.AddComponent(new Sprite("Content/paddle.png"));
        rightPlayer.AddComponent(new Sprite("Content/paddle.png"));
        
        ball.Transform.Scale = new Vector2(BallSize, BallSize);
        leftPlayer.Transform.Scale = new Vector2(PaddleSize, PaddleSize);
        rightPlayer.Transform.Scale = new Vector2(PaddleSize, PaddleSize);
        
        var paddleWidth = leftPlayer.GetComponent<Sprite>().Width;
        var paddleHeight = leftPlayer.GetComponent<Sprite>().Height;
        
        ball.Transform.Position = new Vector3(300f, 200f, 1f);
        leftPlayer.Transform.Position = new Vector3(paddleWidth * PaddleSize / 2, 
            GameStateManager.Window.Height / 2, 
            1f
            );
        rightPlayer.Transform.Position = new Vector3(
            GameStateManager.Window.Width - paddleWidth * PaddleSize / 2, 
            GameStateManager.Window.Height / 2, 
            1f
        );
        
        AddEntity(leftPlayer);
        AddEntity(rightPlayer);
        AddEntity(ball);
    }
    
    public override void Update(GameTime gameTime)  
    {  
        base.Update(gameTime);
    }
}