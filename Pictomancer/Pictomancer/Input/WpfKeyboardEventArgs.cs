using System;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;

namespace Pictomancer.Input
{
    public class WpfKeyboardEventArgs : EventArgs
    {
        public WpfKeyboardEventArgs(Keys key, KeyboardState keyboardState)
        {
            this.Key = key;
            this.Modifiers = KeyboardModifiers.None;
            if (keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl))
                this.Modifiers = this.Modifiers | KeyboardModifiers.Control;
            if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                this.Modifiers = this.Modifiers | KeyboardModifiers.Shift;
            if (!keyboardState.IsKeyDown(Keys.LeftAlt) && !keyboardState.IsKeyDown(Keys.RightAlt))
                return;
            this.Modifiers = this.Modifiers | KeyboardModifiers.Alt;
        }

        public Keys Key { get; }

        public KeyboardModifiers Modifiers { get; }

        public char? Character => WpfKeyboardEventArgs.ToChar(this.Key, this.Modifiers);

        private static char? ToChar(Keys key, KeyboardModifiers modifiers = KeyboardModifiers.None)
        {
            bool flag = (modifiers & KeyboardModifiers.Shift) == KeyboardModifiers.Shift;
            switch (key)
            {
                case Keys.A:
                    return new char?(flag ? 'A' : 'a');
                case Keys.B:
                    return new char?(flag ? 'B' : 'b');
                case Keys.C:
                    return new char?(flag ? 'C' : 'c');
                case Keys.D:
                    return new char?(flag ? 'D' : 'd');
                case Keys.E:
                    return new char?(flag ? 'E' : 'e');
                case Keys.F:
                    return new char?(flag ? 'F' : 'f');
                case Keys.G:
                    return new char?(flag ? 'G' : 'g');
                case Keys.H:
                    return new char?(flag ? 'H' : 'h');
                case Keys.I:
                    return new char?(flag ? 'I' : 'i');
                case Keys.J:
                    return new char?(flag ? 'J' : 'j');
                case Keys.K:
                    return new char?(flag ? 'K' : 'k');
                case Keys.L:
                    return new char?(flag ? 'L' : 'l');
                case Keys.M:
                    return new char?(flag ? 'M' : 'm');
                case Keys.N:
                    return new char?(flag ? 'N' : 'n');
                case Keys.O:
                    return new char?(flag ? 'O' : 'o');
                case Keys.P:
                    return new char?(flag ? 'P' : 'p');
                case Keys.Q:
                    return new char?(flag ? 'Q' : 'q');
                case Keys.R:
                    return new char?(flag ? 'R' : 'r');
                case Keys.S:
                    return new char?(flag ? 'S' : 's');
                case Keys.T:
                    return new char?(flag ? 'T' : 't');
                case Keys.U:
                    return new char?(flag ? 'U' : 'u');
                case Keys.V:
                    return new char?(flag ? 'V' : 'v');
                case Keys.W:
                    return new char?(flag ? 'W' : 'w');
                case Keys.X:
                    return new char?(flag ? 'X' : 'x');
                case Keys.Y:
                    return new char?(flag ? 'Y' : 'y');
                case Keys.Z:
                    return new char?(flag ? 'Z' : 'z');
                default:
                    if (key == Keys.D0 && !flag || key == Keys.NumPad0)
                        return new char?('0');
                    if (key == Keys.D1 && !flag || key == Keys.NumPad1)
                        return new char?('1');
                    if (key == Keys.D2 && !flag || key == Keys.NumPad2)
                        return new char?('2');
                    if (key == Keys.D3 && !flag || key == Keys.NumPad3)
                        return new char?('3');
                    if (key == Keys.D4 && !flag || key == Keys.NumPad4)
                        return new char?('4');
                    if (key == Keys.D5 && !flag || key == Keys.NumPad5)
                        return new char?('5');
                    if (key == Keys.D6 && !flag || key == Keys.NumPad6)
                        return new char?('6');
                    if (key == Keys.D7 && !flag || key == Keys.NumPad7)
                        return new char?('7');
                    if (key == Keys.D8 && !flag || key == Keys.NumPad8)
                        return new char?('8');
                    if (key == Keys.D9 && !flag || key == Keys.NumPad9)
                        return new char?('9');
                    if (key == Keys.D0 & flag)
                        return new char?(')');
                    if (key == Keys.D1 & flag)
                        return new char?('!');
                    if (key == Keys.D2 & flag)
                        return new char?('@');
                    if (key == Keys.D3 & flag)
                        return new char?('#');
                    if (key == Keys.D4 & flag)
                        return new char?('$');
                    if (key == Keys.D5 & flag)
                        return new char?('%');
                    if (key == Keys.D6 & flag)
                        return new char?('^');
                    if (key == Keys.D7 & flag)
                        return new char?('&');
                    if (key == Keys.D8 & flag)
                        return new char?('*');
                    if (key == Keys.D9 & flag)
                        return new char?('(');
                    switch (key)
                    {
                        case Keys.Back:
                            return new char?('\b');
                        case Keys.Tab:
                            return new char?('\t');
                        case Keys.Enter:
                            return new char?('\r');
                        case Keys.Space:
                            return new char?(' ');
                        case Keys.Multiply:
                            return new char?('*');
                        case Keys.Add:
                            return new char?('+');
                        case Keys.Decimal:
                            return new char?('.');
                        case Keys.Divide:
                            return new char?('/');
                        case Keys.OemComma:
                            if (!flag)
                                return new char?(',');
                            break;
                        case Keys.OemBackslash:
                            return new char?('\\');
                    }
                    if (key == Keys.OemComma & flag)
                        return new char?('<');
                    if (key == Keys.OemOpenBrackets && !flag)
                        return new char?('[');
                    if (key == Keys.OemOpenBrackets & flag)
                        return new char?('{');
                    if (key == Keys.OemCloseBrackets && !flag)
                        return new char?(']');
                    if (key == Keys.OemCloseBrackets & flag)
                        return new char?('}');
                    if (key == Keys.OemPeriod && !flag)
                        return new char?('.');
                    if (key == Keys.OemPeriod & flag)
                        return new char?('>');
                    if (key == Keys.OemPipe && !flag)
                        return new char?('\\');
                    if (key == Keys.OemPipe & flag)
                        return new char?('|');
                    if (key == Keys.OemPlus && !flag)
                        return new char?('=');
                    if (key == Keys.OemPlus & flag)
                        return new char?('+');
                    if (key == Keys.OemMinus && !flag)
                        return new char?('-');
                    if (key == Keys.OemMinus & flag)
                        return new char?('_');
                    if (key == Keys.OemQuestion && !flag)
                        return new char?('/');
                    if (key == Keys.OemQuestion & flag)
                        return new char?('?');
                    if (key == Keys.OemQuotes && !flag)
                        return new char?('\'');
                    if (key == Keys.OemQuotes & flag)
                        return new char?('"');
                    if (key == Keys.OemSemicolon && !flag)
                        return new char?(';');
                    if (key == Keys.OemSemicolon & flag)
                        return new char?(':');
                    if (key == Keys.OemTilde && !flag)
                        return new char?('`');
                    if (key == Keys.OemTilde & flag)
                        return new char?('~');
                    return key == Keys.Subtract ? new char?('-') : new char?();
            }
        }
    }
}