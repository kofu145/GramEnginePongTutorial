﻿using System.Numerics;
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

    public override void Initialize()  
    {  
        base.Initialize();
        ball = new Entity();  
        leftPlayer = new Entity();  
        rightPlayer = new Entity();  
        
        leftPlayer.Tag = "paddle";
        rightPlayer.Tag = "paddle";
        
        ball.AddComponent(new Sprite("Content/ball.png"));
        leftPlayer.AddComponent(new Sprite("Content/paddle.png"));
        rightPlayer.AddComponent(new Sprite("Content/paddle.png"));
        
        leftPlayer.AddComponent(new Paddle(Side.Left, PaddleSpeed));
        rightPlayer.AddComponent(new Paddle(Side.Right, PaddleSpeed));
        ball.AddComponent(new Ball(12.5f, BallSpeed));

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