using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class Enemies : Node2D
{
    private PackedScene enemy = GD.Load<PackedScene>("res://Scenes/Player/Dummy.tscn");

    Timer _timer;
    Player _player;
    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Timeout += OnTimerTimeout;
        _player = GetNode<Player>("../Player");

    }
    public override void _PhysicsProcess(double delta)
    {

    }

    private double generateRandomNumber(int scalar = 20, bool includeNegative = false) {
        Random randomNumberGenerator = new Random();
        Double randomNum1;
        Double randomNum2;
        
        int direction = 0;

        if (!includeNegative) {
            randomNum1 = randomNumberGenerator.NextDouble() * scalar;
            randomNum2 = randomNumberGenerator.NextDouble() * randomNum1;
            return randomNum2;
        }

        if(randomNumberGenerator.Next(0, 2) == 0) {
            direction = -1;
        } else {
            direction = 1;
        }

        randomNum1 = randomNumberGenerator.NextDouble() * scalar * direction;
        randomNum2 = randomNumberGenerator.NextDouble() * randomNum1 * direction;

        return randomNum1;
       
    }

    private void randomizeSpawnTimes(int scalar = 20) {
        Double randomWaitTime = this.generateRandomNumber();
        _timer.WaitTime = randomWaitTime;
    }

    private Vector2 randomizeSpawnDistances(int scalar = 1000) {
        float xPosition = (float) this.generateRandomNumber(scalar, true) + this._player.GlobalPosition.X;
        float yPosition = (float) this.generateRandomNumber(scalar, true) + this._player.GlobalPosition.Y;
        Vector2 spawnPosition = new Vector2(xPosition, yPosition);

        Console.WriteLine(this._player.GlobalPosition.X);
        Console.WriteLine(spawnPosition);

        return spawnPosition;
    }

    public void OnTimerTimeout() {
        Enemy enemy_instance = enemy.Instantiate<Enemy>();
        enemy_instance.Position = this.randomizeSpawnDistances();
        this.AddChild(enemy_instance);
      
        this.randomizeSpawnTimes();
        GD.Print(_timer.WaitTime);
    }


}
