using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;

namespace Library.Web.Services;

public class SoapClientService
{
    private readonly IHttpClientFactory _http;
    public SoapClientService(IHttpClientFactory http) => _http = http;

    private static StringContent XmlContent(string xml, string? soapAction = null)
    {
        var content = new StringContent(xml, Encoding.UTF8, "text/xml");
        // SOAPAction header helps some stacks; SoapCore accepts both
        if (!string.IsNullOrEmpty(soapAction))
            content.Headers.Add("SOAPAction", soapAction);
        return content;
    }

    public async Task<string> PingAsync()
    {
        var client = _http.CreateClient("Soap");
        var envelope =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
  <s:Body>
    <Ping xmlns=""http://tempuri.org/"" />
  </s:Body>
</s:Envelope>";
        var resp = await client.PostAsync("", XmlContent(envelope, "\"http://tempuri.org/ILibrarySoapService/Ping\""));
        resp.EnsureSuccessStatusCode();
        var xml = XDocument.Parse(await resp.Content.ReadAsStringAsync());
        // .../<PingResponse>/<PingResult>
        return xml.Descendants().First(e => e.Name.LocalName == "PingResult").Value;
    }

    public async Task<bool> IsAvailableAsync(string isbn)
    {
        var client = _http.CreateClient("Soap");
        var envelope =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
  <s:Body>
    <IsAvailable xmlns=""http://tempuri.org/"">
      <isbn>{System.Security.SecurityElement.Escape(isbn)}</isbn>
    </IsAvailable>
  </s:Body>
</s:Envelope>";
        var resp = await client.PostAsync("", XmlContent(envelope, "\"http://tempuri.org/ILibrarySoapService/IsAvailable\""));
        resp.EnsureSuccessStatusCode();
        var xml = XDocument.Parse(await resp.Content.ReadAsStringAsync());
        var val = xml.Descendants().First(e => e.Name.LocalName == "IsAvailableResult").Value;
        return string.Equals(val, "true", StringComparison.OrdinalIgnoreCase);
    }

    public record SoapBookDto(int Id, string Isbn, string Title, string Author, int Year, int TotalCopies, int AvailableCopies);

    public async Task<SoapBookDto?> GetBookByIsbnAsync(string isbn)
    {
        var client = _http.CreateClient("Soap");
        var envelope =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">
  <s:Body>
    <GetBookByIsbn xmlns=""http://tempuri.org/"">
      <isbn>{System.Security.SecurityElement.Escape(isbn)}</isbn>
    </GetBookByIsbn>
  </s:Body>
</s:Envelope>";
        var resp = await client.PostAsync("", XmlContent(envelope, "\"http://tempuri.org/ILibrarySoapService/GetBookByIsbn\""));
        resp.EnsureSuccessStatusCode();
        var doc = XDocument.Parse(await resp.Content.ReadAsStringAsync());

        // Find <GetBookByIsbnResult> then its child elements
        var result = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "GetBookByIsbnResult");
        if (result == null || !result.Elements().Any()) return null;

        int Int(string name) => int.TryParse(result.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value, out var i) ? i : 0;
        string Str(string name) => result.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? "";

        return new SoapBookDto(
            Id: Int("Id"),
            Isbn: Str("Isbn"),
            Title: Str("Title"),
            Author: Str("Author"),
            Year: Int("Year"),
            TotalCopies: Int("TotalCopies"),
            AvailableCopies: Int("AvailableCopies")
        );
    }
}
