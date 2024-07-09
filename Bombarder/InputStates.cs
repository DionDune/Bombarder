using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Bombarder;

internal class InputStates
{
    public List<Keys> PreviousKeys { get; set; } = new();
    public bool IsClickingLeft { get; set; }
    public bool IsClickingRight { get; set; }
    public bool IsClickingMiddle { get; set; }
}