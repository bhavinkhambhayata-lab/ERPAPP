using ERPAPP.Helper;
using ERPAPP.Interfaces;
using ERPAPP.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;

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
        public List<MailListDto> GetMailCustomerUnBlockList(string division)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Division", (object?)division ?? DBNull.Value)
            };

            DataTable dt = _db.GetDataTable("Customer_CustomerUnblockResuestMailListNew", parameters);

            List<MailListDto> list = new List<MailListDto>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MailListDto
                {
                    TOMailID = row["TOMailID"]?.ToString(),
                    CCMailID = row["CCMailID"]?.ToString()
                });
            }

            return list;
        }

        public async Task<bool> SendMailCustomerUnBlock(string division, CustomerEmailItemDto approvalItem)
        {
            try
            {
                // ✅ Validation
                if (approvalItem == null)
                    return false;

                var mailList = GetMailCustomerUnBlockList(division);

                if (mailList == null || mailList.Count == 0)
                    return false;

                // ✅ FROM MAIL
                string fromMail = approvalItem.MailID;

                if (string.IsNullOrWhiteSpace(fromMail))
                    return false;

                // ✅ COLLECT EMAILS
                List<string> toEmails = new List<string>();
                List<string> ccEmails = new List<string>();

                foreach (var item in mailList)
                {
                    if (!string.IsNullOrWhiteSpace(item.TOMailID))
                        toEmails.AddRange(item.TOMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(item.CCMailID))
                        ccEmails.AddRange(item.CCMailID.Split(','));
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
            body.Append("<tr bgcolor='#d3d3d3'><th>Name</th><th>Vendor Code</th><th>Division</th></tr>");

            body.Append("<tr>");
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

        public async Task<bool> SendMailVendorUnBlock(string division, VendorEmailItemDto approvalData)
        {
            try
            {
                // ✅ Validation
                if (approvalData == null)
                    return false;

                var mailList = GetMailVendorUnBlockList(division);

                if (mailList == null || mailList.Count == 0)
                    return false;

                // ✅ FROM MAIL
                string fromMail = approvalData.MailID;

                if (string.IsNullOrWhiteSpace(fromMail))
                    return false;

                // ✅ COLLECT EMAILS
                List<string> toEmails = new List<string>();
                List<string> ccEmails = new List<string>();

                foreach (var item in mailList)
                {
                    if (!string.IsNullOrWhiteSpace(item.TOMailID))
                        toEmails.AddRange(item.TOMailID.Split(','));

                    if (!string.IsNullOrWhiteSpace(item.CCMailID))
                        ccEmails.AddRange(item.CCMailID.Split(','));
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

        public List<MailListDto> GetMailVendorUnBlockList(string division)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Division", (object?)division ?? DBNull.Value)
            };

            DataTable dt = _db.GetDataTable("Vendor_VendorUnblockRequestMailListNew", parameters);

            List<MailListDto> list = new List<MailListDto>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MailListDto
                {
                    TOMailID = row["TOMailID"]?.ToString(),
                    CCMailID = row["CCMailID"]?.ToString()
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
                    Subject = "Customer Block Request", // 🔥 change subject
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
    }
}
