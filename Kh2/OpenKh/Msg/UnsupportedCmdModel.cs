namespace KhLib.Kh2.OpenKh.Msg
{
    internal class UnsupportedCmdModel : BaseCmdModel
    {
        public UnsupportedCmdModel(byte data)
        {
            Command = MessageCommand.Unsupported;
            RawData = data;
        }
    }
}
