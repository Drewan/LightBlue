using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LightBlue.WebService
{
    public class WebConfiguration : IDisposable
    {
        private readonly FileInfo _file;
        private readonly XDocument _xml;

        public static WebConfiguration Load(string filepath)
        {
            var file = new FileInfo(filepath);
            if (!file.Exists)
                throw new InvalidOperationException("Web.config file does not exist at " + filepath);
            var xml = XDocument.Load(file.OpenRead());
            return new WebConfiguration(file, xml);
        }

        private WebConfiguration(FileInfo file, XDocument xml)
        {
            _file = file;
            _xml = xml;
        }

        public void RemoveTraceListener(string name)
        {
            if (_xml.Root == null)
                return;

            var diagnostics = _xml.Root.Element("system.diagnostics");
            if (diagnostics == null)
                return;

            var listener = diagnostics
                .Descendants("listeners")
                .FirstOrDefault(l => l.Parent != null && l.Parent.Name == "trace");
            if (listener == null)
                return;

            var settings = listener.Descendants("add")
                .Where(a => a.Attribute("type").Value.StartsWith(name));

            foreach (var element in settings)
                element.Remove();
        }

        public void Dispose()
        {
            _xml.Save(_file.OpenWrite());
        }
    }
}