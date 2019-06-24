using System.Configuration;
using System.Web.Configuration;

namespace ClpQrColoring.Config.Sections
{
    // https://haacked.com/archive/2007/03/12/custom-configuration-sections-in-3-easy-steps.aspx/
    public class MailViaGraphSection : ConfigurationSection
    {
        private static MailViaGraphSection section
            = WebConfigurationManager.GetSection("myMailViaOAuthSettings/mail_clp_noreply") as MailViaGraphSection;
        public static MailViaGraphSection Section
        {
            get { return section; }
        }


        [ConfigurationProperty("authority", IsRequired = true)]
        public string Authority
        {
            get { return this["authority"] as string; }
            set { this["authority"] = value; }
        }

        [ConfigurationProperty("clientId", IsRequired = true)]
        public string ClientId
        {
            get { return this["clientId"] as string; }
            set { this["clientId"] = value; }
        }

        [ConfigurationProperty("clientAppUri", IsRequired = true)]
        public string ClientAppUri
        {
            get { return this["clientAppUri"] as string; }
            set { this["clientAppUri"] = value; }
        }

        [ConfigurationProperty("graphUrl", IsRequired = true)]
        public string GraphUrl
        {
            get { return this["graphUrl"] as string; }
            set { this["graphUrl"] = value; }
        }

        [ConfigurationProperty("fromUser", IsRequired = true)]
        public string FromUser
        {
            get { return this["fromUser"] as string; }
            set { this["fromUser"] = value; }
        }

        [ConfigurationProperty("fromPwd", IsRequired = true)]
        public string FromPwd
        {
            get { return this["fromPwd"] as string; }
            set { this["fromPwd"] = value; }
        }
    }
}