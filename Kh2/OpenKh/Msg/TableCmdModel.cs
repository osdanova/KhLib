namespace KhLib.Kh2.OpenKh.Msg
{
    internal class TableCmdModel : BaseCmdModel
    {
        private readonly char[] _table;

        public TableCmdModel(MessageCommand messageCommand, char[] table)
        {
            Command = messageCommand;
            _table = table;
        }

        public string GetText(byte data) => $"{_table[data]}";
    }
}
