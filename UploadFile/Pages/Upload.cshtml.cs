using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UploadFile.Pages
{
    public class UploadModel : PageModel
    {                  

        private readonly UploadfileContext _context;

        public UploadModel(UploadfileContext context)
        {
            
            _context = context;
        }
        public int ? myID { get; set; }

        [BindProperty]
        public IFormFile file { get; set; }

        [BindProperty]
        public int ? ID { get; set; }
        public void OnGet(int? id)
        {
            myID = id;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (file != null)
            {
                if (file.Length > 0 && file.Length < 300000)
                {
                    var myInv = _context.Invoices.FirstOrDefault(x => x.Id == ID);

                    using (var target = new MemoryStream())
                    {
                        file.CopyTo(target);
                        myInv.Attachment = target.ToArray();
                    }

                    _context.Invoices.Update(myInv);
                    await _context.SaveChangesAsync();
                }

            }

            return RedirectToPage("./Index");
        }

    }
}
