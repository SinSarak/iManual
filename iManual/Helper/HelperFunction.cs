using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace iManual.Helper
{
    public class HelperFunction
    {
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string GenerateToken(int size)
        {
            // Characters except I, l, O, 1, and 0 to decrease confusion when hand typing tokens
            var charSet = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789-+*$%!#";
            var chars = charSet.ToCharArray();
            var data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(size);
            foreach (var b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
        public static string ImageToBase64(Image image,
               System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }


        public static string ConvertFileImagetoBase64(HttpPostedFileBase file)
        {
            System.IO.Stream fs = file.InputStream;
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            return base64String;
        }

        public string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static DateTime[] splitRangeDate(string dates, char sign, string originalFormat)   //"MM/dd/yyyy"
        {
            DateTime[] range = new DateTime[2];
            try
            {
                if (!string.IsNullOrEmpty(dates))
                {
                    string[] splited = dates.Split(sign);


                    if (splited.Length >= 2)
                    {

                        range[0] = DateTime.ParseExact(splited[0].Trim(), originalFormat, CultureInfo.InvariantCulture);
                        range[1] = DateTime.ParseExact(splited[1].Trim(), originalFormat, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        range[0] = DateTime.Now;
                        range[1] = DateTime.Now;
                    }
                }
                else
                {
                    range[0] = DateTime.Now;
                    range[1] = DateTime.Now;
                }
            }
            catch (Exception)
            {
                range[0] = DateTime.Now;
                range[1] = DateTime.Now;
                return range;
            }
            return range;
        }

        public static List<int> JsonListInt(string json)
        {
            return JsonConvert.DeserializeObject<List<int>>(json);
        }


        public static DateTime? CheckStringToNullableDatetime(string datetime, string originalFormat) //"MM/dd/yyyy"
        {
            DateTime? result = null;
            try
            {
                if (!string.IsNullOrEmpty(datetime))
                {
                    result = DateTime.ParseExact(datetime, originalFormat, CultureInfo.InvariantCulture);
                }
            }
            catch (Exception) { }
            return result;
        }

        public static DateTime PostStringtoDate(DateTime date)
        {
            try
            {
                return DateTime.Parse(date.Date.ToString("MM/dd/yyyy"));
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        public static DateTime PostStringtoDate(String date)
        {
            try
            {
                return DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        //Horng Sothearith
        #region "Data member for encrypt"
        static readonly string PasswordHash = "DYn@M!cPha$m!";
        static readonly string SaltKey = "Dyn@m!C#";
        static readonly string VIKey = "Dy9Nam!c@aD$m!N1";
        #endregion
        public static string EnHash(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 16);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);

        }
        public static string DeHash(string encryptedText)
        {
            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
                byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 16);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string TimeSpanToMinute(TimeSpan ts)
        {
            string time;
            if (ts.Days != 0)
            {
                time = ts.Days + " Day " + ts.Hours.ToString() + " hr " + ts.Minutes + " Min ago";
            }
            else if (ts.Hours != 0)
            {
                time = ts.Hours.ToString() + " hr " + ts.Minutes + " Min ago";
            }
            else if (ts.Minutes != 0)
            {
                time = ts.Minutes.ToString() + " Min ago" + ts.Seconds + " Sec ago";
            }
            else if (ts.Seconds != 0)
            {
                time = ts.Seconds.ToString() + " Sec ago";
            }
            else
            {
                time = "Now";
            }
            return time;
        }
        //End Horng Sothearith

        public static int isInteger(string num)
        {
            int i = 0;
            if (int.TryParse(num, out i))
            {
                return i;
            }
            return 0;
        }

        //Helper Hub Task in  TaskController
        //public static void HelpHubTask(RegTask task, out int id, out int notiId, out string fromuser, out string time, out string taskname, out int star)
        //{
        //    fromuser = task.FromUser.ToUpperInvariant();
        //    time = "";
        //    taskname = "";
        //    star = task.Priority;
        //    id = task.Id;
        //    notiId = task.NotificationId;
        //    //time
        //    System.TimeSpan timeSpan = new TimeSpan();
        //    if (task.Notifications.NoteDate != null)
        //    {
        //        timeSpan = DateTime.Now.Subtract((DateTime)task.Notifications.NoteDate);
        //        time = ITMS.Classes.Helper.TimeSpanToMinute(timeSpan);
        //    }
        //    else
        //    {
        //        time = "Now";
        //    }

        //    //task name
        //    if (task.TaskCategories != null)
        //    {
        //        taskname = task.TaskCategories.CateName;
        //    }
        //    else
        //    {
        //        taskname = "NULL";
        //    }
        //}

        public static bool isEmail(string e)
        {
            var regex = new Regex(@"[a-z0-9A-Z!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9A-Z!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9A-Z](?:[a-zA-Z0-9-]*[a-z0-9A-Z])?\.)+[a-z0-9A-Z](?:[a-zA-Z0-9-]*[a-z0-9A-Z])?");
            return regex.IsMatch(e);
        }
        public static bool isWebsite(string w)
        {
            var regex = new Regex(@"^(http:\/\/www.|https:\/\/www.|ftp:\/\/www.|www.){1}([0-9A-Za-z]+\.)");
            return regex.IsMatch(w);
        }
        public static bool isPassword(string w)
        {
            var regex = new Regex(@"^[a-z0-9A-Z!@#$%&]");
            return regex.IsMatch(w);
        }
        public static string toLower(string n)
        {
            return n.ToLower();
        }
        public static string toUpper(string n)
        {
            return n.ToUpper();
        }
        public static string RemoveSpace(string n)
        {
            return n.Replace(" ", String.Empty);
        }
        public static string UppercaseFirstEach(string s)
        {
            char[] a = s.ToLower().ToCharArray();

            for (int i = 0; i < a.Count(); i++)
            {
                a[i] = i == 0 || a[i - 1] == ' ' ? char.ToUpper(a[i]) : a[i];

            }

            return new string(a);
        }

        public static bool CheckBoolNullable(bool? value)
        {
            if (value != null)
            {
                return (bool)value;
            }
            else
            {
                return false;
            }
        }
        public static int CheckIntNullable(int? value)
        {
            if (value != null)
            {
                return (int)value;
            }
            else
            {
                return 0;
            }
        }
        public static int CheckStringNulltoIntable(string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    return int.Parse(value);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        //Sin Sarak 2017.09.02


        public static String CheckNullDate(DateTime dt)
        {
            if (dt == default(DateTime))
            {
                return "0001-01-01";
            }
            return dt.ToString("yyyy-MM-dd");
        }


        public static string CheckNullString(string s)
        {
            if (s == null)
            {
                s = "";
            }
            return s;
        }

        //Sin Sarak 2017.08.03
        public static string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
        public static string ListObjectsToJSONWithJSONNet(List<object> lists)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(lists);
        }

        public static string ClassToJSONWithJSONNet(object obj)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(obj);
        }
        //public static Person[] JsonToArray(json)
        //{
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    Person[] persons = js.Deserialize<Person[]>(json);
        //}


        //Sin Sarak 2017.10.25
        public static bool CheckExtImage(string filename)
        {
            string ext = System.IO.Path.GetExtension(filename).ToLower();
            string[] exts = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg", ".jfif", ".jpeg 2000", ".exif", ".tiff", ".ppm", ".pgm", ".pbm", ".pnm", ".webp", ".hdr", ".bpg", ".3d", ".cgm" };
            if (exts.Contains(ext))
            {
                return true;
            }
            return false;
        }
        public static bool CheckExtPDF(string filename)
        {
            string ext = System.IO.Path.GetExtension(filename).ToLower();
            string[] exts = { ".pdf" };
            if (exts.Contains(ext))
            {
                return true;
            }
            return false;
        }
        public static bool CheckExtWord(string filename)
        {
            string ext = System.IO.Path.GetExtension(filename).ToLower();
            string[] exts = { ".docx" };
            if (exts.Contains(ext))
            {
                return true;
            }
            return false;
        }
        public static bool CheckExtVideo(string filename)
        {
            string ext = System.IO.Path.GetExtension(filename).ToLower();
            string[] exts = { ".webm", ".mkv", ".flv", ".mpeg", ".mpeg-4", ".vob", ".ogv", ".ogg", ".gifv", ".mng", ".avi", ".mov", ".wmv", ".yuv", ".rm", ".rmvb", ".asf", ".amv", ".mp4", ".mp4g", ".mpv", ".mpg", ".m2v", ".mpeg", ".m4v", ".svi", ".3gp", ".mxf", ".roq", ".nsv", ".flv", ".f4v", ".f4p", ".f4a", ".f4b" };
            if (exts.Contains(ext))
            {
                return true;
            }
            return false;
        }
        /*tp ( Type print ), ttp ( type title print)
         * 
         * tp can equal  0 (New Request User ), 1 ( User Request 2 or more types of request ) ,2 (Specific Request )
         * 
         * tp = 0 || 1 , ttp must be null
         * tp = 2 , ttp must have Text as string which contains either string in WebConfig (UserRequestPrintTitle)
         * 
         * WebConfig(UserRequestPrintTitle).value = NEW USER REQUEST FORM,USER REQUEST FORM,SOFTWARE REQUEST FORM,FOLDER and FILE PERMISSION REQUEST FORM,EMAIL REQUEST FORM,TELEPHONE LINE REQUEST FORM
         */
        //public static string GetTitleUserRequestPrint(int tp, string ttp)
        //{
        //    string title = "";
        //    List<string> titles = AppSetting.ListUserRequestPrintTitle;
        //    try
        //    {
        //        int _tp = Helper.CheckIntNullable(tp);

        //        switch (_tp)
        //        {
        //            case 0:
        //                //New User Request
        //                title = titles[0];
        //                break;
        //            case 2:
        //                //Specific Request
        //                if (!string.IsNullOrEmpty(ttp))
        //                {
        //                    title = titles.SingleOrDefault(p => p.ToUpper().Contains(ttp.ToUpper()));
        //                }
        //                else
        //                {
        //                    title = titles[1];
        //                }
        //                break;
        //            case 1:
        //                //User Request
        //                title = titles[1];
        //                break;
        //            default:
        //                title = titles[1];
        //                break;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return titles[1];
        //    }
        //    return title;
        //}

        public static string CheckExt(string filename)
        {
            string type = "";
            string ext = System.IO.Path.GetExtension(filename).ToLower();
            string[] extImage = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg", ".jfif", ".jpeg 2000", ".exif", ".tiff", ".ppm", ".pgm", ".pbm", ".pnm", ".webp", ".hdr", ".bpg", ".3d", ".cgm" };
            string[] extVideo = { ".webm", ".mkv", ".flv", ".mpeg", ".mpeg-4", ".vob", ".ogv", ".ogg", ".gifv", ".mng", ".avi", ".mov", ".wmv", ".yuv", ".rm", ".rmvb", ".asf", ".amv", ".mp4", ".mp4g", ".mpv", ".mpg", ".m2v", ".mpeg", ".m4v", ".svi", ".3gp", ".mxf", ".roq", ".nsv", ".flv", ".f4v", ".f4p", ".f4a", ".f4b" };
            string[] extWord = { ".docx" };
            string[] extPDF = { ".pdf" };
            if (extImage.Contains(ext))
            {
                type = "image";
            }
            else if (extPDF.Contains(ext))
            {
                type = "pdf";
            }
            else if (extVideo.Contains(ext))
            {
                type = "video";
            }
            else if (extWord.Contains(ext))
            {
                type = "word";
            }

            return type;
        }




        public static bool CheckNullStrings(params string[] strings)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                if (strings[i] == null)
                    return true;
            }
            return false;
        }
        public static bool CheckZeroValues(params float[] intvalue)
        {
            for (int i = 0; i < intvalue.Length; i++)
            {
                if (intvalue[i] == 0)
                    return true;
            }
            return false;
        }

        public static string[] HrefSpliter(string url)
        {
            string[] result = new string[2] { "", "" };
            if (!string.IsNullOrEmpty(url))
            {
                url = url.Substring(1);
                string[] parts = url.Split('/');
                if (parts.Length > 2)
                {
                    result[0] = parts[0];
                    for (int i = 1; i < parts.Length; i++)
                    {
                        result[1] += parts[i];
                    }
                }
                else if (parts.Length > 1)
                {
                    result = parts;
                }

            }
            return result;
        }

        public static string GenerateOTP()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";
            string symbel = "!@#$%&";
            var valid = false;

            string characters = numbers;
            characters += alphabets + symbel + small_alphabets + numbers;
            int length = 8;
            string otp;
            do
            {
                otp = string.Empty;
                for (int i = 0; i < length; i++)
                {
                    string character = string.Empty;
                    do
                    {
                        int index = new Random().Next(0, characters.Length);
                        character = characters.ToCharArray()[index].ToString();
                    } while (otp.IndexOf(character) != -1);
                    otp += character;
                }

               // valid = Helper.isPassword(otp);
            } while (!valid);
            return otp;

        }
    }
}