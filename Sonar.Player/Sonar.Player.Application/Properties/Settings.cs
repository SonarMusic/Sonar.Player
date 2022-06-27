using Sonar.Player.Application.Tools;

namespace Sonar.Player.Application.Properties;

public class Settings
{
    private static Settings? _instance = null;
    public static int Bitrate = (int) BitrateEnum.MediumBitrate;
    
    private Settings() { }
    
    public static Settings GetInstance()
    {
        return _instance ??= new Settings();
    }
}