using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ContactDataModel;
using ContactDataHelper;
using System.Configuration;

namespace ContactsRestApi.Controllers
{
    
    public class ContactController : ApiController
    {
        public string xmlPath = ConfigurationManager.AppSettings["xmlPath"];
    
        [HttpGet]
        public List<ContactDetails> Get()
        {
            List<ContactDetails> lstContractDetails = null;
            DataHolder objDataHolder = new DataHolder(xmlPath);
            lstContractDetails = objDataHolder.GetContactList();
            return lstContractDetails.ToList();
        }

        [HttpGet]
        public ContactDetails Get(int id)
        {

            ContactDetails contact = new ContactDetails();
            DataHolder objDataHolder = new DataHolder(xmlPath);
            contact = objDataHolder.SelectContact(id);
            return contact;
        }
        
        [HttpPost]
        [ActionName("Create")]
        public string Post(ContactDetails contact)
        {
            string message = "";
            try
            {
                DataHolder objDataHolder = new DataHolder(xmlPath);
                objDataHolder.AddContact(contact);

                message = "Contract added successfully!!!";
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return message;
        }

        [HttpPost]
        [ActionName("Update")]
        public string Put(ContactDetails contact)
        {
            string message = "";
            try
            {
                DataHolder objDataHolder = new DataHolder(xmlPath);
                objDataHolder.UpdateContact(contact);

                message = "Contact Details updated having name " + contact.FirstName + " " + contact.LastName;
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return message;
        }
        
        [HttpPost]
        [ActionName("Delete")]
        public string Delete(ContactDetails contact)
        {
            string message = "";
            try
            {
                DataHolder objDataHolder = new DataHolder(xmlPath);
                objDataHolder.DeleteContact(contact.ContactID);
                message = "Employee Details deteled having id " + contact.ContactID.ToString();
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            return message;
        }
    }
}
