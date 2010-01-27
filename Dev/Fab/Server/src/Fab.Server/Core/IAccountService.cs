using System.Runtime.Serialization;
using System.ServiceModel;

namespace Fab.Server.Core
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        string GetJournalTypeName(int value);

        [OperationContract]
        JournalType GetJournalType(int value);
    }

    [DataContract]
    public class CompositeType
    {
        private bool boolValue = true;
        private string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}