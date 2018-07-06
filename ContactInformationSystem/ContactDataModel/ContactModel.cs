using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ContactDataModel
{
    public class ContactDetails
    {
        [Required(ErrorMessage = "Please Enter ContactID")]
        [DisplayName("ContactID:")]
        public int ContactID { get; set; }
        
        [Required(ErrorMessage = "Please Enter First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please Only Enter Letters.")]
        [StringLength(50, ErrorMessage = "First Name Maximum Limit 50 Character ", MinimumLength = 1)]
        [DisplayName("First Name:")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Please Enter Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please Only Enter Letters.")]
        [StringLength(50, ErrorMessage = "Last Name Maximum Limit 50 Character ", MinimumLength = 1)]
        [DisplayName("Last Name:")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Please Enter Email")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Please Enter Valid Email.")]
        [DisplayName("Email:")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Please Enter Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [DisplayName("Phone Number:")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Please Select Status")]
        [DisplayName("Status:")]
        public string Status { get; set; }

        [XmlIgnore]
        public string EntryDate { get; set; }

    }
    
}
