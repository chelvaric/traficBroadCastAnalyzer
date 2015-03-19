using BeMobile.Global.Serialization;
using BeMobile.TrafficBroadcastAnalyzerService.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using Topshelf;

namespace BeMobile.TrafficBroadcastAnalyzerService
{
    public sealed class Program
    {
        private const string serviceName = "BeMobile.TrafficBroadcastAnalyzerService";
        private const string serviceDisplayName = "BeMobile.TrafficBroadcastAnalyzerService";
        private const string configFileXmlRoot = "BeMobile.TrafficBroadcastAnalyzerService";
        private static  string configFilePath;
        private static List<ConfigurationEntry> configurationEntries;

        static Program()
        {
            configFilePath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "config.xml";

            

            CreateConfigFile();

    /*       configurationEntries = XmlSerializer<List<ConfigurationEntry>>.DeserializeFromFile(
                configFilePath,
                configFileXmlRoot);*/
        }

        public static void Main(string[] args)
        {
            HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.Service<TrafficBroadcastAnalyzerService>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(() => new TrafficBroadcastAnalyzerService(configurationEntries));
                    serviceConfigurator.WhenStarted(service => service.Start());
                    serviceConfigurator.WhenStopped(service => service.Stop());
                });
                hostConfigurator.SetServiceName(serviceName);
                hostConfigurator.SetDisplayName(serviceDisplayName);
                hostConfigurator.SetDescription(serviceDisplayName);
                hostConfigurator.RunAsLocalSystem();
                hostConfigurator.StartAutomatically();
                hostConfigurator.EnableServiceRecovery(recoveryConfigurator => recoveryConfigurator.RestartService(1));
            });

            

            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            }

        private static void CreateConfigFile()
        {
            configurationEntries = new List<ConfigurationEntry>
            {
                new RdsTmcConfigurationEntry
                {
                    Name = "RdsTmcEntry1",
                    RepositoryUsername = "bemobile",
                    RepositoryPassword = "oY1qR0EW9XYiGRFhXfESSjoHzZpfbv",
                    ReferenceTrafficEventsMapId = "BE-LTN01-v2.8",
                    UecpServiceHostname = "example.com",
                    UecpServicePort = 8888
                }
            };
          //XmlSerializer<List<ConfigurationEntry>>.SerializeToFile(
          //      configFilePath,
          //      configurationEntries,
          //      configFileXmlRoot);
        }
    }
}
