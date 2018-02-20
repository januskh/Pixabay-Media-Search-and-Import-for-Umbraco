using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Core.IO;

namespace Umbraco781.Install
{
    public class PixabayInstaller : IPackageAction
    {

        private const string caption = "Pixabay";

        private const string sectionAlias = "Media";

        private string sectionXPth = $"//section[@alias='{sectionAlias}']";

        private string tabXPath = $"//section[@alias='{sectionAlias}']/tab[@caption='{caption}']";

        public string Alias()
        {
            return $"Umbraco{caption}{sectionAlias}DashboardSection";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {

            var dashboardConfig = SystemFiles.DashboardConfig;
            XmlDocument dashboardFile = XmlHelper.OpenAsXmlDocument(dashboardConfig);

            XmlNode existingSection = dashboardFile.SelectSingleNode(sectionXPth);

            if (existingSection == null)
            {
                string xmlsection = $@"<section alias=""{this.Alias()}""><areas><area>{sectionAlias}</area></areas>
                                <tab caption=""{caption}"">
                              <control showOnce=""false"" addPanel=""true"">~/App_Plugins/Umbraco.Pixabay/views/browse.html</control>
                                </tab></section> ";

                XmlNode dashboardNode = dashboardFile.SelectSingleNode("//dashBoard");

                if (dashboardNode != null)
                {
                    XmlDocument xmlNodeToAdd = new XmlDocument();
                    xmlNodeToAdd.LoadXml(xmlsection);

                    XmlNode node = xmlNodeToAdd.SelectSingleNode("*");

                    dashboardNode.PrependChild(dashboardNode.OwnerDocument.ImportNode(node, true));

                    dashboardFile.Save(IOHelper.MapPath(dashboardConfig));
                }

            }

            return true;

        }

        public XmlNode SampleXml()
        {
            var xml = $"<Action runat=\"install\" undo=\"true\" alias=\"{this.Alias()}\" />";
            XmlDocument x = new XmlDocument();
            x.LoadXml(xml);
            return x;
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            string dbConfig = SystemFiles.DashboardConfig;
            XmlDocument dashboardFile = XmlHelper.OpenAsXmlDocument(dbConfig);

            XmlNode section = dashboardFile.SelectSingleNode(sectionXPth);

            if (section != null)
            {

                dashboardFile.SelectSingleNode(sectionXPth).RemoveChild(section);
                dashboardFile.Save(IOHelper.MapPath(dbConfig));
            }

            return true;
        }
    }
}