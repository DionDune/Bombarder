using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Bombarder;

public class KeyboardInput
{
    public HashSet<Keys> PreviousKeys { get; set; } = new();
    public HashSet<Keys> CurrentKeys { get; set; } = new();
    private readonly Dictionary<Keys, Dictionary<string, Action>> _keyPressActions = new();
    private readonly Dictionary<Keys, Dictionary<string, Action>> _keyReleaseActions = new();
    
    public void Update()
    {
        PreviousKeys = new HashSet<Keys>(CurrentKeys);
        CurrentKeys = new HashSet<Keys>(Keyboard.GetState().GetPressedKeys());
        
        ExecuteKeyPressActions();
        ExecuteKeyReleaseActions();
    }
    
    public void AddKeyPressAction(Keys key, Action action, string name)
    {
        if (!_keyPressActions.ContainsKey(key))
        {
            _keyPressActions[key] = new Dictionary<string, Action>();
        }

        _keyPressActions[key][name] = action;
    }
    
    public void AddKeyReleaseAction(Keys key, Action action, string name)
    {
        if (!_keyReleaseActions.ContainsKey(key))
        {
            _keyReleaseActions[key] = new Dictionary<string, Action>();
        }

        _keyReleaseActions[key][name] = action;
    }
    
    public void ExecuteKeyPressActions() =>
        CurrentKeys
            .Where(HasJustPressed)
            .Where(_keyPressActions.ContainsKey) 
            .SelectMany(Key => _keyPressActions[Key].Values)
            .ToList()
            .ForEach(Action => Action.Invoke());
    
    public void ExecuteKeyReleaseActions() =>
        PreviousKeys
            .Where(HasJustReleased)
            .Where(_keyReleaseActions.ContainsKey) 
            .SelectMany(Key => _keyReleaseActions[Key].Values)
            .ToList()
            .ForEach(Action => Action.Invoke());
    
    public void RemoveKeyPressAction(Keys Key, string Name)
    {
        if (_keyPressActions.TryGetValue(Key, out var Action))
        {
            Action.Remove(Name);
        }
    }
    
    public void RemoveKeyReleaseAction(Keys Key, string Name)
    {
        if (_keyReleaseActions.TryGetValue(Key, out var Action))
        {
            Action.Remove(Name);
        }
    }

    public bool IsKeyDown(Keys Key) => CurrentKeys.Contains(Key);
    public bool IsKeyUp(Keys Key) => !CurrentKeys.Contains(Key);
    public bool IsHoldingKey(Keys Key) => IsKeyDown(Key) && PreviousKeys.Contains(Key);
    public bool HasJustPressed(Keys Key) => IsKeyDown(Key) && !PreviousKeys.Contains(Key);
    public bool HasJustReleased(Keys Key) => IsKeyUp(Key) && PreviousKeys.Contains(Key);
}