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
                var journalType = mc.JournalTypes.Where(t => t.Id == value).SingleOrDefault();
                
                return journalType != null
                           ? journalType.Name
                           : string.Empty;
            }
        }

        public JournalType GetJournalType(int value)
        {
            using (var mc = new ModelContainer())
            {
                return mc.JournalTypes.Where(t => t.Id == value).SingleOrDefault();
            }
        }

        #endregion
    }
}