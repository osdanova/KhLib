namespace KhLib.Kh2.OpenKh.Msg
{
    internal class DataCmdModel : BaseCmdModel
    {
        public DataCmdModel(MessageCommand command, int lenght)
        {
            Command = command;
            Length = lenght;
        }
    }
}
