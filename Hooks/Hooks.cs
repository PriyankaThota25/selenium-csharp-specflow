using BoDi;
using OpenQA.Selenium;
using ShopAutomation.Config;
using ShopAutomation.Drivers;
using ShopAutomation.Support;
using TechTalk.SpecFlow;

namespace ShopAutomation.Hooks;

/// <summary>
/// Composition root. Registers abstractions in the SpecFlow (BoDi) container; page objects and step classes
/// are concrete types whose constructor dependencies BoDi resolves automatically.
/// </summary>
[Binding]
public class Hooks
{
    private readonly IObjectContainer _container;
    private ScenarioRecorder _recorder;

    public Hooks(IObjectContainer container) => _container = container;

    [AfterTestRun]
    public static void WriteReport() => ReportWriter.Instance.Write();

    [BeforeScenario]
    public void Setup()
    {
        // Singletons shared by every scenario.
        ITestSettings settings = SettingsProvider.Instance;
        _container.RegisterInstanceAs(settings);
        _container.RegisterInstanceAs<IReportWriter>(ReportWriter.Instance);

        // Per-scenario services.
        _container.RegisterInstanceAs<IDriverFactory>(DriverFactory.CreateDefault(settings));
        _container.RegisterTypeAs<ScreenshotService, IScreenshotService>();
        _container.RegisterInstanceAs(_container.Resolve<IDriverFactory>().Create());

        _recorder = _container.Resolve<ScenarioRecorder>();
        _recorder.Start();
    }

    [BeforeStep]
    public void BeforeStep() => _recorder?.BeforeStep();

    [AfterStep]
    public void AfterStep() => _recorder?.AfterStep();

    [AfterScenario]
    public void TearDown()
    {
        // Setup may have failed before the recorder/driver existed; don't let cleanup mask the real error.
        try { _recorder?.Finish(); }
        finally
        {
            if (_container.IsRegistered<IWebDriver>()) _container.Resolve<IWebDriver>().Quit();
        }
    }
}
