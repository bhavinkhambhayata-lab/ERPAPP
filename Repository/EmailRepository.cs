using ERPAPP.Helper;
using ERPAPP.Interfaces;
using ERPAPP.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ERPAPP.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _config;
        private readonly DbHelper _db;

        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _senderEmail;
        private readonly string _username;
        private readonly string _password;

        public EmailRepository(IConfiguration config, DbHelper db)
        {
            _config = config;
            _db = db;
            _smtpServer = config["EmailSettings:SmtpServer"];
            _port = int.Parse(config["EmailSettings:Port"]);
            _senderEmail = config["EmailSettings:SenderEmail"];
            _username = config["EmailSettings:Username"];
            _password = config["EmailSettings:Password"];
        }

        #region Customer Unblock Send Email
        public List<MailListDto> GetMailCustomerUnBlockList(string division, int displayNo)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Division", (object?)division ?? DBNull.Value),
                new SqlParameter("@DisplayNo", (object?)displayNo ?? DBNull.Value)
            };

            DataTable dt = _db.GetDataTable("Customer_CustomerUnblockResuestMailListNew", parameters);

            List<MailListDto> list = new List<MailListDto>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MailListDto
                {
                    TOMailID = row["TOMailID"]?.ToString(),
                    CCMailID = row["CCMailID"]?.ToString(),
                    BCCMailID = row["BCCMailID"]?.ToString()
                });
            }

            return list;
        }

        public async Task<bool> SendMailCustomerUnBlock(string division, CustomerEmailItemDto approvalItem, int displayNo)
        {
            try
            {
                // ✅ Validation
                if (approvalItem == null)
                    return false;

                var mailList = GetMailCustomerUnBlockList(division, displayNo);

                if (mailList == null || mailList.Count == 0)
                    return false;

                // ✅ FROM MAIL
                string fromMail = "softwarecare@italiagroup.in";

                if (string.IsNullOrWhiteSpace(fromMail))
                    return false;

                // ✅ COLLECT EMAILS
                List<string> toEmails = new List<string>();
                List<string> ccEmails = new List<string>();
                List<string> bccEmails = new List<string>();

                foreach (var item in mailList)
                {
                    if (!string.IsNullOrWhiteSpace(item.TOMailID))
                        toEmails.AddRange(item.TOMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(item.CCMailID))
                        ccEmails.AddRange(item.CCMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(item.BCCMailID))
                        bccEmails.AddRange(item.BCCMailID.Split(','));
                }

                // ✅ CLEAN EMAIL LIST
                toEmails = toEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                ccEmails = ccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                bccEmails = bccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                // ⚠️ At least one TO required
                if (toEmails.Count == 0)
                    return false;

                // ✅ CREATE MAIL
                var mail = new MailMessage
                {
                    From = new MailAddress(fromMail),
                    Subject = "Customer Unblock Request",
                    Body = CustomerUnBlockBuildBody(approvalItem),
                    IsBodyHtml = true
                };

                // ✅ ADD TO
                foreach (var email in toEmails)
                {
                    mail.To.Add(new MailAddress(email)); // 🔥 validation included
                }

                // ✅ ADD CC
                foreach (var email in ccEmails)
                {
                    mail.CC.Add(new MailAddress(email));
                }

                // ✅ ADD BCC
                foreach (var email in bccEmails)
                {
                    mail.Bcc.Add(new MailAddress(email));
                }

                // ✅ SMTP CONFIG
                var smtp = new SmtpClient(_smtpServer)
                {
                    Port = _port,
                    Credentials = new NetworkCredential(_username, _password),
                    EnableSsl = true,
                    UseDefaultCredentials = false // 🔥 MUST for Gmail
                };

                // ✅ SEND MAIL
                await smtp.SendMailAsync(mail);

                return true;
            }
            catch (Exception ex)
            {
                // 🔥 Logging mukvu better (file / db / console)
                return false;
            }
        }

        private string CustomerUnBlockBuildBody(CustomerEmailItemDto item)
        {
            if (item == null)
                return "No data available";

            StringBuilder body = new StringBuilder();

            body.Append("Dear Sir/Madam,<br/><br/>");

            body.Append("Kindly process the customer unblock request.<br/><br/>");
            body.Append("Please log in to the ERP application and take the necessary action.<br/><br/>");
            body.Append("Customer Unblock Request Details:<br/><br/>");

            body.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;'>");
            body.Append("<tr bgcolor='#d3d3d3'><th>SrNo</th><th>Requested By</th><th>Customer Name</th><th>Customer Code</th><th>Division</th></tr>");

            body.Append("<tr>");
            body.Append($"<td>{item.SrNo}</td>");
            body.Append($"<td>{item.RequestedBy}</td>");
            body.Append($"<td>{item.Name}</td>");
            body.Append($"<td>{item.CustomerCode}</td>");
            body.Append($"<td>{item.Division}</td>");
            body.Append("</tr>");

            body.Append("</table>");

            body.Append("<br/><br/>");
            body.Append(GetSignature());
            body.Append("<br/>Note: Please do not reply to this mail.");

            return body.ToString();
        }

        #endregion

        #region Vendor Unblock Send Email

        public async Task<bool> SendMailVendorUnBlock(string division, VendorEmailItemDto approvalData, int displayNo)
        {
            try
            {
                // ✅ Validation
                if (approvalData == null)
                    return false;

                var mailList = GetMailVendorUnBlockList(division, displayNo);

                if (mailList == null || mailList.Count == 0)
                    return false;

                // ✅ FROM MAIL
                string fromMail = "softwarecare@italiagroup.in";

                if (string.IsNullOrWhiteSpace(fromMail))
                    return false;

                // ✅ COLLECT EMAILS
                List<string> toEmails = new List<string>();
                List<string> ccEmails = new List<string>();
                List<string> bccEmails = new List<string>();

                foreach (var item in mailList)
                {
                    if (!string.IsNullOrWhiteSpace(item.TOMailID))
                        toEmails.AddRange(item.TOMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(item.CCMailID))
                        ccEmails.AddRange(item.CCMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(item.BCCMailID))
                        bccEmails.AddRange(item.BCCMailID.Split(','));
                }

                // ✅ CLEAN EMAIL LIST
                toEmails = toEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                ccEmails = ccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                bccEmails = bccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                // ⚠️ At least one TO required
                if (toEmails.Count == 0)
                    return false;

                // ✅ CREATE MAIL
                var mail = new MailMessage
                {
                    From = new MailAddress(fromMail),
                    Subject = "Vendor Unblock Request",
                    Body = VendorUnBlockBuildBody(approvalData),
                    IsBodyHtml = true
                };

                // ✅ ADD TO
                foreach (var email in toEmails)
                {
                    mail.To.Add(new MailAddress(email)); // 🔥 validation included
                }

                // ✅ ADD CC
                foreach (var email in ccEmails)
                {
                    mail.CC.Add(new MailAddress(email));
                }

                // ✅ ADD BCC
                foreach (var email in bccEmails)
                {
                    mail.Bcc.Add(new MailAddress(email));
                }

                // ✅ SMTP CONFIG
                var smtp = new SmtpClient(_smtpServer)
                {
                    Port = _port,
                    Credentials = new NetworkCredential(_username, _password),
                    EnableSsl = true,
                    UseDefaultCredentials = false // 🔥 MUST for Gmail
                };

                // ✅ SEND MAIL
                await smtp.SendMailAsync(mail);

                return true;
            }
            catch (Exception ex)
            {
                // 🔥 Logging mukvu better (file / db / console)
                return false;
            }
        }

        public List<MailListDto> GetMailVendorUnBlockList(string division, int displayNo)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Division", (object?)division ?? DBNull.Value),
                new SqlParameter("@DisplayNo", (object?)displayNo ?? DBNull.Value)
            };

            DataTable dt = _db.GetDataTable("Vendor_VendorUnblockResuestMailListNew", parameters);

            List<MailListDto> list = new List<MailListDto>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MailListDto
                {
                    TOMailID = row["TOMailID"]?.ToString(),
                    CCMailID = row["CCMailID"]?.ToString(),
                    BCCMailID = row["BCCMailID"]?.ToString()
                });
            }

            return list;
        }

        private string VendorUnBlockBuildBody(VendorEmailItemDto item)
        {
            if (item == null)
                return "No data available";

            StringBuilder body = new StringBuilder();

            body.Append("Dear Sir/Madam,<br/><br/>");
            body.Append("Kindly process the vendor unblock request.<br/><br/>");
            body.Append("Please log in to the ERP application and take the necessary action.<br/><br/>");
            body.Append("Vendor Unblock Request Details:<br/><br/>");

            body.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;'>");

            // ✅ Header
            body.Append("<tr bgcolor='Gray'>");
            body.Append("<th>SrNo</th>");
            body.Append("<th>Requested By</th>");
            body.Append("<th>Vendor Name</th>");
            body.Append("<th>Vendor Code</th>");
            body.Append("<th>Vendor Category</th>");
            body.Append("<th>Purchaser Code/Name</th>");
            body.Append("<th>Payment Terms</th>");
            body.Append("<th>Payment Method</th>");
            body.Append("</tr>");

            // ✅ Single Row (NO LOOP 🔥)
            body.Append("<tr bgcolor='White'>");
            body.Append($"<td>{item.SrNo}</td>");
            body.Append($"<td>{item.RequestedBy}</td>");
            body.Append($"<td>{item.VendorName}</td>");
            body.Append($"<td>{item.VendorCode}</td>");
            body.Append($"<td>{item.VendorCategory}</td>");
            body.Append($"<td>{item.PurchaseCode}</td>");
            body.Append($"<td>{item.PaymentTerm}</td>");
            body.Append($"<td>{item.PaymentMethod}</td>");
            body.Append("</tr>");

            body.Append("</table>");

            body.Append("<br/><br/>");
            body.Append(GetSignature());
            body.Append("<br/>Note: Please do not reply to this mail.");

            return body.ToString();
        }

        #endregion

        #region Customer Block Send Email
        public List<MailListDto> GetMailCustomerBlockList(string division, int displayNo)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@Division", (object?)division ?? DBNull.Value),
        new SqlParameter("@DisplayNo", displayNo)
            };

            DataSet ds = _db.GetDataSet("Customer_CustomerUnblockReplayMailList", parameters);

            // Safety check
            if (ds == null || ds.Tables.Count == 0)
                return new List<MailListDto>();

            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return new List<MailListDto>();

            // TO (single)
            string toMail = dt.AsEnumerable()
                .Select(r => r["TOMailID"]?.ToString()?.Trim().TrimEnd(','))
                .FirstOrDefault();

            // CC (multiple combine)
            string ccMail = string.Join(",",
                dt.AsEnumerable()
                  .Select(r => r["CCMailID"]?.ToString()?.Trim().TrimEnd(','))
                  .Where(x => !string.IsNullOrWhiteSpace(x))
                  .Distinct()
            );

            // BCC (single)
            string bccMail = dt.AsEnumerable()
                .Select(r => r["BCCMailID"]?.ToString()?.Trim().TrimEnd(','))
                .FirstOrDefault();

            return new List<MailListDto>
    {
        new MailListDto
        {
            TOMailID = toMail,
            CCMailID = ccMail,
            BCCMailID = bccMail
        }
    };
        }

        public async Task<bool> SendMailCustomerBlock(string division, int displayNo, string customerCode, string customerName)
        {
            try
            {
                var mailList = GetMailCustomerBlockList(division, displayNo);

                if (mailList == null || mailList.Count == 0)
                    return false;

                string fromMail = "softwarecare@italiagroup.in";

                if (string.IsNullOrWhiteSpace(fromMail))
                    return false;

                List<string> toEmails = new List<string>();
                List<string> ccEmails = new List<string>();
                List<string> bccEmails = new List<string>();

                foreach (var data in mailList)
                {
                    if (!string.IsNullOrWhiteSpace(data.TOMailID))
                        toEmails.AddRange(data.TOMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(data.CCMailID))
                        ccEmails.AddRange(data.CCMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(data.BCCMailID))   // 👈 add this
                        bccEmails.AddRange(data.BCCMailID.Split(','));
                }

                toEmails = toEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                ccEmails = ccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                bccEmails = bccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                if (toEmails.Count == 0)
                    return false;

                var mail = new MailMessage
                {
                    From = new MailAddress(fromMail),
                    Subject = "Customer UnBlock Successfully", // 🔥 change subject
                    Body = CustomerBlockBuildBody(customerName, customerCode),
                    IsBodyHtml = true
                };

                foreach (var email in toEmails)
                {
                    mail.To.Add(new MailAddress(email));
                }

                foreach (var email in ccEmails)
                {
                    mail.CC.Add(new MailAddress(email));
                }

                foreach (var email in bccEmails)
                {
                    mail.Bcc.Add(new MailAddress(email));
                }

                var smtp = new SmtpClient(_smtpServer)
                {
                    Port = _port,
                    Credentials = new NetworkCredential(_username, _password),
                    EnableSsl = true,
                    UseDefaultCredentials = false
                };

                await smtp.SendMailAsync(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string CustomerBlockBuildBody(string customerName, string customerCode)
        {
            StringBuilder body = new StringBuilder();

            body.Append("Dear Sir/Madam,<br/><br/>");

            body.Append("The #CustomerName# ( #CustomerCode# ) customer has been successfully unblocked. <br/><br/>").Replace("#CustomerName#", customerName).Replace("#CustomerCode#", customerCode);
            body.Append("Please verify the same in D365.<br/><br/>");

            body.Append("<br/><br/>");
            body.Append(GetSignature());
            body.Append("<br/>Note: Please do not reply to this mail.");

            return body.ToString();
        }

        #endregion

        #region Vendor Block Send Email
        public List<MailListDto> GetMailVendorBlockList(string division, int displayNo)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Division", (object?)division ?? DBNull.Value),
                new SqlParameter("@DisplayNo", displayNo)
            };

            DataSet ds = _db.GetDataSet("Vendor_VendorUnblockReplayMailList", parameters);

            if (ds == null || ds.Tables.Count < 2)
                return new List<MailListDto>();

            DataTable dt = ds.Tables[1]; // ✅ SECOND result set

            if (dt.Rows.Count == 0)
                return new List<MailListDto>();

            var toMail = dt.AsEnumerable()
                .Select(r => r["TOMailID"]?.ToString())
                .FirstOrDefault();

            var ccMail = string.Join(",",
                dt.AsEnumerable()
                  .Select(r => r["CCMailID"]?.ToString())
                  .Where(x => !string.IsNullOrWhiteSpace(x))
                  .Distinct()
            );

            var bccMail = dt.AsEnumerable()
                .Select(r => r["BCCMailID"]?.ToString())
                .FirstOrDefault();

            return new List<MailListDto>
    {
        new MailListDto
        {
            TOMailID = toMail,
            CCMailID = ccMail,
            BCCMailID = bccMail
        }
    };
        }

        public async Task<bool> SendMailVendorBlock(string division, int displayNo, string vendorCode, string vendorName)
        {
            try
            {
                var mailList = GetMailVendorBlockList(division, displayNo);

                if (mailList == null || mailList.Count == 0)
                    return false;

                string fromMail = "softwarecare@italiagroup.in";

                if (string.IsNullOrWhiteSpace(fromMail))
                    return false;

                List<string> toEmails = new List<string>();
                List<string> ccEmails = new List<string>();
                List<string> bccEmails = new List<string>();

                foreach (var data in mailList)
                {
                    if (!string.IsNullOrWhiteSpace(data.TOMailID))
                        toEmails.AddRange(data.TOMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(data.CCMailID))
                        ccEmails.AddRange(data.CCMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(data.BCCMailID))   // 👈 add this
                        bccEmails.AddRange(data.BCCMailID.Split(','));
                }

                toEmails = toEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                ccEmails = ccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                bccEmails = bccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                if (toEmails.Count == 0)
                    return false;

                var mail = new MailMessage
                {
                    From = new MailAddress(fromMail),
                    Subject = "Vendor UnBlock Successfully", // 🔥 changed
                    Body = VendorBlockBuildBody(vendorCode, vendorName),
                    IsBodyHtml = true
                };

                foreach (var email in toEmails)
                {
                    mail.To.Add(new MailAddress(email));
                }

                foreach (var email in ccEmails)
                {
                    mail.CC.Add(new MailAddress(email));
                }

                foreach (var email in bccEmails)
                {
                    mail.Bcc.Add(new MailAddress(email));
                }

                var smtp = new SmtpClient(_smtpServer)
                {
                    Port = _port,
                    Credentials = new NetworkCredential(_username, _password),
                    EnableSsl = true,
                    UseDefaultCredentials = false
                };

                await smtp.SendMailAsync(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string VendorBlockBuildBody(string vendorCode, string vendorName)
        {

            StringBuilder body = new StringBuilder();

            body.Append("Dear Sir/Madam,<br/><br/>");

            body.Append("The #VendorName# ( #VendorCode# ) vendor has been successfully unblocked. <br/><br/>").Replace("#VendorName#", vendorName).Replace("#VendorCode#", vendorCode);
            body.Append("Please verify the same in D365.<br/><br/>");

            body.Append(GetSignature());
            body.Append("<br/><br/>Note: Please do not reply to this email.");

            return body.ToString();
        }

        #endregion

        private string GetSignature()
        {
            return @"<p>Thank you for your support.</p>
                 <b style='color:#EA7513'>ITALIA Group</b>";
        }


        public VendorLocationEmail GetVendorSendEmailDetailByLocationCode(string code)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@Code", (object?)code ?? DBNull.Value)
            };

            DataTable dt = _db.GetDataTable("GetVendorSendEmailDetailByLocationCode", parameters);

            if (dt == null || dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new VendorLocationEmail
            {
                Code = row["Code"]?.ToString(),
                EmpRowID = row["EmpRowID"]?.ToString()
            };
        }

        public async Task<bool> SendMailFixedAssetUnBlock(FixedAssetEmailItem model, int displayNo)
        {
            try
            {
                // ✅ Validation
                if (model == null)
                    return false;

                var mailList = GetMailFixedAssetUnBlockList(displayNo);

                if (mailList == null || mailList.Count == 0)
                    return false;

                // ✅ FROM MAIL
                string fromMail = "softwarecare@italiagroup.in";

                if (string.IsNullOrWhiteSpace(fromMail))
                    return false;

                // ✅ COLLECT EMAILS
                List<string> toEmails = new List<string>();
                List<string> ccEmails = new List<string>();
                List<string> bccEmails = new List<string>();

                foreach (var item in mailList)
                {
                    if (!string.IsNullOrWhiteSpace(item.TOMailID))
                        toEmails.AddRange(item.TOMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(item.CCMailID))
                        ccEmails.AddRange(item.CCMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(item.BCCMailID))   // 👈 add this
                        bccEmails.AddRange(item.BCCMailID.Split(','));
                }

                // ✅ CLEAN EMAIL LIST
                toEmails = toEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                ccEmails = ccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                bccEmails = bccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                // ⚠️ At least one TO required
                if (toEmails.Count == 0)
                    return false;

                // ✅ CREATE MAIL
                var mail = new MailMessage
                {
                    From = new MailAddress(fromMail),
                    Subject = "Fixed Asset Unblock Request",
                    Body = FixedAssetUnBlockBuildBody(model),
                    IsBodyHtml = true
                };

                // ✅ ADD TO
                foreach (var email in toEmails)
                {
                    mail.To.Add(new MailAddress(email));
                }

                // ✅ ADD CC
                foreach (var email in ccEmails)
                {
                    mail.CC.Add(new MailAddress(email));
                }

                foreach (var email in bccEmails)
                {
                    mail.Bcc.Add(new MailAddress(email));
                }

                // ✅ SMTP CONFIG
                var smtp = new SmtpClient(_smtpServer)
                {
                    Port = _port,
                    Credentials = new NetworkCredential(_username, _password),
                    EnableSsl = true,
                    UseDefaultCredentials = false
                };

                // ✅ SEND MAIL
                await smtp.SendMailAsync(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<MailListDto> GetMailFixedAssetUnBlockList(int displayNo)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@DisplayNo", displayNo)
            };

            DataTable dt = _db.GetDataTable("FixedAsset_UnblockRequestMailList", parameters);

            List<MailListDto> list = new List<MailListDto>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MailListDto
                {
                    TOMailID = row["TOMailID"]?.ToString(),
                    CCMailID = row["CCMailID"]?.ToString(),
                    BCCMailID = row["BCCMailID"]?.ToString()
                });
            }

            return list;
        }

        private string FixedAssetUnBlockBuildBody(FixedAssetEmailItem item)
        {
            if (item == null)
                return "No data available";

            StringBuilder body = new StringBuilder();

            body.Append("Dear Sir/Madam,<br/><br/>");
            body.Append("Kindly process the Fixed Asset unblock request.<br/><br/>");
            body.Append("Please log in to the ERP application and take the necessary action.<br/><br/>");

            body.Append("Fixed Asset Unblock Request Details:<br/><br/>");

            body.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;'>");

            // ✅ Header
            body.Append("<tr bgcolor='Gray'>");
            body.Append("<th>SrNo</th>");
            body.Append("<th>Requested By</th>");
            body.Append("<th>Description</th>");
            body.Append("<th>Fixed Asset No</th>");
            body.Append("<th>Division</th>");
            body.Append("</tr>");

            // ✅ Single Row
            body.Append("<tr bgcolor='White'>");
            body.Append($"<td>{item.SrNo}</td>");
            body.Append($"<td>{item.RequestedBy}</td>");
            body.Append($"<td>{item.Description}</td>");
            body.Append($"<td>{item.FixedAssetNo}</td>");
            body.Append($"<td>{item.Division}</td>");
            body.Append("</tr>");

            body.Append("</table>");

            body.Append("<br/><br/>");
            body.Append(GetSignature());
            body.Append("<br/>Note: Please do not reply to this mail.");

            return body.ToString();
        }

        public List<MailListDto> GetMailFixedAssetBlockList(int displayNo)
        {

            SqlParameter[] parameters = new SqlParameter[]
             {
                new SqlParameter("@DisplayNo", displayNo)
             };

            DataSet ds = _db.GetDataSet("FixedAsset_UnblockReplayMailList", parameters);

            // Safety check
            if (ds == null || ds.Tables.Count == 0)
                return new List<MailListDto>();

            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return new List<MailListDto>();

            // TO (single)
            string toMail = dt.AsEnumerable()
                .Select(r => r["TOMailID"]?.ToString()?.Trim().TrimEnd(','))
                .FirstOrDefault();

            // CC (multiple combine)
            string ccMail = string.Join(",",
                dt.AsEnumerable()
                  .Select(r => r["CCMailID"]?.ToString()?.Trim().TrimEnd(','))
                  .Where(x => !string.IsNullOrWhiteSpace(x))
                  .Distinct()
            );

            // BCC (single)
            string bccMail = dt.AsEnumerable()
                .Select(r => r["BCCMailID"]?.ToString()?.Trim().TrimEnd(','))
                .FirstOrDefault();

            return new List<MailListDto>
    {
        new MailListDto
        {
            TOMailID = toMail,
            CCMailID = ccMail,
            BCCMailID = bccMail
        }
    };
        }

        public async Task<bool> SendMailFixedAssetBlock(int displayNo, string fixedAssetCode, string fixedDescription)
        {
            try
            {
                var mailList = GetMailFixedAssetBlockList(displayNo);

                if (mailList == null || mailList.Count == 0)
                    return false;

                string fromMail = "softwarecare@italiagroup.in";

                if (string.IsNullOrWhiteSpace(fromMail))
                    return false;

                List<string> toEmails = new List<string>();
                List<string> ccEmails = new List<string>();
                List<string> bccEmails = new List<string>();

                foreach (var data in mailList)
                {
                    if (!string.IsNullOrWhiteSpace(data.TOMailID))
                        toEmails.AddRange(data.TOMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(data.CCMailID))
                        ccEmails.AddRange(data.CCMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(data.BCCMailID))   // 👈 add this
                        bccEmails.AddRange(data.BCCMailID.Split(','));
                }

                toEmails = toEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                ccEmails = ccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                bccEmails = bccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                if (toEmails.Count == 0)
                    return false;

                var mail = new MailMessage
                {
                    From = new MailAddress(fromMail),
                    Subject = "Fixed Asset UnBlock Successfully",
                    Body = FixedAssetBlockBuildBody(fixedAssetCode, fixedDescription),
                    IsBodyHtml = true
                };

                foreach (var email in toEmails)
                {
                    mail.To.Add(new MailAddress(email));
                }

                foreach (var email in ccEmails)
                {
                    mail.CC.Add(new MailAddress(email));
                }

                foreach (var email in bccEmails)
                {
                    mail.Bcc.Add(new MailAddress(email));
                }

                var smtp = new SmtpClient(_smtpServer)
                {
                    Port = _port,
                    Credentials = new NetworkCredential(_username, _password),
                    EnableSsl = true,
                    UseDefaultCredentials = false
                };

                await smtp.SendMailAsync(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string FixedAssetBlockBuildBody(string fixedAssetNo, string fixedAssetDescription)
        {
            StringBuilder body = new StringBuilder();

            body.Append("Dear Sir/Madam,<br/><br/>");

            body.Append("The #FADescription# ( #FAAssetNo# ) fixed asset has been successfully unblocked. <br/><br/>")
                .Replace("#FADescription#", fixedAssetDescription)
                .Replace("#FAAssetNo#", fixedAssetNo);

            body.Append("Please verify the same in D365.<br/><br/>");

            body.Append(GetSignature());
            body.Append("<br/><br/>Note: Please do not reply to this email.");

            return body.ToString();
        }

        public async Task<List<FGItemEmailDetailModel>> GetFGItemsEmailList(string itemCodes)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@ItemCode", itemCodes)
            };

            DataSet ds = _db.GetDataSet("GetFGItemEmailList", param);

            List<FGItemEmailDetailModel> list = new List<FGItemEmailDetailModel>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new FGItemEmailDetailModel
                    {
                        DisplayNo = row["DisplayNo"] != DBNull.Value ? Convert.ToInt32(row["DisplayNo"]) : 0,

                        Description = row["Description"]?.ToString(),
                        Description2 = row["Description2"]?.ToString(),

                        BaseUnitOfMeasure = row["BaseUnitOfMeasure"]?.ToString(),
                        ItemCategoryCode = row["ItemCategoryCode"]?.ToString(),

                        GrossWeight = row["GrossWeight"] != DBNull.Value
                            ? Convert.ToDecimal(row["GrossWeight"])
                            : (decimal?)null,

                        NetWeight = row["NetWeight"] != DBNull.Value
                            ? Convert.ToDecimal(row["NetWeight"])
                            : (decimal?)null,

                        RoundingPrecision = row["RoundingPrecision"] != DBNull.Value
                            ? Convert.ToDecimal(row["RoundingPrecision"])
                            : 0,

                        SalesUnitOfMeasure = row["SalesUnitOfMeasure"]?.ToString(),
                        PurchUnitOfMeasure = row["PurchUnitOfMeasure"]?.ToString(),
                        ReplenishmentSystem = row["ReplenishmentSystem"]?.ToString(),
                        ItemTrackingCode = row["ItemTrackingCode"]?.ToString(),
                        CostingMethod = row["CostingMethod"]?.ToString(),
                        GenProdPostingGroup = row["GenProdPostingGroup"]?.ToString(),
                        VATProdPostingGroup = row["VATProdPostingGroup"]?.ToString(),
                        InventoryPostingGroup = row["InventoryPostingGroup"]?.ToString(),
                        ProductionBOMNo = row["ProductionBOMNo"]?.ToString(),
                        RoutingNo = row["RoutingNo"]?.ToString(),
                        ManufacturingPolicy = row["ManufacturingPolicy"]?.ToString(),
                        ReorderingPolicy = row["ReorderingPolicy"]?.ToString(),

                        Category = row["Category"]?.ToString(),
                        SizeOfTile = row["SizeOfTile"]?.ToString(),
                        Collection = row["Collection"]?.ToString(),
                        SurfaceFinishOrGlaze = row["SurfaceFinishOrGlaze"]?.ToString(),
                        GlazeEffect = row["GlazeEffect"]?.ToString(),
                        DesignColor = row["DesignColor"]?.ToString(),
                        ColourFamily = row["ColourFamily"]?.ToString(),
                        TypeOfTile = row["TypeOfTile"]?.ToString(),
                        Packaging = row["Packaging"]?.ToString(),
                        Thickness = row["Thickness"]?.ToString(),
                        Body = row["Body"]?.ToString(),
                        PLCollection = row["PLCollection"]?.ToString(),
                        PLColours = row["PLColours"]?.ToString(),
                        MovementType = row["MovementType"]?.ToString(),
                        TypeOfProduct = row["TypeOfProduct"]?.ToString(),

                        GSTGroupCode = row["GSTGroupCode"]?.ToString(),
                        GSTCredit = row["GSTCredit"]?.ToString(),
                        HSNSACCode = row["HSNSACCode"]?.ToString(),

                        GradeItemCode = row["GradeItemCode"]?.ToString(),
                        Brand = row["Brand"]?.ToString(),
                        Grade = row["Grade"]?.ToString(),
                        GradeLinkCode = row["GradeLinkCode"]?.ToString(),

                        CreatedBy = row["CreatedBy"]?.ToString(),
                        CreatedOn = row["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(row["CreatedOn"]) : (DateTime?)null,
                        ModifiedBy = row["ModifiedBy"]?.ToString(),
                        ModifiedOn = row["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(row["ModifiedOn"]) : (DateTime?)null

                    });
                }
            }

            return await Task.FromResult(list);
        }

        public async Task<List<ERPEmailConfigurationModel>> GetFGItemMailConfigurationList(string createdUser)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ModuleName", "FGItems"),
                new SqlParameter("@Type", "Domestic"),
                new SqlParameter("@CreatedUser", createdUser)
            };

            DataTable dt = _db.GetDataTable("GetERPEmailConfiguration", parameters);

            List<ERPEmailConfigurationModel> list = new List<ERPEmailConfigurationModel>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ERPEmailConfigurationModel
                {
                    Id = row["Id"] != DBNull.Value ? Convert.ToInt32(row["Id"]) : 0,
                    ModuleName = row["ModuleName"]?.ToString(),
                    Type = row["Type"]?.ToString(),
                    FromEmail = row["FromEmail"]?.ToString(),
                    ToEmails = row["ToEmails"]?.ToString(),
                    CCEmails = row["CCEmails"]?.ToString(),
                    BCCEmails = row["BCCEmails"]?.ToString(),
                    IsActive = row["IsActive"] != DBNull.Value && Convert.ToBoolean(row["IsActive"])
                });
            }

            return list;
        }

        public async Task<ERPEmailConfigurationModel> GetERPEmailConfiguration(string moduleName, string type)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@ModuleName", moduleName),
                new SqlParameter("@Type", type)
            };

            DataSet ds = _db.GetDataSet("GetERPEmailConfiguration", param);

            ERPEmailConfigurationModel model = new ERPEmailConfigurationModel();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                model.Id = row["Id"] != DBNull.Value ? Convert.ToInt32(row["Id"]) : 0;
                model.ModuleName = row["ModuleName"]?.ToString();
                model.Type = row["Type"]?.ToString();
                model.FromEmail = row["FromEmail"]?.ToString();
                model.ToEmails = row["ToEmails"]?.ToString();
                model.CCEmails = row["CCEmails"]?.ToString();
                model.BCCEmails = row["BCCEmails"]?.ToString();
                model.IsActive = row["IsActive"] != DBNull.Value && Convert.ToBoolean(row["IsActive"]);
            }

            return await Task.FromResult(model);
        }


        public async Task<bool> SendFGItemCreationMail(List<FGItemEmailDetailModel> itemList, string createdUser)
        {
            try
            {
                if (itemList == null || itemList.Count == 0)
                    return false;

                var mailList = await GetFGItemMailConfigurationList(createdUser);

                if (mailList == null || mailList.Count == 0)
                    return false;

                string fromMail = mailList?.FirstOrDefault()?.FromEmail ?? string.Empty;

                List<string> toEmails = new List<string>();
                List<string> ccEmails = new List<string>();
                List<string> bccEmails = new List<string>();

                foreach (var item in mailList)
                {
                    if (!string.IsNullOrWhiteSpace(item.ToEmails))
                        toEmails.AddRange(item.ToEmails.Split(','));

                    if (!string.IsNullOrWhiteSpace(item.CCEmails))
                        ccEmails.AddRange(item.CCEmails.Split(','));

                    if (!string.IsNullOrWhiteSpace(item.BCCEmails))
                        bccEmails.AddRange(item.BCCEmails.Split(','));
                }

                toEmails = toEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                ccEmails = ccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                bccEmails = bccEmails
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();

                if (toEmails.Count == 0)
                    return false;

                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromMail);
                    mail.Subject = $"FG Items Created Successfully";
                    mail.Body = FGItemBuildBody(itemList);
                    mail.IsBodyHtml = true;

                    foreach (var email in toEmails)
                        mail.To.Add(email);

                    foreach (var email in ccEmails)
                        mail.CC.Add(email);

                    foreach (var email in bccEmails)
                        mail.Bcc.Add(email);

                    using (var smtp = new SmtpClient(_smtpServer))
                    {
                        smtp.Port = _port;
                        smtp.Credentials = new NetworkCredential(_username, _password);
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;

                        await smtp.SendMailAsync(mail);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string FGItemBuildBody(List<FGItemEmailDetailModel> items)
        {
            StringBuilder body = new StringBuilder();

            body.Append("Dear All,<br/><br/>");
            body.Append("The FG items have been successfully created in the system.\r\nPlease find below attached the list of the newly created FG items for your reference.<br/><br/>");

            body.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;'>");

            body.Append("<tr style='background-color:#D9D9D9;'>");
            body.Append("<th>Sr No</th>");
            body.Append("<th>Item No</th>");
            body.Append("<th>Description</th>");
            body.Append("<th>Description 2</th>");
            body.Append("<th>Size Of Tile</th>");
            body.Append("<th>Category</th>");
            body.Append("<th>Grade</th>");
            body.Append("<th>Brand</th>");
            body.Append("<th>Packaging</th>");
            body.Append("<th>Thickness</th>");
            body.Append("<th>Gen Prod Posting Group</th>");
            body.Append("<th>Created By</th>");
            body.Append("</tr>");

            int srNo = 1;

            foreach (var item in items)
            {
                body.Append("<tr>");
                body.Append($"<td>{srNo}</td>");
                body.Append($"<td>{item.GradeItemCode}</td>");
                body.Append($"<td>{item.Description}</td>");
                body.Append($"<td>{item.Description2}</td>");
                body.Append($"<td>{item.SizeOfTile}</td>");
                body.Append($"<td>{item.Category}</td>");
                body.Append($"<td>{item.Grade}</td>");
                body.Append($"<td>{item.Brand}</td>");
                body.Append($"<td>{item.Packaging}</td>");
                body.Append($"<td>{item.Thickness}</td>");
                body.Append($"<td>{item.GenProdPostingGroup}</td>");
                body.Append($"<td>{item.CreatedBy}</td>");
                body.Append("</tr>");

                srNo++;
            }

            body.Append("</table>");

            body.Append("<br/><br/>");
            body.Append(GetSignature());
            body.Append("<br/>Note: Please do not reply to this mail.");

            return body.ToString();
        }
    }
}
