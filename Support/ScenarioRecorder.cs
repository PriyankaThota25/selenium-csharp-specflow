using System.Diagnostics;
using OpenQA.Selenium;
using ShopAutomation.Config;
using TechTalk.SpecFlow;

namespace ShopAutomation.Support;

/// <summary>Records steps, timings and screenshots for one scenario (extracted from Hooks - single responsibility).</summary>
public class ScenarioRecorder
{
    private readonly ScenarioContext _scenario;
    private readonly FeatureContext _feature;
    private readonly IWebDriver _driver;
    private readonly ITestSettings _settings;
    private readonly IScreenshotService _screenshots;
    private readonly IReportWriter _report;

    private ScenarioResult _result;
    private string _folderAbs;
    private int _stepNo;
    private Stopwatch _stepClock;
    private Stopwatch _scenarioClock;
    private bool _errorRecorded;

    public ScenarioRecorder(ScenarioContext scenario, FeatureContext feature, IWebDriver driver,
        ITestSettings settings, IScreenshotService screenshots, IReportWriter report)
    {
        _scenario = scenario;
        _feature = feature;
        _driver = driver;
        _settings = settings;
        _screenshots = screenshots;
        _report = report;
    }

    public void Start()
    {
        // Screenshots/<scenario>_<timestamp>/ - one folder per scenario run.
        var started = DateTime.Now;
        // Short unique suffix: outline examples share a title and parallel runs can start in the same second.
        var unique = Guid.NewGuid().ToString("N")[..8];
        var folderName = $"{PathHelper.Sanitize(_scenario.ScenarioInfo.Title)}_{started:yyyyMMdd_HHmmss}_{unique}";
        var shotRoot = Path.IsPathRooted(_settings.ScreenshotDir)
            ? _settings.ScreenshotDir : Path.Combine(PathHelper.RootDir, _settings.ScreenshotDir);
        _folderAbs = Path.Combine(shotRoot, folderName);
        Directory.CreateDirectory(_folderAbs);

        _result = new ScenarioResult
        {
            Feature = _feature.FeatureInfo.Title,
            Name = _scenario.ScenarioInfo.Title,
            Tags = _scenario.ScenarioInfo.Tags,
            Started = started,
            Folder = $"{_settings.ScreenshotDir}/{folderName}".Replace('\\', '/'),
        };
        _scenarioClock = Stopwatch.StartNew();
    }

    public void BeforeStep() => _stepClock = Stopwatch.StartNew();

    public void AfterStep()
    {
        _stepNo++;
        var info = _scenario.StepContext.StepInfo;
        var failed = _scenario.TestError != null && !_errorRecorded;
        if (failed) _errorRecorded = true;

        // Screenshot name: <step#>_<scenario>_<timestamp>.png so files sort in execution order.
        string rel = null;
        var file = $"{_stepNo:00}_{PathHelper.Sanitize(_scenario.ScenarioInfo.Title)}_{DateTime.Now:yyyyMMdd_HHmmss_fff}{(failed ? "_FAILED" : "")}.png";
        if (_screenshots.TryCapture(_driver, Path.Combine(_folderAbs, file)))
            rel = $"{_result.Folder}/{file}";

        _result.Steps.Add(new StepResult
        {
            Keyword = info.StepDefinitionType.ToString(),
            Text = info.Text,
            Passed = !failed,
            Seconds = _stepClock.Elapsed.TotalSeconds,
            Screenshot = rel,
            Error = failed ? _scenario.TestError.ToString() : null,
        });
    }

    public void Finish()
    {
        _result.Seconds = _scenarioClock.Elapsed.TotalSeconds;
        _result.Passed = _scenario.TestError == null;
        _result.Error = _scenario.TestError?.Message;
        _report.Add(_result);
    }
}
