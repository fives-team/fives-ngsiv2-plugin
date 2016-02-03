using RESTServicePlugin;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGSIv2Plugin
{
    class NGSIv2RequestHandler : RequestHandler
    {
        public override string ContentType
        {
            get { throw new NotImplementedException(); }
        }

        protected override RequestResponse HandleDELETE(string requestPath)
        {
            throw new NotImplementedException();
        }

        protected override RequestResponse HandleGET(string requestPath)
        {
            throw new NotImplementedException();
        }

        protected override RequestResponse HandlePOST(string requestPath, string content)
        {
            throw new NotImplementedException();
        }

        protected override RequestResponse HandlePUT(string requestPath, string content)
        {
            throw new NotImplementedException();
        }

        public override string Path
        {
            get { return "v2"; }
        }
    }
}
