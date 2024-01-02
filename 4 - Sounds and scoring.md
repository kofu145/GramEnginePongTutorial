# Finishing Up
Now that we've finished up implementing the last touches on gameplay, it's time to add up some scores and sounds!

> NOTE: Implementation details within this section are due to change. If you have issues in this section, they are likely due to API changes. Message kofu145 on discord OR pull up an issue in this github repository to notify me. Thanks!

In pong, we usually want the ball to make a small "blip" sound whenever it collides with something - that being a paddle or a wall. We'll also want a little sound that plays upon scoring, for cool effect. If you recall though, we already have code that checks for both of these things!

To start off, we'll be creating two new entities holding ``Sound`` ``Component``s we'll be using to play our collision and scoring sounds. In ``MainScene.cs``, hit up our entities again and let's make new ones:

```cs
    //...
    private Entity ball;
    private Entity leftPlayer;
    private Entity rightPlayer;
    // NEW
    private Entity ballCollisionSound;
    private Entity ballScoreSound;

    public override void Initialize()  
    {  
        base.Initialize();
        ball = new Entity();  
        leftPlayer = new Entity();  
        rightPlayer = new Entity();

        // NEW
        ballCollisionSound = new Entity();
        ballScoreSound = new Entity();
        
    //...
```
With our new entities created, we'll need to make sure we actually add them to our ``GameState`` scene:

```cs
//...
    AddEntity(leftPlayer);
    AddEntity(rightPlayer);
    AddEntity(ball);
    // NEW
    AddEntity(ballCollisionSound);
    AddEntity(ballScoreSound);
}
//...
```

With our entities created, let's go ahead and add our ``Sound`` Components! To start off though, add the following files to your "Content" Folder.

[blipSelect sound](sounds/blipSelect.wav) <br>
[score sound](sounds/score.wav)

Your Content folder should be looking like this:
```
Content
├── ball.png
├── blipSelect.wav
├── paddle.png
├── score.wav
```

Now we use these wav files to make our ``Sound`` Components:

```cs
//...
public override void Initialize()
{
    //...
    ball.AddComponent(new Sprite("Content/ball.png"));
    leftPlayer.AddComponent(new Sprite("Content/paddle.png"));
    rightPlayer.AddComponent(new Sprite("Content/paddle.png"));

    // NEW
    ballCollisionSound.AddComponent(new Sound("Content/blipSelect.wav"));
    ballScoreSound.AddComponent(new Sound("Content/score.wav"));

    leftPlayer.AddComponent(new Paddle(Side.Left, PaddleSpeed));
    rightPlayer.AddComponent(new Paddle(Side.Right, PaddleSpeed));

//...
```

With these components added, we finally have sounds set up! All we have to do now is to call the ``Play`` method on each of the ``Sound`` components whenever they're expected. 

However, all of the instances in which they are expected are all checked under ``Ball.cs``. In order to play these sounds in their expected areas then, we'll have to add references in our ``Ball`` component. Simply amend the ``Ball`` constructor in ``Ball.cs`` as so:

```cs
//...
public class Ball : Component
{
    private Vector3 velocity;
    private float speed;
    private float radius;
    private Entity[] paddles;
    // NEW
    private Sound collisionSound;
    private Sound scoreSound;
    
    // NEW
    public Ball(float radius, float speed, Sound collisionSound, Sound scoreSound)
    {
        this.radius = radius;
        this.speed = speed;
        // NEW
        this.collisionSound = collisionSound;
        this.scoreSound = scoreSound;
    }
//...
```

Now that we've added new arguments to our ``Ball`` component's constructor, we'll have to add them back in ``MainScene.cs``:

```cs
//...
public override void Initialize()  
{  
    //...
    leftPlayer.AddComponent(new Paddle(Side.Left, PaddleSpeed));
    rightPlayer.AddComponent(new Paddle(Side.Right, PaddleSpeed));
    // NEW
    ball.AddComponent(new Ball(12.5f, BallSpeed, 
        ballCollisionSound.GetComponent<Sound>(), 
        ballScoreSound.GetComponent<Sound>()));
//...
```

