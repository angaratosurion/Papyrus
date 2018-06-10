using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Papyrus.Core.SDK.Interfaces;
using System.IO;

namespace Papyrus.Core.SDK
{
    public static  class BootStraper
    {
        [ImportMany(typeof(IFileTypePlugin))]
        public static IEnumerable<Lazy<IFileTypePlugin>> FileTypePlugins;
        private static CompositionContainer CompositionContainer;
        private static bool IsLoaded = false;
        //   static CommonTools cmTools = new CommonTools();
        static AggregateCatalog Catlgs;
        [ImportMany]
        private static IEnumerable<Lazy<IPluginInfo>>PluginInfos;
       
        public static void BootStrap()
        {
            try
            {
                int i;
                var pluginFolders = new List<string>();

                var plugins = Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"));
                if ( plugins == null)
                {
                    return;
                }
                for (i = 0; i < plugins.Length; i++)
                {

                    
                        var di = new DirectoryInfo(plugins[i]);
                        pluginFolders.Add(di.Name);
                  
                }



                Compose(pluginFolders);

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
            }
        }

        public static void Compose(List<string> pluginFolders)
        {
            try
            {

                if (IsLoaded) return;

                var catalog = new AggregateCatalog();

                catalog.Catalogs.Add(new DirectoryCatalog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory)));

                foreach (var plugin in pluginFolders)
                {
                    var directoryCatalog = new DirectoryCatalog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        "Plugins", plugin));
                    catalog.Catalogs.Add(directoryCatalog);

                }
                CompositionContainer = new CompositionContainer(catalog);

                CompositionContainer.ComposeParts();
                PluginInfos = CompositionContainer.GetExports<IPluginInfo>();
                FileTypePlugins = CompositionContainer.GetExports<IFileTypePlugin>();


                Catlgs = catalog;
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }

        public static T GetInstance<T>(string contractName = null)
        {
            try
            {

                var type = default(T);

                if (CompositionContainer == null) return type;

                if (!string.IsNullOrWhiteSpace(contractName))
                {
                    type = CompositionContainer.GetExportedValue<T>(contractName);
                }
                else
                {
                    type = CompositionContainer.GetExportedValue<T>();
                }


                return type;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return default(T);
            }
        }
        public static List<T> GetInstances<T>(string contractName = null)
        {
            try
            {

                List<T> type = null;
                int i = 0;
                if (CompositionContainer == null) return type;

                if (!string.IsNullOrWhiteSpace(contractName))
                {
                     type=(List<T>)CompositionContainer.GetExportedValues<T>(contractName);
                   
                    
                
                }
                else
                {
                    type = (List<T>)CompositionContainer.GetExportedValues<T>();
                }


                return type;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }

    }
}
