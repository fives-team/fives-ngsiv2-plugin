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
using NGSIv2Plugin.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSIv2Plugin
{
    public class NGSIv2Processor
    {
        private NGSIv2Client ngsiClient;

        private void RegisterSinfoniService()
        {
            string idlContents = File.ReadAllText("ngsi.sinfoni");
            SINFONIPlugin.SINFONIServerManager.Instance.SinfoniServer.AmendIDL(idlContents);
            ClientManager.Instance.RegisterClientService("ngsi", true, new Dictionary<string, Delegate>{
                {"createEntity", (Action<Dictionary<string, object>>)CreateEntity},
                {"deleteEntity", (Action<string>)DeleteEntity},
                {"updateOrAppend", (Action<string,string,Dictionary<string,object>>)UpdateOrAppend},
                {"retrieveEntityData", (Action<string>)RetrieveEntityData},
                {"listEntities", (Action)ListAllEntities}
            });
        }

        private void RetrieveEntityCollection()
        {
            ngsiClient.EntityCollection.ListAllEntities(r =>
            {
                foreach (Dictionary<string, object> e in r.ResponseData)
                {
                    var ngsiEntity = new EntityObject(e);
                    Entity fivesEntity = GetFivesEntity(ngsiEntity);
                    ApplyNgsiAttributes(fivesEntity, ngsiEntity);
                }
            });
        }

        private Entity GetFivesEntity(EntityObject ngsiEntity)
        {
            Entity fivesEntity = new Entity();
            try
            {
                fivesEntity = World.Instance.First(entity
                    => entity.ContainsComponent("ngsi") && entity["ngsi"]["id"].Value.Equals(ngsiEntity["id"]));
            }
            catch (InvalidOperationException)
            {
                fivesEntity["ngsi"]["id"].Suggest(ngsiEntity["id"]);
                fivesEntity["ngsi"]["type"].Suggest(ngsiEntity["type"]);
                World.Instance.Add(fivesEntity);
            }

            return fivesEntity;
        }

        private void ApplyNgsiAttributes(Entity fivesEntity, EntityObject ngsiEntity)
        {
            foreach (KeyValuePair<string, object> val in ngsiEntity)
            {
                if (val.Key != "id" && val.Key != "type")
                {
                    var entityComponent = fivesEntity[val.Key];
                    var attributeValue = ((Dictionary<string, object>)val.Value)["value"];

                    foreach (KeyValuePair<string, object> attr in (Dictionary<string, object>)attributeValue)
                    {
                        entityComponent[attr.Key].Suggest(attr.Value);
                    }
                }
            }
        }
    }
}
