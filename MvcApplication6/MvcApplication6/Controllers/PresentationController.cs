using Aspose.Slides;
using Aspose.Slides.Export;
using Bussines;
using Data;
using MvcApplication6.Filters;
using MvcApplication6.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using WebMatrix.WebData;

namespace MvcApplication6.Controllers
{
    [InitializeSimpleMembership]
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

        public IEnumerable<Customer> GetCustomers()
        {
             using(var ent = new Entities<Customer>())
            {
                var result = ent.GetCustomers();
                return result.ToList();
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

        public ActionResult Modal(long pptId)
        {
            ViewBag.Id = pptId;
            return PartialView();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Presenation Id</param>
        /// <returns></returns>
        public ActionResult Broadcast(int id)
        {
            ViewBag.Id = id;
            using (var ent = new Entities<Broadcast>())
            {
                //  ent.Add(newPrez);.
                var result = ent.TotalSlideCount(id);
                ViewBag.SlideCount = ent.TotalSlideCount(id);
            }
            return View();
        }


        Thread thread;
        [HandleError]
        [HandleError()]
        private  void generatePPT(Stream fileStream)
        {
            // The path to the documents directory.
          //  string dataDir = Path.GetFullPath("ss.test.pptx");

            try
            {
                //Instantiate a Presentation object that represents a presentation file
                using (Aspose.Slides.Presentation pres = new Aspose.Slides.Presentation(fileStream))
                {
                    HtmlOptions htmlOpt = new HtmlOptions();
                    htmlOpt.HtmlFormatter = HtmlFormatter.CreateDocumentFormatter("custom.css", false);

                    var path = @"C:\\CourseWork\\doomo.html";
                    //Saving the presentation to HTML
                    pres.Save(path, Aspose.Slides.Export.SaveFormat.Html, htmlOpt); 

                    using(StreamReader sr = new StreamReader(path))
                    {
                        //String line = sr.ReadToEnd();//await sr.ReadToEndAsync();
                        //var txt = line;
                        
                    }

                    var html = new HtmlDocument();
                    html.Load(path);

                    var document = html.DocumentNode;
                    var SVGs = document.Descendants("svg");

                    Data.Presentation newPrez = new Data.Presentation();
                    newPrez.Created = DateTime.UtcNow;
                    var usr = User.Identity.Name;
                    newPrez.Owner = (int)WebSecurity.GetUserId(User.Identity.Name);
                    //UserProfile usr = int mu1 = 
                    //newPrez.Pages = new Data.Slide();

                    List<Data.Slide> slides = new List<Data.Slide>();

                    int counter = 0;
                    HtmlNode bckgrnd = HtmlNode.CreateNode("");
                    foreach (var item in SVGs)
                    {
                        item.Attributes.Remove("width");
                        item.Attributes.Remove("height");
                        var lastNode = item.LastChild;
                        //var decendatds = item.Descendants();
                        //var ggg = item.ChildNodes["g"];
                        //var ttt = ggg.ChildNodes.Where(x => x.Name == "text");

                        var nodesToDel = item.ChildNodes["g"].ChildNodes.Where(x => x.Name == "text").ToList();

                        foreach (var node in nodesToDel)
                        {
                            item.ChildNodes["g"].RemoveChild(node);
                        }

                        if (counter == 0)
                            bckgrnd = item.ChildNodes["defs"];
                        else
                            item.ChildNodes.Insert(1, bckgrnd);
                        //item.ChildNodes.Insert(0,


                        slides.Add(new Data.Slide { Presentation = newPrez, SlideNo = counter, Text = item.OuterHtml });
                        
                        //string fileName = "right" + counter +".txt";
                        //FileStream fs = null;
                        //try
                        //{
                        //    fs = new FileStream(@"C:\\CourseWork\\"+fileName, FileMode.CreateNew);
                        //    using (StreamWriter writer = new StreamWriter(fs))
                        //    {
                        //        writer.Write(item.OuterHtml);
                        //    }
                        //}
                        //finally
                        //{
                        //    if (fs != null)
                        //        fs.Dispose();
                        //}
                        newPrez.Pages = slides.ToList();

                       
                        counter++;
                    }

                    //var dbprez = newPrez;
                    //using (var ent = new Repository<Data.Presentation>())
                    //{
                    //    ent.CreatePresentation(newPrez);
                    //}
                    using (var ent = new Entities<Data.Presentation>())
                    {
                      //  ent.Add(newPrez);
                        ent.CreatePresentation(newPrez);
                    }

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
