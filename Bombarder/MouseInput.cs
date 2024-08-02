using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bombarder;

public class MouseInput
{
    public HashSet<MouseButtons> PreviousButtons { get; set; } = new();
    public HashSet<MouseButtons> CurrentButtons { get; set; } = new();
    public Vector2 Position => Mouse.GetState().Position.ToVector2();
    
    private readonly Dictionary<MouseButtons, Dictionary<string, Action>> _clickActions = new();
    private readonly Dictionary<MouseButtons, Dictionary<string, Action>> _releaseActions = new();
    
    public void Update()
    {
        PreviousButtons = new HashSet<MouseButtons>(CurrentButtons);
        var MouseState = Mouse.GetState();
        var MouseDict = new Dictionary<MouseButtons, ButtonState>
        {
            {MouseButtons.Left, MouseState.LeftButton},
            {MouseButtons.Middle, MouseState.MiddleButton},
            {MouseButtons.Right, MouseState.RightButton},
            {MouseButtons.XButton1, MouseState.XButton1},
            {MouseButtons.XButton2, MouseState.XButton2}
        };

        CurrentButtons = new HashSet<MouseButtons>(
            MouseDict
                .Where(KeyValue => KeyValue.Value == ButtonState.Pressed)
                .Select(KeyValue => KeyValue.Key)
        );
        
        ExecuteClickActions();
        ExecuteReleaseActions();
   }
    
    public void AddClickAction(MouseButtons Button, Action Action, string Name)
    {
        if (!_clickActions.ContainsKey(Button))
        {
            _clickActions[Button] = new Dictionary<string, Action>();
        }

        _clickActions[Button][Name] = Action;
    }
    
    public void AddReleaseAction(MouseButtons Button, Action Action, string Name)
    {
        if (!_releaseActions.ContainsKey(Button))
        {
            _releaseActions[Button] = new Dictionary<string, Action>();
        }

        _releaseActions[Button][Name] = Action;
    }
    
    public void ExecuteClickActions() =>
        CurrentButtons
            .Where(HasJustPressed)
            .Where(_clickActions.ContainsKey) 
            .SelectMany(Key => _clickActions[Key].Values)
            .ToList()
            .ForEach(Action => Action.Invoke());
    
    public void ExecuteReleaseActions() =>
        PreviousButtons
            .Where(HasJustReleased)
            .Where(_releaseActions.ContainsKey) 
            .SelectMany(Key => _releaseActions[Key].Values)
            .ToList()
            .ForEach(Action => Action.Invoke());

    public void RemoveClickAction(MouseButtons Button, string Name)
    {
        if (_clickActions.TryGetValue(Button, out var Action))
        {
            Action.Remove(Name);
        }
    }
    
    public void RemoveReleaseAction(MouseButtons Button, string Name)
    {
        if (_releaseActions.TryGetValue(Button, out var Action))
        {
            Action.Remove(Name);
        }
    }


    public bool IsKeyDown(MouseButtons Key) => CurrentButtons.Contains(Key);
    public bool IsKeyUp(MouseButtons Key) => !CurrentButtons.Contains(Key);
    public bool IsHolding(MouseButtons Key) => IsKeyDown(Key) && PreviousButtons.Contains(Key);
    public bool HasJustPressed(MouseButtons Key) => IsKeyDown(Key) && !PreviousButtons.Contains(Key);
    public bool HasJustReleased(MouseButtons Key) => IsKeyUp(Key) && PreviousButtons.Contains(Key);
}