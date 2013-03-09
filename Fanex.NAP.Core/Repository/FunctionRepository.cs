using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Core.Domain;
using Fanex.NAP.Core.Command;
using Fanex.Data;

namespace Fanex.NAP.Core.Repository
{
    public class FunctionRepository : IRepository<Function>
    {
        public Function Get(int Id)
        {
            throw new NotImplementedException();
        }

        public Function Get(ICommand command)
        {
            using (ObjectDb objectDb = new ObjectDb(command.SettingKey))
            {
                return objectDb.QueryEntity<Function>(command.GetParameter());
            }
        }

        public void Add(Function entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Function entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Function> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
