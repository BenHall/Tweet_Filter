using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;

using Microsoft.ComponentModel.Composition.Scripting;
using Microsoft.Scripting.Hosting;

using System.ComponentModel.Composition;

namespace ClassLibrary1
{
    //Build of DLR, IronRuby taken from 
    //http://chris.caliberweb.com/2009/06/01/mef-the-dlr-ironruby-and-the-web/
    public class Composer
    {
        private readonly object part;
        private readonly string rubyScriptsPath;
        private readonly ComposablePartCatalog[] otherCatalogs;

        public Composer(object part, string rubyScriptsPath, params ComposablePartCatalog[] otherCatalogs)
        {
            this.part = part;
            this.rubyScriptsPath = rubyScriptsPath;
            this.otherCatalogs = otherCatalogs;
        }

        public void Compose()
        {
            var catalog = new AggregateCatalog();

            foreach (var extraCatalog in otherCatalogs)
            {
                catalog.Catalogs.Add(extraCatalog);
            }

            var source = new RubyDirectoryPartSource(rubyScriptsPath);
            var rubyCatalog = new RubyCatalog(source);
            catalog.Catalogs.Add(rubyCatalog);

            var batch = new CompositionBatch();
            batch.AddPart(part);

            new CompositionContainer(catalog).Compose(batch);
        }

        private class RubyDirectoryPartSource : RubyPartSource
        {
            private readonly string path;

            public RubyDirectoryPartSource(string path)
            {
                this.path = path;
            }

            public override void Execute(ScriptEngine scriptEngine)
            {
                if (scriptEngine == null)
                {
                    throw new ArgumentNullException("scriptEngine");
                }

                var info = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
                if (info.Exists == false)
                {
                    throw new Exception(string.Format("Can't find path: {0}", path));
                }

                var rubyFiles = info.GetFiles("*.rb");

                foreach (FileInfo file in rubyFiles)
                {
                    new RubyPartFile(file.FullName).Execute(scriptEngine);
                }
            }
        }
    }
}