using System.Collections.Concurrent;
using System.Net;
using System.Text;

namespace ShopAutomation.Support;

public class StepResult
{
    public string Keyword { get; set; }
    public string Text { get; set; }
    public bool Passed { get; set; }
    public double Seconds { get; set; }
    public string Screenshot { get; set; }   // path relative to report.html
    public string Error { get; set; }
}

public class ScenarioResult
{
    public string Feature { get; set; }
    public string Name { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public DateTime Started { get; set; }
    public double Seconds { get; set; }
    public string Folder { get; set; }       // relative to report.html
    public bool Passed { get; set; }
    public string Error { get; set; }
    public List<StepResult> Steps { get; } = new();
}

/// <summary>
/// Singleton: collects scenario results during the run and writes a single self-contained report.html at the end.
/// One shared instance is required so results from every scenario land in the same report.
/// </summary>
public sealed class ReportWriter : IReportWriter
{
    private static readonly Lazy<ReportWriter> Lazy = new(() => new ReportWriter());
    public static ReportWriter Instance => Lazy.Value;

    private readonly ConcurrentBag<ScenarioResult> Results = new();
    private readonly DateTime _runStart = DateTime.Now;

    private ReportWriter() { }

    public void Add(ScenarioResult r) => Results.Add(r);

    public void Write()
    {
        var list = Results.OrderBy(r => r.Started).ToList();
        int pass = list.Count(r => r.Passed), fail = list.Count - pass;
        var h = (Func<string, string>)(s => WebUtility.HtmlEncode(s ?? ""));
        var sb = new StringBuilder();
        sb.Append($@"<!doctype html><html lang=""en""><head><meta charset=""utf-8""><title>Test Report</title>
<style>
body{{font-family:-apple-system,Segoe UI,Arial,sans-serif;margin:0;background:#f4f6f8;color:#222}}
header{{background:#1f2937;color:#fff;padding:20px 32px}}
header h1{{margin:0 0 6px;font-size:22px}} header small{{color:#cbd5e1}}
.summary{{display:flex;gap:16px;padding:20px 32px}}
.card{{background:#fff;border-radius:8px;padding:14px 22px;box-shadow:0 1px 3px #0002;min-width:100px}}
.card b{{display:block;font-size:26px}} .pass{{color:#15803d}} .fail{{color:#b91c1c}}
.sc{{background:#fff;margin:0 32px 16px;border-radius:8px;box-shadow:0 1px 3px #0002;overflow:hidden}}
.sc>summary{{cursor:pointer;padding:14px 18px;font-weight:600;display:flex;gap:12px;align-items:center}}
.badge{{padding:2px 10px;border-radius:12px;color:#fff;font-size:12px}} .b-pass{{background:#15803d}} .b-fail{{background:#b91c1c}}
.meta{{font-weight:400;color:#64748b;font-size:13px;margin-left:auto}}
.step{{border-top:1px solid #e5e7eb;padding:10px 18px}}
.step .kw{{font-weight:700;color:#475569}} .step .t{{float:right;color:#94a3b8;font-size:12px}}
.step img{{display:block;margin-top:8px;max-width:520px;border:1px solid #cbd5e1;border-radius:4px}}
pre{{background:#fef2f2;color:#7f1d1d;padding:10px;border-radius:4px;white-space:pre-wrap}}
.tag{{background:#e0e7ff;color:#3730a3;border-radius:4px;padding:1px 6px;font-size:12px;font-weight:400}}
</style></head><body>
<header><h1>Test Execution Report</h1><small>Run started {_runStart:yyyy-MM-dd HH:mm:ss} &middot; generated {DateTime.Now:yyyy-MM-dd HH:mm:ss}</small></header>
<div class=""summary"">
<div class=""card""><b>{list.Count}</b>Scenarios</div>
<div class=""card""><b class=""pass"">{pass}</b>Passed</div>
<div class=""card""><b class=""fail"">{fail}</b>Failed</div></div>");

        foreach (var s in list)
        {
            sb.Append($@"<details class=""sc"" {(s.Passed ? "" : "open")}><summary>
<span class=""badge {(s.Passed ? "b-pass" : "b-fail")}"">{(s.Passed ? "PASSED" : "FAILED")}</span>
<span>{h(s.Feature)} &rsaquo; {h(s.Name)}</span>{string.Concat(s.Tags.Select(t => $" <span class=\"tag\">@{h(t)}</span>"))}
<span class=""meta"">{s.Started:HH:mm:ss} &middot; {s.Seconds:0.0}s &middot; {h(s.Folder)}</span></summary>");
            foreach (var st in s.Steps)
            {
                sb.Append($@"<div class=""step""><span class=""t"">{st.Seconds:0.00}s</span>
<span class=""badge {(st.Passed ? "b-pass" : "b-fail")}"">{(st.Passed ? "pass" : "fail")}</span>
<span class=""kw"">{h(st.Keyword)}</span> {h(st.Text)}");
                if (!st.Passed && st.Error != null) sb.Append($"<pre>{h(st.Error)}</pre>");
                if (st.Screenshot != null)
                    sb.Append($@"<a href=""{h(st.Screenshot)}"" target=""_blank""><img loading=""lazy"" src=""{h(st.Screenshot)}"" alt=""screenshot""></a>");
                sb.Append("</div>");
            }
            sb.Append("</details>");
        }
        sb.Append("</body></html>");
        File.WriteAllText(Path.Combine(PathHelper.RootDir, "report.html"), sb.ToString());
    }
}
