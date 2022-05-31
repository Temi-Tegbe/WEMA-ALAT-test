using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Helpers
{
    public class ManagerBase<TEntity> : BaseRepository<TEntity> where TEntity : class
    {
        public ManagerBase(DbContext context) : base(context)
        {

        }
    }
}
