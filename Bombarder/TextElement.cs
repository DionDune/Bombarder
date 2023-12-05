using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Bombarder
{
    internal class TextElement
    {
        public int XOffset { get; set; }
        public int YOffset { get; set; }

        public string Text { get; set; }

        public List<List<bool>> Elements { get; set; }
        public int ElementSize { get; set; }

        public Color Color { get; set; }
        public Color BackgroundColor { get; set; }

        public bool hasBackground { get; set; }

        public TextElement()
        {
            XOffset = 0;
            YOffset = 0;

            Text = "EXAMPLE";
            Elements = GetString(Text);
            ElementSize = 5;

            Color = Color.Black;
            BackgroundColor = Color.White;

            hasBackground = false;
        }



        public static List<List<bool>> GetLetter(char Character)
        {
            List<List<bool>> letter = new List<List<bool>>();

            if (Character == 'A')
            {
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
            }
            if (Character == 'B')
            {
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, false });
            }
            if (Character == 'C')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'D')
            {
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, false });
            }
            if (Character == 'E')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'F')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
            }
            if (Character == 'G')
            {
                letter.Add(new List<bool>() { true, true, true, false });
                letter.Add(new List<bool>() { true, false, false, false });
                letter.Add(new List<bool>() { true, false, true, true });
                letter.Add(new List<bool>() { true, false, false, true });
                letter.Add(new List<bool>() { true, true, true, true });
            }
            if (Character == 'H')
            {
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
            }
            if (Character == 'I')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'J')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { false, true, false });
            }
            if (Character == 'K')
            {
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
            }
            if (Character == 'L')
            {
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'M')
            {
                letter.Add(new List<bool>() { true, true, false, true, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
            }
            if (Character == 'N')
            {
                letter.Add(new List<bool>() { true, false, false, true });
                letter.Add(new List<bool>() { true, true, false, true });
                letter.Add(new List<bool>() { true, true, true, true });
                letter.Add(new List<bool>() { true, false, true, true });
                letter.Add(new List<bool>() { true, false, false, true });
            }
            if (Character == 'O')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'P')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
            }
            if (Character == 'Q')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { false, true, true });
            }
            if (Character == 'R')
            {
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
            }
            if (Character == 'S')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'T')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
            }
            if (Character == 'U')
            {
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'V')
            {
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
            }
            if (Character == 'W')
            {
                letter.Add(new List<bool>() { true, false, false, false, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
                letter.Add(new List<bool>() { true, true, false, true, true });
            }
            if (Character == 'X')
            {
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
            }
            if (Character == 'Y')
            {
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
            }
            if (Character == 'Z')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '1')
            {
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '2')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '3')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '4')
            {
                letter.Add(new List<bool>() { true, false, false, false });
                letter.Add(new List<bool>() { true, false, false, false });
                letter.Add(new List<bool>() { true, false, true, false });
                letter.Add(new List<bool>() { true, true, true, true });
                letter.Add(new List<bool>() { false, false, true, false });
            }
            if (Character == '5')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '6')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '7')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { false, false, true });
            }
            if (Character == '8')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '9')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { false, false, true });
            }
            if (Character == '0')
            {
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { false, true, false });
            }
            if (Character == ':')
            {
                letter.Add(new List<bool>() { false, false, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, false, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, false, false });
            }
            if (Character == ' ')
            {
                letter.Add(new List<bool>() { false });
                letter.Add(new List<bool>() { false });
                letter.Add(new List<bool>() { false });
                letter.Add(new List<bool>() { false });
                letter.Add(new List<bool>() { false });
            }

            return letter;
        }

        public static List<List<bool>> GetString(string Text)
        {
            List<List<bool>> Elements = new List<List<bool>>();
            foreach (List<bool> Lst in GetLetter('A'))
            {
                Elements.Add(new List<bool>());
            }

            foreach (char c in Text)
            {
                List<List<bool>> Element = GetLetter(c);

                for (int i = 0; i < Element.Count(); i++)
                {
                    foreach (bool b in Element[i])
                    {
                        Elements[i].Add(b);
                    }

                    if (Text.IndexOf(c) != Text.Length - 1)
                    {
                        Elements[i].Add(false);
                    }
                }

            }

            return Elements;
        }
    }
}
