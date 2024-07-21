using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Bombarder.UI;

public class TextElement
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

        switch (Character)
        {
            case 'A':
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                break;
            case 'B':
                letter.Add(new List<bool> { true, true, false });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, false });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, false });
                break;
            case 'C':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, true, true });
                break;
            case 'D':
                letter.Add(new List<bool> { true, true, false });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, false });
                break;
            case 'E':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, true, false });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, true, true });
                break;
            case 'F':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, true, false });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, false, false });
                break;
            case 'G':
                letter.Add(new List<bool> { true, true, true, false });
                letter.Add(new List<bool> { true, false, false, false });
                letter.Add(new List<bool> { true, false, true, true });
                letter.Add(new List<bool> { true, false, false, true });
                letter.Add(new List<bool> { true, true, true, true });
                break;
            case 'H':
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                break;
            case 'I':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { true, true, true });
                break;
            case 'J':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { false, false, true });
                letter.Add(new List<bool> { false, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { false, true, false });
                break;
            case 'K':
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, false });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                break;
            case 'L':
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, true, true });
                break;
            case 'M':
                letter.Add(new List<bool> { true, true, false, true, true });
                letter.Add(new List<bool> { true, false, true, false, true });
                letter.Add(new List<bool> { true, false, true, false, true });
                letter.Add(new List<bool> { true, false, true, false, true });
                letter.Add(new List<bool> { true, false, true, false, true });
                break;
            case 'N':
                letter.Add(new List<bool> { true, false, false, true });
                letter.Add(new List<bool> { true, true, false, true });
                letter.Add(new List<bool> { true, true, true, true });
                letter.Add(new List<bool> { true, false, true, true });
                letter.Add(new List<bool> { true, false, false, true });
                break;
            case 'O':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, true });
                break;
            case 'P':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, false, false });
                break;
            case 'Q':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, false });
                letter.Add(new List<bool> { false, true, true });
                break;
            case 'R':
                letter.Add(new List<bool> { true, true, false });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, false });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                break;
            case 'S':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { false, false, true });
                letter.Add(new List<bool> { true, true, true });
                break;
            case 'T':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, true, false });
                break;
            case 'U':
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, true });
                break;
            case 'V':
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, true, false });
                break;
            case 'W':
                letter.Add(new List<bool> { true, false, false, false, true });
                letter.Add(new List<bool> { true, false, true, false, true });
                letter.Add(new List<bool> { true, false, true, false, true });
                letter.Add(new List<bool> { true, false, true, false, true });
                letter.Add(new List<bool> { true, true, false, true, true });
                break;
            case 'X':
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                break;
            case 'Y':
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, true, false });
                break;
            case 'Z':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { false, false, true });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, true, true });
                break;
            case '1':
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { true, true, false });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { true, true, true });
                break;
            case '2':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { false, false, true });
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, true, true });
                break;
            case '3':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { false, false, true });
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { false, false, true });
                letter.Add(new List<bool> { true, true, true });
                break;
            case '4':
                letter.Add(new List<bool> { true, false, false, false });
                letter.Add(new List<bool> { true, false, false, false });
                letter.Add(new List<bool> { true, false, true, false });
                letter.Add(new List<bool> { true, true, true, true });
                letter.Add(new List<bool> { false, false, true, false });
                break;
            case '5':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { false, false, true });
                letter.Add(new List<bool> { true, true, true });
                break;
            case '6':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, false });
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, true });
                break;
            case '7':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { false, false, true });
                letter.Add(new List<bool> { false, false, true });
                letter.Add(new List<bool> { false, false, true });
                letter.Add(new List<bool> { false, false, true });
                break;
            case '8':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, true });
                break;
            case '9':
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, true, true });
                letter.Add(new List<bool> { false, false, true });
                letter.Add(new List<bool> { false, false, true });
                break;
            case '0':
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { true, false, true });
                letter.Add(new List<bool> { false, true, false });
                break;
            case ':':
                letter.Add(new List<bool> { false, false, false });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, false, false });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, false, false });
                break;
            case ' ':
                letter.Add(new List<bool> { false });
                letter.Add(new List<bool> { false });
                letter.Add(new List<bool> { false });
                letter.Add(new List<bool> { false });
                letter.Add(new List<bool> { false });
                break;
            case '!':
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, true, false });
                letter.Add(new List<bool> { false, false, false });
                letter.Add(new List<bool> { false, true, false });
                break;
            case '.':
                letter.Add(new List<bool> { false });
                letter.Add(new List<bool> { false });
                letter.Add(new List<bool> { false });
                letter.Add(new List<bool> { false });
                letter.Add(new List<bool> { true });
                break;
        }

        return letter;
    }

    public static List<List<bool>> GetString(string Text)
    {
        List<List<bool>> Elements = GetLetter('A').Select(_ => new List<bool>()).ToList();

        foreach (char c in Text)
        {
            List<List<bool>> Element = GetLetter(c);

            for (int i = 0; i < Element.Count; i++)
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
