using elFinder.AspNet;
using elFinder.AspNet.Drawing;
using elFinder.AspNet.Drivers.FileSystem;
using elFinder.AspNet.Models;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Mvc;
using TayanaYacht.Models;
using TayanaYacht.Models.Yacht;
using TayanaYacht.Models.News;
using TayanaYacht.Models.Dealer;

namespace TayanaYacht.Areas.Admin.Controllers
{
    [RoutePrefix("el-finder/file-system")]
    public class FileSystemController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();
        private static readonly HashSet<string> AllowedTags = new HashSet<string>
        {
        "HtmlEditorPicture", "YachtBannerPicture", "YachtGalleryPicture", "YachtLayoutPicture",
        "NewsBannerPicture", "NewsCoverPicture", "DealerBannerPicture", "DealerCoverPicture","ContactBannerPicture", "File"
        };

        [HttpGet, Route("connector")]
        public async Task<ActionResult> Connector(string type, int? id, string tag)
        {
            var folder = GetMediaFolder(type, id, tag);
            if (folder == null || type == null) return new HttpStatusCodeResult(400);
            var connector = GetConnector(folder, type, id.Value, tag);

            var parameters = Request.QueryString.AllKeys.ToDictionary(k => k, k => new StringValues(Request.QueryString.GetValues(k)));

            var result = (await connector.ProcessAsync(parameters)).Value;
            if (result is FileContent)
            {
                var file = result as FileContent;
                return File(file.ContentStream, file.ContentType);
            }
            else
            {
                return Content(JsonSerializer.Serialize(result), "application/json");
            }
        }

        [HttpPost, Route("connector")]
        public async Task<ActionResult> ConnectorPost(string type, int? id, string tag)
        {
            var folder = GetMediaFolder(type, id, tag);
            if (folder == null || type == null) return new HttpStatusCodeResult(400);
            var connector = GetConnector(folder, type, id.Value, tag);

            var parameters = Request.Form.Keys.Cast<string>().ToDictionary(k => k, v => new StringValues(Request.Form[v]));

            if (Request.Files.Count > 0)
            {
                var files = new List<FileContent>();
                //https://stackoverflow.com/questions/1760510/foreach-on-request-files
                foreach (string fileName in Request.Files)
                {
                    var file = Request.Files[fileName];
                    files.Add(new FileContent
                    {
                        Length = file.ContentLength,
                        ContentStream = file.InputStream,
                        ContentType = file.ContentType,
                        FileName = file.FileName
                    });
                }
                return Content(JsonSerializer.Serialize((await connector.ProcessAsync(parameters, files)).Value), "application/json");
            }
            else
            {
                return Content(JsonSerializer.Serialize((await connector.ProcessAsync(parameters)).Value), "application/json");
            }
        }

        [HttpGet, Route("thumb/{type}/{id:int}/{tag}/{hash}")]
        public async Task<ActionResult> Thumbs(string hash, string type, int? id, string tag)
        {
            var folder = GetMediaFolder(type, id, tag);
            if (folder == null || type == null) return new HttpStatusCodeResult(400);
            var connector = GetConnector(folder, type, id.Value, tag);

            var result = (await connector.GetThumbnailAsync(hash)).Value;
            if (result is ImageWithMimeType)
            {
                var file = result as ImageWithMimeType;
                return File(file.ImageStream, file.MimeType);
            }
            else
            {
                return Content(JsonSerializer.Serialize(result), "application/json");
            }
        }

        private Connector GetConnector(string folder, string type, int id, string tag)
        {
            var driver = new FileSystemDriver();
            //取得網站協定 scheme+host+port+authority+path+query
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            //path開始
            string imageUrl = baseUrl
                + Url.Content("~/Files/" + type + "/" + id + "/" + tag + "/");

            string thumbUrl = baseUrl
                + Url.Content("~/el-finder/file-system/thumb/" + type + "/" + id + "/" + tag + "/");


            var root = new RootVolume(
                folder,
                imageUrl,
                thumbUrl
            )
            {
                //IsReadOnly = !User.IsInRole(AccountController.SuperUser)
                IsReadOnly = false, // Can be readonly according to user's membership permission
                IsLocked = false, // If locked, files and directories cannot be deleted, renamed or moved
                Alias = "Files", // Beautiful name given to the root/home folder
                MaxUploadSizeInKb = 10240       //MaxUploadSizeInKb = 2048, // Limit imposed to user uploaded file <= 2048 KB
                                                //LockedFolders = new List<string>(new string[] { "Folder1" })
            };

            driver.AddRoot(root);

            return new Connector(driver)
            {
                // This allows support for the "onlyMimes" option on the client.
                MimeDetect = MimeDetectOption.Internal
            };
        }
        // 通過就回傳這艘遊艇的資料夾路徑，不通過就回傳 null
        private string GetMediaFolder(string type, int? id, string tag)
        {
            //TODO: 這裡要檢查使用者是否有權限存取這個資料夾，否則就回傳 null
            if (id == null || type == null)
            {
                return null;
            }
            string folderName = "";
            string tagName = "";

            switch (type)
            {
                case "Yacht":
                    Yacht yacht = db.Yachts.Find(id.Value);
                    if (yacht == null || yacht.DeletedAt != null)
                    {
                        return null;
                    }
                    folderName = type;
                    break;
                case "News":
                    News news = db.News.Find(id.Value);
                    if (news == null || news.DeletedAt != null)
                    {
                        return null;
                    }
                    folderName = type;
                    break;
                case "NewsPage":
                    if (id != 1)
                    {
                        return null;
                    }
                    folderName = type;
                    break;
                case "Dealer":
                    Dealer dealer = db.Dealers.Find(id.Value);
                    if (dealer == null)
                    {
                        return null;
                    }
                    folderName = type;
                    break;
                case "DealerPage":
                    if (id != 1)
                    {
                        return null;
                    }
                    folderName = type;
                    break;
                case "ContactPage":
                    if (id != 1)
                    {
                        return null;
                    }
                    folderName = type;
                    break;
                default:
                    return null;
            }
            if (!AllowedTags.Contains(tag))
            {
                return null;
            }
            tagName = tag;
            string rootPath = Path.GetFullPath(Server.MapPath("~/Files"));
            string fullPath = Path.GetFullPath(Path.Combine(rootPath, folderName, id.Value.ToString(), tagName));
            //網站目錄+\,不依照大小寫比較
            if (!fullPath.StartsWith(rootPath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            Directory.CreateDirectory(fullPath);
            return fullPath;

        }
    }
}
