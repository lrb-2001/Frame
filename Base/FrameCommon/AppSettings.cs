using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FrameCommon;


public class AppSettings
{
    static IConfiguration Configuration { get; set; }
    static string contentPath { get; set; }

    public AppSettings(string contentPath)
    {
        string Path = "appsettings.json";

        //如果你把配置文件 是 根据环境变量来分开了，可以这样写
        //Path = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json";

        Configuration = new ConfigurationBuilder()
           .SetBasePath(contentPath)
           .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
           .Build();
    }

    public AppSettings(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// 封装要操作的字符
    /// </summary>
    /// <param name="sections">节点配置</param>
    /// <returns></returns>
    public static string app(params string[] sections)
    {
        try
        {
            if (sections.Any())
            {
                return Configuration[string.Join(":", sections)];
            }
        }
        catch (Exception) { }

        return "";
    }

    /// <summary>
    /// 递归获取配置信息数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sections"></param>
    /// <returns></returns>
    public static List<T> app<T>(params string[] sections)
    {
        List<T> list = new List<T>();
        Configuration.Bind(string.Join(":", sections), list);
        return list;
    }

    /// <summary>
    /// 写appsettings.json里的字段
    /// </summary>
    public static bool write(string name, string key)
    {
        try
        {
            Configuration[name] = key;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// 读appsettings.json里的字段
    /// </summary>
    public static string read(string name)
    {
        try
        {
            return Configuration[name];
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public AppSettings(IConfigurationBuilder configurationBuilder, IHostEnvironment hostEnvironment)
    {
        // 获取根配置
        var configuration = configurationBuilder is ConfigurationManager
            ? (configurationBuilder as ConfigurationManager)
            : configurationBuilder.Build();

        // 获取程序执行目录
        var executeDirectory = AppContext.BaseDirectory;

        // 获取自定义配置扫描目录
        var configurationScanDirectories = (configuration.GetSection("ConfigurationScanDirectories")
                .Get<string[]>()
            ?? Array.Empty<string>()).Select(u => Path.Combine(executeDirectory, u));

        // 扫描执行目录及自定义配置目录下的 *.json 文件
        var jsonFiles = new[] { executeDirectory }.Concat(configurationScanDirectories)
                           .SelectMany(u =>
                                Directory.GetFiles(u, "*.json", SearchOption.TopDirectoryOnly));

        // 如果没有配置文件，中止执行
        if (!jsonFiles.Any()) return;

        // 获取环境变量名，如果没找到，则读取 NETCORE_ENVIRONMENT 环境变量信息识别（用于非 Web 环境）
        var envName = hostEnvironment?.EnvironmentName ?? Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT") ?? "Unknown";

        // 读取忽略的配置文件
        var ignoreConfigurationFiles = configuration.GetSection("IgnoreConfigurationFiles")
                .Get<string[]>()
            ?? Array.Empty<string>();

        // 处理控制台应用程序
        var _excludeJsonPrefixs = hostEnvironment == default ? excludeJsonPrefixs.Where(u => !u.Equals("appsettings")) : excludeJsonPrefixs;

        // 将所有文件进行分组
        var jsonFilesGroups = SplitConfigFileNameToGroups(jsonFiles)
                                                                .Where(u => !_excludeJsonPrefixs.Contains(u.Key, StringComparer.OrdinalIgnoreCase) && !u.Any(c => runtimeJsonSuffixs.Any(z => c.EndsWith(z, StringComparison.OrdinalIgnoreCase)) || ignoreConfigurationFiles.Contains(Path.GetFileName(c), StringComparer.OrdinalIgnoreCase)));

        // 遍历所有配置分组
        foreach (var group in jsonFilesGroups)
        {
            // 限制查找的 json 文件组
            var limitFileNames = new[] { $"{group.Key}.json", $"{group.Key}.{envName}.json" };

            // 查找默认配置和环境配置
            var files = group.Where(u => limitFileNames.Contains(Path.GetFileName(u), StringComparer.OrdinalIgnoreCase))
                                             .OrderBy(u => Path.GetFileName(u).Length);

            // 循环加载
            foreach (var jsonFile in files)
            {
                configurationBuilder.AddJsonFile(jsonFile, optional: true, reloadOnChange: true);
            }
        }
        Configuration = configurationBuilder.Build();
    }

    /// <summary>
    /// 排除的配置文件前缀
    /// </summary>
    private static readonly string[] excludeJsonPrefixs = new[] { "appsettings", "bundleconfig", "compilerconfig" };

    /// <summary>
    /// 排除运行时 Json 后缀
    /// </summary>
    private static readonly string[] runtimeJsonSuffixs = new[]
    {
            "deps.json",
            "runtimeconfig.dev.json",
            "runtimeconfig.prod.json",
            "runtimeconfig.json",
            "staticwebassets.runtime.json"
        };

    /// <summary>
    /// 对配置文件名进行分组
    /// </summary>
    /// <param name="configFiles"></param>
    /// <returns></returns>
    private static IEnumerable<IGrouping<string, string>> SplitConfigFileNameToGroups(IEnumerable<string> configFiles)
    {
        // 分组
        return configFiles.GroupBy(Function);

        // 本地函数
        static string Function(string file)
        {
            // 根据 . 分隔
            var fileNameParts = Path.GetFileName(file).Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (fileNameParts.Length == 2) return fileNameParts[0];

            return string.Join('.', fileNameParts.Take(fileNameParts.Length - 2));
        }
    }

}
