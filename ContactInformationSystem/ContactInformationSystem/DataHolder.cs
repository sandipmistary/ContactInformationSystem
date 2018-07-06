using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContactDataModel;

namespace ContactDataHelper
{
    public class DataHolder
    {
        public string xmlFileName { get; set; }

        public DataHolder(string fileName)
        {
            xmlFileName = fileName;
        }
        
        public void AddContact(ContactDetails contact)
        {
            try
            {
                var cDb = GetContactDB();
                List<ContactDetails> allConts = cDb.ContactList;
                cDb.MaxContactID++;
                contact.ContactID = cDb.MaxContactID;

                if (allConts == null)
                {
                    allConts = new List<ContactDetails>();
                }
                allConts.Add(contact);
                cDb.ContactList = allConts;
                Save(cDb);
            }
            finally
            {
            }
        }

        private void SaveList(List<ContactDetails> objContactList)
        {
            XmlSerializer objXmlSerializer = null;
            TextWriter objTextWriter = null;
            try
            {
                objXmlSerializer = new XmlSerializer(typeof(List<ContactDetails>));

                using (objTextWriter = new StreamWriter(xmlFileName))
                {
                    objXmlSerializer.Serialize(objTextWriter, objContactList);
                }
            }
            finally
            {
                objTextWriter.Close();
                objTextWriter = null;
                objXmlSerializer = null;
            }
        }

        private void Save(ContactData contact)
        {
            XmlSerializer objXmlSerializer = null;
            TextWriter objTextWriter = null;
            try
            {
                objXmlSerializer = new XmlSerializer(typeof(ContactData));

                using (objTextWriter = new StreamWriter(xmlFileName))
                {
                    objXmlSerializer.Serialize(objTextWriter, contact);
                }
            }
            finally
            {
                objTextWriter.Close();
                objTextWriter = null;
                objXmlSerializer = null;
            }
        }

        public void UpdateContact(ContactDetails contact)
        {
            var cDb = GetContactDB();
            List<ContactDetails> allConts = cDb.ContactList;
            var obj = allConts.FirstOrDefault(x => x.ContactID == contact.ContactID);
            if (obj != null)
            {
                obj.ContactID = contact.ContactID;
                obj.FirstName = contact.FirstName;
                obj.LastName = contact.LastName;
                obj.PhoneNumber = contact.PhoneNumber;
                obj.Email = contact.Email;
                obj.Status = contact.Status;
            }
            Save(cDb);

        }

        public ContactDetails SelectContact(int contactID)
        {
            ContactDetails selectedContact = new ContactDetails();
            var cDb = GetContactDB();
            List<ContactDetails> allConts = cDb.ContactList;
            var obj = allConts.FirstOrDefault(x => x.ContactID == contactID);
            if (obj != null)
            {
                selectedContact.ContactID = contactID;
                selectedContact.FirstName = obj.FirstName;
                selectedContact.LastName = obj.LastName;
                selectedContact.PhoneNumber = obj.PhoneNumber;
                selectedContact.Email = obj.Email;
                selectedContact.Status = obj.Status;
            }
            return selectedContact;
        }

        public void DeleteContact(int contactID)
        {
            var cDb = GetContactDB();
            List<ContactDetails> allConts = cDb.ContactList;
            var index = allConts.FindIndex(item => item.ContactID == contactID);
            allConts.RemoveAt(index);
            cDb.ContactList = allConts;
            Save(cDb);
        }

        private ContactData GetContactDB()
        {
            XmlSerializer objXmlSerializer = null;
            TextReader reader = null;
            try
            {
                objXmlSerializer = new XmlSerializer(typeof(ContactData));
                objXmlSerializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                objXmlSerializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);
                ContactData xmlData = null;
                if (System.IO.File.Exists(xmlFileName))
                {
                    reader = new StreamReader(xmlFileName);
                    object obj = objXmlSerializer.Deserialize(reader);
                    xmlData = (ContactData)obj;
                    reader.Close();
                }


                if (xmlData == null)
                {
                    xmlData = new ContactData();
                }

                return xmlData;

            }
            finally
            {
                objXmlSerializer = null;
                reader = null;
            }
        }

        public List<ContactDetails> GetContactList()
        {

            try
            {
                var cDb = GetContactDB();
                List<ContactDetails> xmlData = cDb.ContactList;

                if (xmlData == null)
                {
                    xmlData = new List<ContactDetails>();
                }

                return xmlData;

            }
            finally
            {

            }
        }

        protected void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            throw new Exception("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        protected void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            throw new Exception("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
        }
    }
}
