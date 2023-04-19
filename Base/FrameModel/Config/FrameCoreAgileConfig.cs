namespace FrameModel;

/// <summary>
/// FrameCore组模型
/// </summary>
public class FrameCoreAgileConfig
{
    /// <summary>
    /// 基础配置
    /// </summary>
    public BaseConfig baseConfig { get; set; }

    /// <summary>
    /// 连接字符串配置
    /// </summary>
    public ConnectionConfig connectionConfig { get; set; }

    /// <summary>
    /// 外部地址配置
    /// </summary>
    public ExternalUrl externalUrl { get; set; }

    /// <summary>
    /// 微信公众号配置
    /// </summary>
    public WeiXinMPConfig weiXinMPConfig { get; set; }

    /// <summary>
    /// OpenAI配置
    /// </summary>
    public OpenAIConfig openAIConfig { get; set; }
}

/// <summary>
/// 基础配置
/// </summary>
public class BaseConfig
{

}

/// <summary>
/// 连接字符串配置
/// </summary>
public class ConnectionConfig
{
    /// <summary>
    /// Redis连接字符串
    /// </summary>
    public string RedisCon { get; set; }

    /// <summary>
    /// mongoDB连接字符串
    /// </summary>
    public string MongoDBCon { get; set; }

    /// <summary>
    /// ftp连接字符串
    /// </summary>
    public string FtpCon { get; set; }

    /// <summary>
    /// SqlServer Frame库连接字符串
    /// </summary>
    public string frameCon { get; set; }
}

/// <summary>
/// 外部地址配置
/// </summary>
public class ExternalUrl
{
    /// <summary>
    /// 高德接口的Key
    /// </summary>
    public string GdKey { get; set; }
    /// <summary>
    /// 高德接口 -- 经纬度转地址
    /// </summary>
    public string GdAddressByLocation { get; set; }

    /// <summary>
    /// 一言接口 -- 一句话
    /// </summary>
    public string OneWord { get; set; }

    /// <summary>
    /// 天气接口  --  一周的天气
    /// </summary>
    public string WeekWeather { get; set; }

    /// <summary>
    /// 一句诗
    /// </summary>
    public string OneVerse { get; set; }
}

/// <summary>
/// 微信公众号配置
/// </summary>
public class WeiXinMPConfig
{
    /// <summary>
    /// AppId
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// AppSecret
    /// </summary>
    public string AppSecret { get; set; }

    /// <summary>
    /// 定时通知模板Key
    /// </summary>
    public string TimedNotifKey { get; set; }

    /// <summary>
    /// 获取AccessToken的url
    /// </summary>
    public string AccessTokenUrl { get; set; }

    /// <summary>
    /// 推送消息的url
    /// </summary>
    public string SendTemplateUrl { get; set; }

    /// <summary>
    /// 获取Ticke的url
    /// </summary>
    public string CreateTicketUrl { get; set; }

    /// <summary>
    /// 获取用户信息的url
    /// </summary>
    public string UserInfoUrl { get; set; }

    /// <summary>
    /// 获取场景二维码的url
    /// </summary>
    public string SceneCodeUrl { get; set; }
}

/// <summary>
/// OpenAI配置
/// </summary>
public class OpenAIConfig
{
    /// <summary>
    /// OpenAiKey
    /// </summary>
    public string OpenAiKey { get; set; }

    /// <summary>
    /// OpenAI对话Url
    /// </summary>
    public string OpenAiCompletionsUrl { get; set; }

    /// <summary>
    ///  OpenAI对话Url-GPT3.5
    /// </summary>
    public string CompletionsGPT3_5Url { get; set; }

    /// <summary>
    /// OpenAI图片Url
    /// </summary>
    public string OpenAiImageUrl { get; set; }
}

