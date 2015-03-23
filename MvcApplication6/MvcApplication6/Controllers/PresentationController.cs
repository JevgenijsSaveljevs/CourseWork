using Aspose.Slides;
using Aspose.Slides.Export;
using MvcApplication6.Filters;
using MvcApplication6.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication6.Controllers
{
    public class PresentationController : Controller
    {

        MvcApplication6.Filters.PptException PptEx = null;
        //
        // GET: /Presentation/

        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult SomeAction()
        {
            try
            {
                thread = new Thread(test);

                thread.Start();
                //var k = 0;
                //var z = 1 / k;

                return Json("W");
            }
            catch
            {
                throw new NotImplementedException();
            }


        }

        [PptExceptionHandler]
        private void test()
        {
            try
            {
                var k = 0;
                var z = 1 / k;
                thread.Abort();
            }
            catch (Exception)
            {

                //throw new Exception();
            }
        }

        [PptExceptionHandler]
        public JsonResult Upload()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                //Use the following properties to get file's name, size and MIMEType
                int fileSize = file.ContentLength;
                string fileName = file.FileName;
                string mimeType = file.ContentType;
                System.IO.Stream fileContent = file.InputStream;
                ////To save file, use SaveAs method
                // file.SaveAs(Server.MapPath("~/") + fileName); //File will be saved in application root
                try
                {
                    thread = new Thread(o => generatePPT(fileContent));

                    thread.Start();
                    var z = thread.ExecutionContext;
                    thread.Join();
                }
                catch (Exception ex)
                {
                    return Json("We are sorry, but exception occured during convertation procces. Possible reason : " + ex.Message);
                }
                if (PptEx != null)
                {
                    return Json("We are sorry, but exception occured during convertation procces. Possible reason : " + PptEx.GetExMsg());
                }
            }

            return Json("Uploaded " + Request.Files.Count + " files");


        }

        Thread thread;
        [HandleError]
        [HandleError()]
        private void generatePPT(Stream fileStream)
        {
            // The path to the documents directory.
          //  string dataDir = Path.GetFullPath("ss.test.pptx");

            try
            {
                //Instantiate a Presentation object that represents a presentation file
                using (Presentation pres = new Presentation(fileStream))
                {

                    HtmlOptions htmlOpt = new HtmlOptions();
                    htmlOpt.HtmlFormatter = HtmlFormatter.CreateDocumentFormatter("custom.css", false);

                    var path = Directory.GetCurrentDirectory().ToString() + @"\doomo.html";
                    //Saving the presentation to HTML
                    pres.Save(Directory.GetCurrentDirectory().ToString() + @"\doomo.html", Aspose.Slides.Export.SaveFormat.Html, htmlOpt);

                    

                }

                

            }
            catch (Exception ex)
            {
                PptEx = new MvcApplication6.Filters.PptException(thread, ex.Message);
            }
            finally
            {
                thread.Abort();
            }
        }


    }
}
