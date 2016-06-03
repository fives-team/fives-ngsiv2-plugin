// This file is part of FiVES.
//
// FiVES is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation (LGPL v3)
//
// FiVES is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with FiVES.  If not, see <http://www.gnu.org/licenses/>.
using ClientManagerPlugin;
using FIVES;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSIv2Plugin
{
    public class NGSIv2PluginInitializer : IPluginInitializer
    {
        #region Plugin Interface
        public List<string> ComponentDependencies
        {
            get { return new List<string>(); }
        }

        public void Initialize()
        {
            RESTServicePlugin.RequestDispatcher.Instance.RegisterHandler(new NGSIv2RequestHandler());
        }

        public string Name
        {
            get { return "NGSIv2"; }
        }

        public List<string> PluginDependencies
        {
            get { return new List<string> { "RESTService" }; }
        }

        public void Shutdown()
        {

        }
        #endregion

        private void RegisterNGSIComponent()
        {
            ComponentDefinition ngsi = new ComponentDefinition("ngsi");
            ngsi.AddAttribute<string>("id");
            ngsi.AddAttribute<string>("type");
            ComponentRegistry.Instance.Register(ngsi);
        }

        private void RegisterSinfoniService()
        {
            string idlContents = File.ReadAllText("ngsi.sinfoni");
            SINFONIPlugin.SINFONIServerManager.Instance.SinfoniServer.AmendIDL(idlContents);
            ClientManager.Instance.RegisterClientService("ngsi", true, new Dictionary<string, Delegate>{
                {"createEntity", (Action<Dictionary<string, object>>)NGSIv2Processor.Instance.CreateEntity},
                {"deleteEntity", (Action<string>)NGSIv2Processor.Instance.DeleteEntity},
                {"updateOrAppend",
                    (Action<string,string,Dictionary<string,object>>)NGSIv2Processor.Instance.UpdateOrAppend},
                {"retrieveEntityData", (Action<string>)NGSIv2Processor.Instance.RetrieveEntityData},
                {"listEntities", (Action)NGSIv2Processor.Instance.RetrieveEntityCollection}
            });
        }
    }
}
