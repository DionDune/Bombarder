using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Bombarder
{
    internal class InputStates
    {
        public List<Keys> PreviouseKeys { get; set; }
        public bool isClickingLeft { get; set; }
        public bool isClickingRight { get; set; }
        public bool isClickingMiddle { get; set; }

        public InputStates()
        {
            PreviouseKeys = new List<Keys>();

            isClickingLeft = false;
            isClickingRight = false;
            isClickingMiddle = false;
        }
    }
}
