﻿namespace KhLib.Kh2.OpenKh.Msg
{
    internal class TextCmdModel : BaseCmdModel
    {
        public TextCmdModel(byte chData) :
            this((char)chData)
        { }

        public TextCmdModel(char ch) :
            this($"{ch}")
        {
            Command = MessageCommand.PrintText;
            Text = $"{ch}";
        }

        public TextCmdModel(string str)
        {
            Command = MessageCommand.PrintComplex;
            Text = str;
        }
    }
}