All that's left now is to play the sounds in all the places where we expect them to be played! 
In ``Ball.cs``, amend the following areas to have ``Play()`` method calls:
```cs
//...
public override void Update(GameTime gameTime)
{
    //...

    // Whenever the ball hits the ceiling or floor, make it bounce. 
    // the if statements are split up so we can just set it to a position where it can't get stuck.
    if (Transform.Position.Y <= radius)
    {
        Transform.Position.Y = radius;
        velocity.Y = -velocity.Y;
        // NEW
        collisionSound.Play();
    }

    if (Transform.Position.Y >= GameStateManager.Window.settings.BaseWindowHeight - radius)
    {
        Transform.Position.Y = GameStateManager.Window.settings.BaseWindowHeight - radius;
        velocity.Y = -velocity.Y;
        // NEW
        collisionSound.Play();
    }

    if (Transform.Position.X <= radius)
    {
        Serve(Side.Left);
        paddles[1].GetComponent<Paddle>().Score++;
        // NEW
        scoreSound.Play();
    }
    else if (Transform.Position.X >= GameStateManager.Window.settings.BaseWindowWidth - radius)
    {
        Serve(Side.Right);
        paddles[0].GetComponent<Paddle>().Score++;
        // NEW
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
            // NEW
            collisionSound.Play();
        }
    }
//...
```
That's it for sounds! Now, whenever you go ahead and play the game, you'll hear some sweet sounds upon hitting the ball and scoring!

Now we'll finally move on to displaying our scoreboard. Recall that when we made the ``Paddle`` component, we also gave it a score variable that was incremented whenever the ball entered a player's area. In order to show the score, we'll simply display this at the top of the game.

To start off, we'll need a font for the text that we'll be displaying. Download the following and put it in your Content folder.

[square.ttf font](font/square.ttf)

With that out of the way, we'll create a new entity to house the ``TextComponent`` that will display our scores. In ``MainScene.cs``:

```cs
//...
private Entity ball;
private Entity leftPlayer;
private Entity rightPlayer;
private Entity ballCollisionSound;
private Entity ballScoreSound;
// NEW
private Entity scoreKeeperEntity;

public override void Initialize()  
{  
    base.Initialize();
    ball = new Entity();  
    leftPlayer = new Entity();  
    rightPlayer = new Entity();
    ballCollisionSound = new Entity();
    ballScoreSound = new Entity();
    // NEW
    scoreKeeperEntity = new Entity();

//...
```
Don't forget, we'll also need to add our entity to our ``Scene``. At the end of the ``Initialize`` method:
```cs
//...
    AddEntity(leftPlayer);
    AddEntity(rightPlayer);
    AddEntity(ball);
    AddEntity(ballCollisionSound);
    AddEntity(ballScoreSound);
    // NEW
    AddEntity(scoreKeeperEntity);
}
//...    
```

With our new Entity created, we'll give it a ``TextComponent``, which simply takes a string, a font, and the size we want it to be.

```cs
//...
    leftPlayer.AddComponent(new Paddle(Side.Left, PaddleSpeed));
    rightPlayer.AddComponent(new Paddle(Side.Right, PaddleSpeed));
    ball.AddComponent(new Ball(12.5f, BallSpeed, 
        ballCollisionSound.GetComponent<Sound>(), 
        ballScoreSound.GetComponent<Sound>()));

    // NEW
    scoreKeeperEntity.AddComponent(new TextComponent("text", "Content/square.ttf", 50));
//...
```

Notice that for the initial text, we're giving it "text" rather than our scores. Since we need to constantly update our scores as they change throughout gameplay, we're leaving this with some default value for now, as whatever we put there will change in our ``Update`` method regardless.

Recall that the initial positions for all entities is 0, 0. If we were to run the game as it is now, our score would end up displaying in the upper left corner. In pong, we usually want it to be in the upper middle, so we'll go ahead and give it an intial starting position:
```cs
//...
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
    // NEW
    scoreKeeperEntity.Transform.Position = new Vector3(260, 20, 1f);
//...
```

Now with all that out of the way, we can update our ``TextComponent`` every frame with our player scores! Still in ``MainScene.cs``:

```cs
//...
    public override void Update(GameTime gameTime)  
    {  
        base.Update(gameTime);
        // NEW
        scoreKeeperEntity.GetComponent<TextComponent>().Text = 
            $"{leftPlayer.GetComponent<Paddle>().Score} | {rightPlayer.GetComponent<Paddle>().Score}";
    }
}
```

As ``Update`` is called every frame, this will constantly update our scorekeeper's ``TextComponent`` with the ``Score`` variables that we added to our ``Paddle`` components a while ago.

With that, the tutorial's basically finished. Go ahead and give it a spin! Thank you for following along, and congratulations on creating Pong!
