using System.Linq;
using Fab.Server.Core;

namespace Fab.Server
{
    public class AccountingService : IAccountingService
    {
        #region IAccountingService Members

        public string GetJournalTypeName(int value)
        {
            using (var mc = new ModelContainer())
            {
                var journalType = mc.JournalTypeSet.Where(t => t.Id == value).SingleOrDefault();
                
                return journalType != null
                           ? journalType.Type
                           : string.Empty;
            }
        }

        public JournalType GetJournalType(int value)
        {
            using (var mc = new ModelContainer())
            {
                return mc.JournalTypeSet.Where(t => t.Id == value).SingleOrDefault();
            }
        }

        #endregion
    }
}