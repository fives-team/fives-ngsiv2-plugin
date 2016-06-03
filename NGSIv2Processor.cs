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
using FIVES;
using NGSIv2Plugin.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NGSIv2Plugin
{
    public class NGSIv2Processor
    {
        private NGSIv2Client ngsiClient;

        private static NGSIv2Processor instance;

        public static NGSIv2Processor Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NGSIv2Processor();
                }
                return instance;
            }
        }

        private NGSIv2Processor()
        {
            XmlDocument configXml = new XmlDocument();
            configXml.Load(this.GetType().Assembly.Location + ".config");
            XmlNode ngsiNode = configXml.SelectSingleNode("configuration/ngsi-endpoint");
            string ngsiHost = ngsiNode.Attributes["host"].Value + ":" + ngsiNode.Attributes["port"].Value;
            ngsiClient = new NGSIv2Client(ngsiHost);
        }

        public void CreateEntity(Dictionary<string, object> newEntity)
        {
            EntityObject e = new EntityObject(newEntity);
            ngsiClient.EntityIngestion.CreateEntity(e, r =>
            {

            });
        }

        public void RetrieveEntityCollection()
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

        public void RetrieveEntityData(string entityId)
        {
            ngsiClient.EntityContext.RetrieveEntityData(entityId, r =>
            {
                Entity fivesEntity = GetFivesEntity(r.ResponseData);
                ApplyNgsiAttributes(fivesEntity, r.ResponseData);
            });
        }

        public void UpdateOrAppend(string id, string attributeName, Dictionary<string, object> attributeUpdate)
        {
            Entity fivesEntity = new Entity();
            bool entityExists = true;
            try
            {
                fivesEntity = World.Instance.First(entity => entity.ContainsComponent("ngsi") && entity["ngsi"]["id"].Value.Equals(id));
            }
            catch
            {
                entityExists = false;
            }
            if (entityExists)
            {
                var entityComponent = fivesEntity[attributeName];
                var attributeValue = attributeUpdate["value"];
                foreach (KeyValuePair<string, object> attr in (Dictionary<string, object>)attributeValue)
                {
                    entityComponent[attr.Key].Suggest(attr.Value);
                }
            }

            AttributeObject update = new AttributeObject();
            update.value = attributeUpdate["value"];
            ngsiClient.AttributeContext.UpdateAttribute(id, attributeName, update, r => { });
        }

        public void DeleteEntity(string entityId)
        {
            Entity fivesEntity = new Entity();
            bool entityExists = true;
            try
            {
                fivesEntity = World.Instance.First(entity => entity.ContainsComponent("ngsi") && entity["ngsi"]["id"].Value.Equals(entityId));
            }
            catch
            {
                entityExists = false;
            }
            if (entityExists)
            {
                World.Instance.Remove(fivesEntity);
            }
            ngsiClient.EntityIngestion.RemoveEntity(entityId, r =>
            {

            });
        }
    }
}
