using NGSIv2Plugin.Messages;
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
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NGSIv2Plugin.NUnitTests
{
    [TestFixture()]
    class NGSIv2Tests
    {
        private NGSIv2Client ngsiClient = new NGSIv2Client(globalContextBrokerURI);

        private const string globalContextBrokerURI = "http://130.206.117.75:1026";

        [Test()]
        public void ShouldRetrieveEntryPoints()
        {
            var tempClient = new NGSIv2Client(globalContextBrokerURI);
            Assert.NotNull(tempClient.EntryPoints.Entities);
            Assert.NotNull(tempClient.EntryPoints.Types);
            Assert.NotNull(tempClient.EntryPoints.Subscriptions);
            Assert.NotNull(tempClient.EntryPoints.Registrations);
        }

        [Test()]
        public void ShouldRetrieveAllEntities()
        {
            var autoEvent = new AutoResetEvent(false);
            EntityList entities = new EntityList();
            RequestResponse<EntityList> response = new RequestResponse<EntityList>();
            ngsiClient.EntityCollection.ListAllEntities(r => {
                response = r;
                entities = r.ResponseData as EntityList;
                autoEvent.Set();
            });
            autoEvent.WaitOne();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.Greater(entities.Count, 0);
        }
    }
}
