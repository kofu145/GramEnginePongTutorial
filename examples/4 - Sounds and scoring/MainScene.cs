using System.Numerics;
using GramEngine.Core;
using GramEngine.ECS;
using GramEngine.ECS.Components;
using GramPong.Components;

namespace GramPong;

public class MainScene : GameState
{
    
    public const float PaddleSize = 7f;
    public const float BallSize = 5f;
    
    public const float BallSpeed = 300f;
    public const float PaddleSpeed = 500f;
    
    private Entity ball;
    private Entity leftPlayer;
    private Entity rightPlayer;
    private Entity ballCollisionSound;
    private Entity ballScoreSound;
    private Entity scoreKeeperEntity;

    public override void Initialize()  
    {  
        base.Initialize();
        ball = new Entity();  
        leftPlayer = new Entity();  
        rightPlayer = new Entity();
        ballCollisionSound = new Entity();
        ballScoreSound = new Entity();
        scoreKeeperEntity = new Entity();
        
        leftPlayer.Tag = "paddle";
        rightPlayer.Tag = "paddle";
        
        ball.AddComponent(new Sprite("Content/ball.png"));
        leftPlayer.AddComponent(new Sprite("Content/paddle.png"));
        rightPlayer.AddComponent(new Sprite("Content/paddle.png"));
        
        ballCollisionSound.AddComponent(new Sound("Content/blipSelect.wav"));
        ballScoreSound.AddComponent(new Sound("Content/score.wav"));
        
        leftPlayer.AddComponent(new Paddle(Side.Left, PaddleSpeed));
        rightPlayer.AddComponent(new Paddle(Side.Right, PaddleSpeed));
        ball.AddComponent(new Ball(12.5f, BallSpeed, 
            ballCollisionSound.GetComponent<Sound>(), 
            ballScoreSound.GetComponent<Sound>()));

        scoreKeeperEntity.AddComponent(new TextComponent("text", "Content/square.ttf", 50));
        
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
        scoreKeeperEntity.Transform.Position = new Vector3(260, 20, 1f);
        
        AddEntity(leftPlayer);
        AddEntity(rightPlayer);
        AddEntity(ball);
        AddEntity(ballCollisionSound);
        AddEntity(ballScoreSound);
        AddEntity(scoreKeeperEntity);
    }
    
    public override void Update(GameTime gameTime)  
    {  
        base.Update(gameTime);
        scoreKeeperEntity.GetComponent<TextComponent>().Text = 
            $"{leftPlayer.GetComponent<Paddle>().Score} | {rightPlayer.GetComponent<Paddle>().Score}";
    }
}