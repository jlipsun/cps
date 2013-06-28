using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;

public partial class ViewPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Get XML Data via the id
        //Open connection
        SqlConnection cnn = new SqlConnection(ConfigurationManager.AppSettings["Connection String"]);
        SqlDataReader reader;

        cnn.Open();
        var uniqueID = Request.QueryString["uniqueID"];

        SqlCommand viewRequest = new SqlCommand("viewRequest", cnn);
        viewRequest.CommandType = CommandType.StoredProcedure;
        viewRequest.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar));
        viewRequest.Parameters["@id"].Value = uniqueID;

        Response.Write("<center><img src=\"images/qtc_logo.png\" alt=\"Logo\" /></center>");
        Response.Write("<br />");
        Response.Write("<br />");
        Response.Write("<br />");
        Response.Write("<br />");
        reader = viewRequest.ExecuteReader();
        while (reader.Read())
        {
            SqlXml requestXml = reader.GetSqlXml(0);
            XmlReader requestReaderXml = requestXml.CreateReader();
            XDocument doc = XDocument.Load(requestReaderXml);
            XElement request = doc.Descendants("Request").First();
            Response.Write("<center><table>");
            showField(request);
            //IEnumerable<XElement> fields = request.Descendants();
            //Response.Write("<center><table>");
//            foreach (XElement field in fields)
//            {
//                String name = field.Name.ToString();
//                name = Regex.Replace(name, "(\\B[A-Z])", " $1");
//                String value = field.Value.ToString();
//                if (name.Equals("Phone"))
//                {
//                    if (value.Length == 11)//1-800-293-3756
//                        value = value.Substring(0, 1) + "-"
//                            + value.Substring(1, 3) + "-"
//                            + value.Substring(4, 3) + "-"
//                            + value.Substring(7, 4);
//                    else if (value.Length == 10)//800-293-3756
//                        value = value.Substring(0, 3) + "-"
//                            + value.Substring(3, 3) + "-"
//                            + value.Substring(6, 4);
//                }
//                Response.Write(@"
//                    <tr>
//                        <td align='right'><strong>" + name + @":&nbsp;&nbsp;</strong></td>
//                        <td align='left'>" + value + @"</td></tr>"
//                );
//            }
            Response.Write("</table></center>");
        }
    }

    private void showField(XElement node)
    {
        foreach (XElement child in node.Descendants())
        {
            if (!child.HasElements)
            {
                String name = child.Name.ToString();
                name = Regex.Replace(name, "(\\B[A-Z])", " $1");
                String value = child.Value.ToString();
                if (name.Equals("Phone"))
                {
                    if (value.Length == 11)//1-800-293-3756
                        value = value.Substring(0, 1) + "-"
                            + value.Substring(1, 3) + "-"
                            + value.Substring(4, 3) + "-"
                            + value.Substring(7, 4);
                    else if (value.Length == 10)//800-293-3756
                        value = value.Substring(0, 3) + "-"
                            + value.Substring(3, 3) + "-"
                            + value.Substring(6, 4);
                }
                Response.Write(@"
                        <tr>
                            <td align='right'><strong>" + name + @":&nbsp;&nbsp;</strong></td>
                            <td align='left'>" + value + @"</td></tr>"
                );
            }
        }
    }
}