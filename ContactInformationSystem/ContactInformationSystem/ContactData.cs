using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDataHelper
{
    [Serializable]
    public class ContactData
    {
        public List<ContactDataModel.ContactDetails> ContactList { get; set; }
        public int MaxContactID { get; set; }
    }
}
