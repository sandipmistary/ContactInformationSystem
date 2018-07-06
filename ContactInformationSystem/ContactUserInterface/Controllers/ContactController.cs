using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using ContactDataModel;
using System.Configuration;

namespace ContactUserInterface.Controllers
{
    public class ContactController : Controller
    {
        public string baseURI = ConfigurationManager.AppSettings["ContactApiURI"];

        #region GET CONTACT LIST

        public async Task<ActionResult> ContactList()
        {
            List<ContactDetails> objContactDetails = new List<ContactDetails>();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(baseURI);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllContacts using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("Contact/Get");

                if (Res.ReasonPhrase == "OK")
                {
                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var contactApiResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Contact list  
                        objContactDetails = JsonConvert.DeserializeObject<List<ContactDetails>>(contactApiResponse);

                    }
                }
                else if (Res.ReasonPhrase == "Unauthorized")
                {

                }
                //returning the contact list to view  
                return View(objContactDetails);
            }
        }

        #endregion

        #region GET AND POST FOR NEW CONTACT

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ContactDetails contact)
        {
            if (ModelState.IsValid)
            {
                contact.EntryDate = System.DateTime.Now.ToLongDateString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURI);
                    var postTask = client.PostAsync("Contact/Create", new StringContent(new JavaScriptSerializer().Serialize(contact), Encoding.UTF8, "application/json"));
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ContactList");
                    }
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return RedirectToAction("ContactList");
            }
            else
            {
                return View();
            }
            
        }

        #endregion

        #region UPDATE CONTACT

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            ContactDetails objContactDetails = new ContactDetails();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(baseURI);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("Contact/Get?id=" + id);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    var contactApiResponse = Res.Content.ReadAsStringAsync().Result;

                    objContactDetails = JsonConvert.DeserializeObject<ContactDetails>(contactApiResponse);
                }
                return View(objContactDetails);
            }
        }

        [HttpPost]
        public ActionResult Edit(ContactDetails contact)
        {
            if (ModelState.IsValid)
            {
                contact.EntryDate = System.DateTime.Now.ToLongDateString();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURI);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var postTask = client.PostAsync("Contact/Update", new StringContent(new JavaScriptSerializer().Serialize(contact), Encoding.UTF8, "application/json"));
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ContactList");
                    }
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return RedirectToAction("ContactList");
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region CONTACT DETAILS

        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            ContactDetails objContactDetails = new ContactDetails();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(baseURI);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("Contact/Get?id=" + id);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    var contactApiResponse = Res.Content.ReadAsStringAsync().Result;

                    objContactDetails = JsonConvert.DeserializeObject<ContactDetails>(contactApiResponse);
                }
                return View(objContactDetails);
            }
        }
        #endregion

        #region DELETE CONTACT

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            ContactDetails EmpInfo = new ContactDetails();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(baseURI);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("Contact/Get?id=" + id);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    EmpInfo = JsonConvert.DeserializeObject<ContactDetails>(EmpResponse);
                }
                //returning the employee list to view  
                return View(EmpInfo);
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteContact(string id)
        {
            ContactDetails contact = new ContactDetails();
            contact.ContactID = Convert.ToInt32(id);
            contact.EntryDate = System.DateTime.Now.ToLongDateString();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURI);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var postTask = client.PostAsync("Contact/Delete", new StringContent(new JavaScriptSerializer().Serialize(contact), Encoding.UTF8, "application/json"));
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("ContactList");
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return RedirectToAction("ContactList");
        }
       
        #endregion
    }
}