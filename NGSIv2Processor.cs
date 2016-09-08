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
    /// <summary>
    /// Works as a processing unit between the NGSIv2 Client that retrieves NGSIv2 formatted raw data from the NGSIv2
    /// endpoint, and the FiVES runtime data model. Allows to retrieve entities annd their data from the endpoint and
    /// apply it to the FiVES runtime data
    /// </summary>
    public class NGSIv2Processor
    {
        private NGSIv2Client ngsiClient;

        private static NGSIv2Processor instance;

        /// <summary>
        /// Singleton Instance object of the Processor that exposes Entity operation methods to the outside
        /// </summary>
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

        /// <summary>
        /// Creates a new entity within the NGSIv2 endpoint. This object must be compliant to the NGSIv2 specification
        /// </summary>
        /// <param name="newEntity">Entity in NGSIv2 format that should be created in NGSIv2 endpoint</param>
        public void CreateEntity(Dictionary<string, object> newEntity)
        {
            EntityObject e = new EntityObject(newEntity);
            ngsiClient.EntityIngestion.CreateEntity(e, r =>
            {

            });
        }

        /// <summary>
        /// Retrieves all entities from the NGSIv2 endpoint and copies them to the FiVES runtime data, creating respective
        /// objects locally, or applying updated data to those entities which were already created before
        /// </summary>
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

        /// <summary>
        /// Retrieves the data for one entity from the NGSIv2 endpoint
        /// </summary>
        /// <param name="entityId">ID of the entity under which it is stored in the NGSIv2 endpoint</param>
        public void RetrieveEntityData(string entityId)
        {
            ngsiClient.EntityContext.RetrieveEntityData(entityId, r =>
            {
                Entity fivesEntity = GetFivesEntity(r.ResponseData);
                ApplyNgsiAttributes(fivesEntity, r.ResponseData);
            });
        }

        /// <summary>
        /// Updates an attribute value, or appends a new attribute, in an NGSIv2 entity
        /// </summary>
        /// <param name="id">ID of the entity in the NGSIv2 endpoint</param>
        /// <param name="attributeName">Name of the attribute that should be updated</param>
        /// <param name="attributeUpdate">New attribute object according to NGSIv2 spec</param>
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

        /// <summary>
        /// Deletes an entity from the NGSIv2 endpoint
        /// </summary>
        /// <param name="entityId">Id under which the entity is stored in the NGSIv2 endpoint</param>
        /// <param param name="deleteLocal">Specifies whether the local entity should be deleted from FiVES</param>
        public void DeleteEntity(string entityId, bool deleteLocal)
        {
            Entity fivesEntity = new Entity();
            bool entityExists = true;
            try
            {
                fivesEntity = World.Instance.First
                    (entity => entity.ContainsComponent("ngsi") && entity["ngsi"]["id"].Value.Equals(entityId));
            }
            catch
            {
                entityExists = false;
            }
            if (entityExists && deleteLocal)
            {
                World.Instance.Remove(fivesEntity);
            }
            ngsiClient.EntityIngestion.RemoveEntity(entityId, r =>
            {

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
