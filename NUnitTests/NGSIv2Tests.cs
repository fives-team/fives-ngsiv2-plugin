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
using NGSIv2Plugin.Messages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSIv2Plugin.NUnitTests
{
    [TestFixture()]
    class NGSIv2Tests
    {
        private AsyncRESTClient dispatcher = new AsyncRESTClient();

        [Test()]
        public async Task ShouldRetrieveAllEntities()
        {
            var entities = await dispatcher.Get<Dictionary<string,object>[]>("http://130.206.117.75:1026/v2/entities");
            Assert.NotNull(entities);
        }
    }
}
