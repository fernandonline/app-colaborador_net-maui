namespace crud_maui.Utils
{
    public static class ConnectDB
    {
        public static string DevolverRota(string nomeBaseDados)
        {
            string pastaBase;
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                pastaBase = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }
            else
            {
                throw new NotSupportedException("Plataforma não suportada");
            }
            return Path.Combine(pastaBase, nomeBaseDados);
        }
    }
}
