namespace ShopAutomation.Support;

public interface IReportWriter
{
    void Add(ScenarioResult result);
    void Write();
}
